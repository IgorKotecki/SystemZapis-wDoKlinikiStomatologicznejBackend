using SystemZapisowDoKlinikiApi.DTO;
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

    public Task<UserDTO?> GetUserByIdAsync(int id)
    {
        return _userRepository.GetUserByIdAsync(id);
    }

    public async Task<UserDTO?> UpdateUserAsync(int id, UserUpdateDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            return null;

        user.Name = dto.Name;
        user.Surname = dto.Surname;
        user.PhoneNumber = dto.PhoneNumber;
        user.Email = dto.email;
        user.PhotoURL = dto.PhotoURL;

        await _userRepository.UpdateUserAsync(user);

        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RoleName = user.Roles?.Name ?? ""
        };
    }

    public async Task<ICollection<UserDTO>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }
}