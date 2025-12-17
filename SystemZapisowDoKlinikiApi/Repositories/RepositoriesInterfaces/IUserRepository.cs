using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IUserRepository
{
    public Task<int> CreateGuestUserAsync(string name, string surname, string email, string phoneNumber);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<UserDTO?> GetUserByIdAsync(int id);
    public Task<User?> GetByIdAsync(int id);
    public Task UpdateUserAsync(User user);
    Task<ICollection<UserDTO>> GetAllUsersAsync();
}