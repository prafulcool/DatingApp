using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API;

public class LikesController(ILikesRespository likesRespository) : BaseApiController
{
    [HttpPost("{targetUserId:int}")]
    public async Task<IActionResult> ToggleLike(int targetUserId)
    {
        var sourceUserId = User.GetUserId();
        if (sourceUserId == targetUserId)
            return BadRequest("You cannot like yourself");

        var existingLike = await likesRespository.GetUserLike(sourceUserId, targetUserId);
        if (existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId,
            };

            likesRespository.AddLike(like);
        }
        else
        {
            likesRespository.DeleteLike(existingLike);
        }

        if (await likesRespository.SaveChanges()) return Ok();

        return BadRequest("Failed to update like");
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
    {
        return Ok(await likesRespository.GetCurrentUserLikeIds(User.GetUserId()));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await likesRespository.GetUserLikes(likesParams);
        Response.AddPaginationHeader(users);

        return Ok(users);
    }
}