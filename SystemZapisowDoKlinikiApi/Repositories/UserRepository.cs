using Microsoft.EntityFrameworkCore;
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
        return await _context.SaveChangesAsync().ContinueWith(t => user.Id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        //TODO pamietac ze podzszywac sie moze ktos i trzba potwierzdzic mailowo 
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}