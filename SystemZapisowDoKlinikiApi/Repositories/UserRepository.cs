using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ClinicDbContext _context;

    public UserRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateGuestUserAsync(string name, string surname, string email, string phoneNumber)
    {
        var user = new User
        {
            Name = name,
            Surname = surname,
            Email = email,
            PhoneNumber = phoneNumber,
            RolesId = 4
        };
        _context.Users.Add(user);
        return await _context.SaveChangesAsync().ContinueWith(_ => user.Id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        //TODO pamietac ze podzszywac sie moze ktos i trzba potwierzdzic mailowo 
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<UserDTO?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == id)
            .Select(u => new UserDTO()
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                RoleName = u.Roles.Name,
                PhotoURL = u.PhotoURL
            })
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<UserDTO>> GetAllUsersAsync()
    {
        return await _context.Users
            .Where(u => u.RolesId == 3 || u.RolesId == 4)
            .Select(u => new UserDTO()
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
            })
            .ToListAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        try
        {
            var affectedRows = await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteDeleteAsync();
            if (affectedRows == 0)
            {
                throw new KeyNotFoundException("User not found");
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error deleting user", e);
        }
    }
}