using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.Context;
using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ClinicDbContext _context;

    public UserRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateGuestUserAsync(string name, string surname, string email, string phoneNumber)
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
        return await _context.SaveChangesAsync().ContinueWith(_ => user);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        //TODO pamietac ze podzszywac sie moze ktos i trzba potwierzdzic mailowo 
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email, int excludeId)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != excludeId);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == id)
            .Select(u => new UserDto()
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

    public async Task<object> GetAllUsersAsync(int page, int pageSize, string? searchTerm = null)
    {
        var query = _context.Users
            .Where(u => u.RolesId == 3 || u.RolesId == 4);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(u =>
                u.Name!.ToLower().Contains(searchTerm) ||
                u.Surname!.ToLower().Contains(searchTerm) ||
                u.Email!.ToLower().Contains(searchTerm) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(searchTerm))
            );
        }

        query = query.OrderBy(u => u.Surname).ThenBy(u => u.Name);

        var totalCount = await query.CountAsync();
        var users = await query
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto()
            {
                Email = u.Email,
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                PhoneNumber = u.PhoneNumber,
                RoleName = u.Roles.Name,
                PhotoURL = u.PhotoURL
            })
            .ToListAsync();

        return new { users, totalCount };
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        _context.CompletedAppointments.RemoveRange(
            _context.CompletedAppointments.Where(ca => ca.UserId == userId));
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}