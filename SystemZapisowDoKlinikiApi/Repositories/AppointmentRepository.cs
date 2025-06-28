using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicDbContext _context;
    private readonly IServiceRepository _serviceRepository;

    public AppointmentRepository(ClinicDbContext context, IServiceRepository serviceRepository)
    {
        _context = context;
        _serviceRepository = serviceRepository;
    }

    public async Task CreateAppointmentAsync(AppointmentRequest appointmentRequest, string role)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var appointment = new Appointment
            {
                DoctorBlockId = appointmentRequest.DoctorBlockId,
                UserId = appointmentRequest.UserId,
            };
            var service = await _serviceRepository.GetServiceByIdAsync(appointmentRequest.Service.Id);
            if (service == null)
            {
                throw new ArgumentException("Service not found.");
            }

            appointment.Services.Add(service);
            
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang)
    {
        return await _context.Appointments
            .Where(a => a.UserId == userId)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                DoctorBlockId = a.DoctorBlockId,
                UserId = a.UserId,
                Services = a.Services.Select(s => new ServiceDTO
                {
                    Id = s.Id,
                    Name = s.ServicesTranslations
                        .Where(st => st.LanguageCode == lang)
                        .Select(st => st.Name)
                        .FirstOrDefault(),
                    MinTime = s.MinTime,
                    LowPrice = s.LowPrice,
                    HighPrice = s.HighPrice,
                    Description = s.ServicesTranslations
                        .Where(st => st.LanguageCode == lang)
                        .Select(st => st.Description)
                        .FirstOrDefault(),
                    LanguageCode = lang
                }).ToList()
            })
            .ToListAsync();
    }
}