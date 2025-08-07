using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);

    Task<bool> SaveAllAsync();

    Task<IEnumerable<AppUser>> GetUsersAsync();

    Task<AppUser?> GetUserByUserIDAsync(int id);

    Task<AppUser?> GetUserByUserNameAsync(string username);
    //Task<IEnumerable<MemberDto>> GetMembersAsync();

    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<MemberDto?> GetMembersAsync(string username);
}