using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<int> CreateGuestUserAsync(string name, string surname, string email, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("All user details must be provided.");
        }

        return _userRepository.CreateGuestUserAsync(name, surname, email, phoneNumber);
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        return _userRepository.GetUserByEmailAsync(email);
    }
}