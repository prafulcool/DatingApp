using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace  API.Controllers;

[Authorize]
//public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
public class UsersController(IUserRepository userRepository) : BaseApiController
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
}