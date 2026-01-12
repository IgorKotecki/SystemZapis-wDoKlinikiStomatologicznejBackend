using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface IUserRepository
{
    public Task<User> CreateGuestUserAsync(string name, string surname, string email, string phoneNumber);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<UserDto?> GetUserByIdAsync(int id);
    public Task<User?> GetByIdAsync(int id);
    public Task UpdateUserAsync(User user);
    Task<object> GetAllUsersAsync(int page, int pageSize, string? searchTerm);
    public Task DeleteUserAsync(int userId);
}