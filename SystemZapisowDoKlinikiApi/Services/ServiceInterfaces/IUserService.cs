using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IUserService
{
    public Task<User> CreateGuestUserAsync(string name, string surname, string email, string phoneNumber);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<UserDTO?> GetUserByIdAsync(int id);
    public Task<UserDTO?> UpdateUserAsync(int id, UserUpdateDTO dto);
    Task<ICollection<UserDTO>> GetAllUsersAsync();
    public Task DeleteUserAsync(int userId);
}