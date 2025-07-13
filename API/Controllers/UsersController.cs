using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace  API.Controllers;

[Authorize]
//public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
public class UsersController(IUserRepository userRepository, IMapper mapper,
IPhotoService photoService) : BaseApiController
{
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    // {
    //     var users = await userRepository.GetUsersAsync();
    //     return Ok(users);
    // }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();
        //var usersToReturn = mapper.Map<IEnumerable<MemberDto>>(users);
        return Ok(users);
    }

    //[HttpGet("{id:int}")] // /api/users/1
    //[HttpGet("{username:string}")] //no need to do this by default it's string
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMembersAsync(username);
        if (user == null) return NotFound();
        //return mapper.Map<MemberDto>(user);
        return user;
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //if (username == null) return BadRequest("No username found in token");

        var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());

        if (user == null) return BadRequest("Could not find user");

        mapper.Map(memberUpdateDto, user);

        //userRepository.Update(user);

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
        if (user == null) return BadRequest("Cannot update user");
        var result = await photoService.AddPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };
        user.Photos.Add(photo);

        if (await userRepository.SaveAllAsync())
            //return mapper.Map<PhotoDto>(photo);
            return CreatedAtAction(nameof(GetUser),
            new { username = user.UserName }, mapper.Map<PhotoDto>(photo));
        return BadRequest("problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
        if (user == null) return BadRequest("Could not find user");
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");
        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;
        if (await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Problem setting main photo");
    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
        if (user == null) return BadRequest("User not found");
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");
        if (photo.PublicId != null)
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }
        user.Photos.Remove(photo);
        if (await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Probleming deleting photo");
    }

}