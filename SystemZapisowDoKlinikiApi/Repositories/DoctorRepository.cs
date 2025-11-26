using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicDbContext _context;

    public DoctorRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task AddDoctorAsync(AddDoctorDto addDoctorDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var newDoctor = new Doctor
            {
                UserId = addDoctorDto.UserId,
                SpecializationPl = addDoctorDto.SpecializationPl,
                SpecializationEn = addDoctorDto.SpecializationEn,
                ImgPath = addDoctorDto.ImageUrl
            };
            await _context.Doctors.AddAsync(newDoctor);
            
            var user = await _context.Users.FindAsync(addDoctorDto.UserId);
            if (user != null)
            {
                user.RolesId = 1;
                _context.Users.Update(user);
            }
            
            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task DeleteDoctorAsync(int doctorId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null)
                throw new Exception("Doctor not found");
            
            var user = await _context.Users.FindAsync(doctor.UserId);
            if (user != null)
            {
                user.RolesId = 3;
                _context.Users.Update(user);
            }

            _context.Doctors.Remove(doctor);
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task<User?> GetDoctorByIdAsync(int doctorId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == doctorId); 
    }
    
    public async Task<IEnumerable<User>> GetDoctorsByServiceAsync(int serviceId)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Roles.Services.Any(s => s.Id == serviceId))
            .Include(u => u.Doctor) 
            .ToListAsync();;
    }
}