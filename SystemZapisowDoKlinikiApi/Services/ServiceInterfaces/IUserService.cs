using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

public interface IUserService
{
    public Task<User> CreateGuestUserAsync(string name, string surname, string email, string phoneNumber);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<UserDto?> GetUserByIdAsync(int id);
    public Task<UserDto?> UpdateUserAsync(int id, UserUpdateDto dto);
    Task<object> GetAllUsersAsync(int page, int pageSize, string? searchTerm);
    public Task DeleteUserAsync(int userId);
}