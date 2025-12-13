using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using SystemZapisowDoKlinikiApi.Controllers;
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
        var appointments = await _context.Appointments
            .Include(a => a.Services)
            .ThenInclude(s => s.ServicesTranslations)
            .Include(a => a.Services)
            .ThenInclude(s => s.ServiceCategories)
            .Include(a => a.AppointmentStatus)
            .Include(a => a.DoctorBlock)
            .ThenInclude(db => db.DoctorUser)
            .ThenInclude(du => du.User)
            .Include(a => a.User)
            .Include(a => a.DoctorBlock)
            .ThenInclude(db => db.TimeBlock)
            .Where(a => a.UserId == userId)
            .ToListAsync();
        var groupedAppointments = appointments
            .GroupBy(a => a.AppointmentGroupId);
        var result = new List<AppointmentDto>();
        foreach (var group in groupedAppointments)
        {
            var orderedBlocks = group.OrderBy(a => a.DoctorBlock.TimeBlock.TimeStart).ToList();

            var dto = new AppointmentDto
            {
                User = new UserDTO
                {
                    Id = group.First().User.Id,
                    Name = group.First().User.Name,
                    Surname = group.First().User.Surname,
                    Email = group.First().User.Email,
                    PhoneNumber = group.First().User.PhoneNumber
                },
                AppointmentGroupId = group.Key!,
                StartTime = orderedBlocks.First().DoctorBlock.TimeBlock.TimeStart,
                EndTime = orderedBlocks.Last().DoctorBlock.TimeBlock.TimeEnd,
                Doctor = new UserDTO
                {
                    Name = group.First().DoctorBlock.DoctorUser.User.Name,
                    Surname = group.First().DoctorBlock.DoctorUser.User.Surname,
                    Email = group.First().DoctorBlock.DoctorUser.User.Email
                },
                Services = group
                    .SelectMany(a => a.Services)
                    .Select(s => new ServiceDTO
                    {
                        Id = s.Id,
                        LowPrice = s.LowPrice,
                        HighPrice = s.HighPrice,
                        MinTime = s.MinTime,
                        LanguageCode = lang,
                        Name = s.ServicesTranslations
                            .Where(t => t.LanguageCode == lang)
                            .Select(t => t.Name)
                            .FirstOrDefault(),
                        Catergories = s.ServiceCategories
                            .Select(c => lang == "pl" ? c.NamePl : c.NameEn)
                            .ToList()
                    })
                    .DistinctBy(s => s.Id) // wymaga System.Linq, usuwa duplikaty usług
                    .ToList(),

                Status = lang == "pl"
                    ? group.First().AppointmentStatus.NamePl
                    : group.First().AppointmentStatus.NameEn
            };
            result.Add(dto);
        }

        return result;
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId, DateTime date,
        string lang)
    {
        var appointments = await _context.Appointments
            .Include(a => a.Services)
            .ThenInclude(s => s.ServicesTranslations)
            .Include(a => a.Services)
            .ThenInclude(s => s.ServiceCategories)
            .Include(a => a.AppointmentStatus)
            .Include(a => a.DoctorBlock)
            .ThenInclude(db => db.DoctorUser)
            .ThenInclude(du => du.User)
            .Include(a => a.User)
            .Include(a => a.DoctorBlock)
            .ThenInclude(db => db.TimeBlock)
            .Where(a => a.DoctorBlock.DoctorUserId == doctorId) // filtr po lekarzu
            .ToListAsync();

        var groupedAppointments = appointments
            .GroupBy(a => a.AppointmentGroupId);

        var result = new List<AppointmentDto>();

        foreach (var group in groupedAppointments)
        {
            var orderedBlocks = group.OrderBy(a => a.DoctorBlock.TimeBlock.TimeStart).ToList();

            var dto = new AppointmentDto
            {
                User = new UserDTO
                {
                    Id = group.First().User.Id,
                    Name = group.First().User.Name,
                    Surname = group.First().User.Surname,
                    Email = group.First().User.Email,
                    PhoneNumber = group.First().User.PhoneNumber
                },
                AppointmentGroupId = group.Key!,
                StartTime = orderedBlocks.First().DoctorBlock.TimeBlock.TimeStart,
                EndTime = orderedBlocks.Last().DoctorBlock.TimeBlock.TimeEnd,
                Doctor = new UserDTO
                {
                    Name = group.First().DoctorBlock.DoctorUser.User.Name,
                    Surname = group.First().DoctorBlock.DoctorUser.User.Surname,
                    Email = group.First().DoctorBlock.DoctorUser.User.Email
                },
                Services = group
                    .SelectMany(a => a.Services)
                    .Select(s => new ServiceDTO
                    {
                        Id = s.Id,
                        LowPrice = s.LowPrice,
                        HighPrice = s.HighPrice,
                        MinTime = s.MinTime,
                        LanguageCode = lang,
                        Name = s.ServicesTranslations
                            .Where(t => t.LanguageCode == lang)
                            .Select(t => t.Name)
                            .FirstOrDefault(),
                        Catergories = s.ServiceCategories
                            .Select(c => lang == "pl" ? c.NamePl : c.NameEn)
                            .ToList()
                    })
                    .DistinctBy(s => s.Id)
                    .ToList(),
                Status = lang == "pl"
                    ? group.First().AppointmentStatus.NamePl
                    : group.First().AppointmentStatus.NameEn
            };

            result.Add(dto);
        }

        return result;
    }

    public async Task<bool> BookAppointmentForRegisteredUserAsync(int userId,
        BookAppointmentRequestDTO bookAppointmentRequestDto)
    {
        await using IDbContextTransaction transaction =
            await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var duration = 0;
            var guid = Guid.NewGuid();
            var services = await _context.Services
                .Where(s => bookAppointmentRequestDto.ServicesIds.Contains(s.Id))
                .ToListAsync();
            if (services.IsNullOrEmpty())
            {
                throw new Exception("No services found for the provided IDs.");
            }

            foreach (var servicesId in bookAppointmentRequestDto.ServicesIds)
            {
                var service = services.FirstOrDefault(s => s.Id == servicesId);
                if (service == null)
                {
                    throw new Exception($"Service with Id {servicesId} not found.");
                }

                for (int i = 1; i <= service.MinTime; i++)
                {
                    if (duration > bookAppointmentRequestDto.Duration)
                    {
                        throw new Exception("Somthing went wrong with booking");
                    }

                    var time = bookAppointmentRequestDto.StartTime.AddMinutes(30 * duration);
                    var doctorBlockId = await _context.DoctorBlocks
                        .Where(db =>
                            db.TimeBlock.TimeStart == time && db.DoctorUserId == bookAppointmentRequestDto.DoctorId)
                        .Select(db => db.Id)
                        .FirstOrDefaultAsync();

                    if (doctorBlockId == 0)
                        throw new Exception($"No available doctor block for time {time}");

                    var isOccupied = await _context.Appointments
                        .AnyAsync(a => a.DoctorBlockId == doctorBlockId);

                    if (isOccupied)
                        throw new Exception($"Doctor block for time {time} is already booked.");

                    var appointment = new Appointment()
                    {
                        UserId = userId,
                        DoctorBlockId = doctorBlockId,
                        AppointmentGroupId = guid.ToString(),
                        AppointmentStatusId = 1,
                        Services = new List<Service>()
                    };
                    duration++;
                    appointment.Services.Add(service);
                    await _context.Appointments.AddAsync(appointment);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Database Exception cannot book appointment", ex);
        }
    }

    public async Task CreateAddInformationAsync(AddInformationDto addInformationDto)
    {
        var addInformation = new AdditionalInformation
        {
            BodyEn = addInformationDto.BodyEn,
            BodyPl = addInformationDto.BodyPl
        };
        _context.AdditionalInformations.Add(addInformation);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang)
    {
        var addInformations = await _context.AdditionalInformations.ToListAsync();
        var result = addInformations.Select(ai => new AddInformationOutDto
        {
            Id = ai.Id,
            Body = lang == "pl" ? ai.BodyPl : ai.BodyEn
        }).ToList();
        return result;
    }
}