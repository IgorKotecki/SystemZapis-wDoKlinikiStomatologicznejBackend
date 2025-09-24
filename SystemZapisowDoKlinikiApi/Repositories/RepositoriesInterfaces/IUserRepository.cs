using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IUserRepository
{
    public Task<int> CreateGuestUserAsync(string name, string surname, string email, string phoneNumber);
    public Task<User?> GetUserByEmailAsync(string email);
}