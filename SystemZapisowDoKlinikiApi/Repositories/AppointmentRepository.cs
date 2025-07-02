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

    public async Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest, int userId)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var doctorBlock in appointmentRequest.DoctorBlockId)
            {
                var appointment = new Appointment
                {
                    DoctorBlockId = doctorBlock,
                    UserId = userId,
                    Services = new List<Service>()
                };
                var service = await _serviceRepository.GetServiceByIdAsync(appointmentRequest.Service.Id);
                if (service == null)
                {
                    throw new ArgumentException("Service not found.");
                }

                appointment.Services.Add(service);
                _context.Appointments.Add(appointment);
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

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang)
    {
        return await _context.Appointments
            .Where(a => a.UserId == userId)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                DoctorBlock = new TimeBlockDto()
                {
                    DoctorBlockId = a.DoctorBlockId,
                    isAvailable = false,
                    TimeStart = a.DoctorBlock.TimeBlock.TimeStart,
                    TimeEnd = a.DoctorBlock.TimeBlock.TimeEnd,
                    User = new UserDTO()
                    {
                        Id = a.DoctorBlock.DoctorUser.UserId,
                        Name = a.DoctorBlock.DoctorUser.User.Name,
                        Surname = a.DoctorBlock.DoctorUser.User.Surname,
                        Email = a.DoctorBlock.DoctorUser.User.Email,
                        PhoneNumber = a.DoctorBlock.DoctorUser.User.PhoneNumber
                    }
                },
                User = new UserDTO()
                {
                    Name = a.User.Name,
                    Surname = a.User.Surname,
                    Email = a.User.Email,
                    PhoneNumber = a.User.PhoneNumber,
                },
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