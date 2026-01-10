using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using SystemZapisowDoKlinikiApi.Controllers;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Exceptions;
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

    public async Task CreateAppointmentGuestAsync(BookAppointmentRequestDTO appointmentRequest, int userId)
    {
        await BookAppointment(appointmentRequest, userId);
    }

    private async Task BookAppointment(BookAppointmentRequestDTO appointmentRequest, int userId)
    {
        await using IDbContextTransaction transaction =
            await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var duration = 0;
            var guid = Guid.NewGuid();
            var services = await _context.Services
                .Where(s => appointmentRequest.ServicesIds.Contains(s.Id))
                .ToListAsync();
            if (services.IsNullOrEmpty())
            {
                throw new BusinessException("SERVICES_NOT_FOUND", "No services found for the provided IDs.");
            }

            foreach (var servicesId in appointmentRequest.ServicesIds)
            {
                var service = services.FirstOrDefault(s => s.Id == servicesId);
                if (service == null)
                {
                    throw new BusinessException("SERVICE_NOT_FOUND", $"Service with Id {servicesId} not found.");
                }

                for (int i = 1; i <= service.MinTime; i++)
                {
                    if (duration > appointmentRequest.Duration)
                    {
                        throw new BusinessException("APPLICATION_ERROR", "Something went wrong with booking");
                    }

                    var time = appointmentRequest.StartTime.AddMinutes(30 * duration);
                    var doctorBlockId = await _context.DoctorBlocks
                        .Where(db =>
                            db.TimeBlock.TimeStart == time && db.DoctorUserId == appointmentRequest.DoctorId)
                        .Select(db => db.Id)
                        .FirstOrDefaultAsync();

                    if (doctorBlockId == 0)
                        throw new BusinessException("BLOCK_NOT_AVAILABLE",
                            $"No available doctor block for time {time}");

                    var isOccupied = await _context.Appointments
                        .AnyAsync(a => a.DoctorBlockId == doctorBlockId);

                    if (isOccupied)
                        throw new BusinessException("BLOCK_BOOKED", $"Doctor block for time {time} is already booked.");

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
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang)
    {
        var appointments = await GetAppointmentsQuery()
            .Where(a => a.UserId == userId)
            .ToListAsync();

        return appointments
            .GroupBy(a => a.AppointmentGroupId)
            .Select(group => MapToAppointmentDto(group, lang))
            .ToList();
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId, DateTime date,
        string lang)
    {
        var startOfWeek = date.Date;
        var endOfWeek = date.AddDays(7).Date;

        var appointments = await GetAppointmentsQuery()
            .Where(a => a.DoctorBlock.DoctorUserId == doctorId &&
                        a.DoctorBlock.TimeBlock.TimeStart.Date >= startOfWeek &&
                        a.DoctorBlock.TimeBlock.TimeStart.Date < endOfWeek)
            .ToListAsync();

        return appointments
            .GroupBy(a => a.AppointmentGroupId)
            .Select(group => MapToAppointmentDto(group, lang))
            .ToList();
    }

    public async Task BookAppointmentForRegisteredUserAsync(int userId,
        BookAppointmentRequestDTO bookAppointmentRequestDto)
    {
        await BookAppointment(bookAppointmentRequestDto, userId);
    }

    public async Task AddInfoToAppointmentAsync(AddInfoToAppointmentDto addInfoToAppointmentDto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var appointments = await _context.Appointments
                .Where(a => a.AppointmentGroupId == addInfoToAppointmentDto.Id)
                .Include(a => a.AdditionalInformations)
                .ToListAsync();

            if (!appointments.Any())
            {
                throw new BusinessException("NO_APPOINTMENT",
                    "No appointments found for the provided appointment ID.");
            }

            List<AdditionalInformation> addInformation;

            if (addInfoToAppointmentDto.AddInformationIds == null || !addInfoToAppointmentDto.AddInformationIds.Any())
            {
                addInformation = new List<AdditionalInformation>();
            }
            else
            {
                addInformation = await _context.AdditionalInformations
                    .Where(ai => addInfoToAppointmentDto.AddInformationIds.Contains(ai.Id))
                    .ToListAsync();

                if (addInformation.Count != addInfoToAppointmentDto.AddInformationIds.Count)
                {
                    var missingIds = addInfoToAppointmentDto.AddInformationIds
                        .Except(addInformation.Select(ai => ai.Id))
                        .ToList();
                    throw new BusinessException("PARTIAL_ADD_INFO",
                        $"Some additional information IDs were not found: {string.Join(", ", missingIds)}");
                }
            }

            foreach (var appointment in appointments)
            {
                appointment.AdditionalInformations = addInformation.ToList();
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

    public async Task UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var appointments = await _context.Appointments
                .Where(a => a.AppointmentGroupId == updateAppointmentStatusDto.AppointmentId)
                .ToListAsync();

            if (appointments.IsNullOrEmpty())
            {
                throw new BusinessException("NO_APPOINTMENT",
                    "No appointments found for the provided appointment group ID.");
            }

            foreach (var appointment in appointments)
            {
                appointment.AppointmentStatusId = updateAppointmentStatusDto.StatusId;
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

    public async Task<ICollection<AppointmentDto>> GetAppointmentsForReceptionistAsync(DateTime mondayDate, string lang)
    {
        var startOfWeek = mondayDate.Date;
        var endOfWeek = startOfWeek.AddDays(7).Date;

        var appointments = await GetAppointmentsQuery()
            .Where(a => a.DoctorBlock.TimeBlock.TimeStart.Date >= startOfWeek &&
                        a.DoctorBlock.TimeBlock.TimeStart.Date < endOfWeek)
            .ToListAsync();

        return appointments
            .GroupBy(a => a.AppointmentGroupId)
            .Select(group => MapToAppointmentDto(group, lang))
            .ToList();
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByDateAsync(string lang, DateTime date)
    {
        var appointments = await GetAppointmentsQuery()
            .Where(a => a.DoctorBlock.TimeBlock.TimeStart.Date == date.Date)
            .ToListAsync();

        return appointments
            .GroupBy(a => a.AppointmentGroupId)
            .Select(group => MapToAppointmentDto(group, lang))
            .ToList();
    }


    private IQueryable<Appointment> GetAppointmentsQuery()
    {
        return _context.Appointments
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
            .Include(a => a.AdditionalInformations)
            .AsSplitQuery();
    }

    public Task<AddInformationOutDto> GetAddInformationByIdAsync(int id, string lang)
    {
        return _context.AdditionalInformations
            .Where(ai => ai.Id == id)
            .Select(ai => new AddInformationOutDto
            {
                Id = ai.Id,
                Body = lang == "pl" ? ai.BodyPl : ai.BodyEn
            })
            .FirstOrDefaultAsync()!;
    }

    private AppointmentDto MapToAppointmentDto(IGrouping<string?, Appointment> group, string lang)
    {
        var orderedBlocks = group.OrderBy(a => a.DoctorBlock.TimeBlock.TimeStart).ToList();
        var firstBlock = orderedBlocks.First();
        var lastBlock = orderedBlocks.Last();

        var patient = firstBlock.User;
        var doctor = firstBlock.DoctorBlock.DoctorUser.User;

        return new AppointmentDto
        {
            User = MapToUserDto(patient),
            AppointmentGroupId = group.Key!,
            StartTime = firstBlock.DoctorBlock.TimeBlock.TimeStart,
            EndTime = lastBlock.DoctorBlock.TimeBlock.TimeEnd,
            Doctor = MapToUserDto(doctor),
            Services = MapServices(group, lang),
            Status = GetLocalizedText(firstBlock.AppointmentStatus.NamePl, firstBlock.AppointmentStatus.NameEn, lang),
            AdditionalInformation = MapAdditionalInformation(firstBlock.AdditionalInformations, lang)
        };
    }

    private UserDTO MapToUserDto(User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    private List<ServiceDTO> MapServices(IGrouping<string?, Appointment> group, string lang)
    {
        return group
            .SelectMany(a => a.Services)
            .DistinctBy(s => s.Id)
            .Select(s => new ServiceDTO
            {
                Id = s.Id,
                LowPrice = s.LowPrice,
                HighPrice = s.HighPrice,
                MinTime = s.MinTime,
                LanguageCode = lang,
                Name = s.ServicesTranslations
                    .FirstOrDefault(t => t.LanguageCode == lang)
                    ?.Name,
                Catergories = s.ServiceCategories
                    .Select(c => GetLocalizedText(c.NamePl, c.NameEn, lang))
                    .ToList()
            })
            .ToList();
    }

    private List<AddInformationOutDto> MapAdditionalInformation(ICollection<AdditionalInformation> additionalInfos,
        string lang)
    {
        return additionalInfos
            .Select(ai => new AddInformationOutDto
            {
                Id = ai.Id,
                Body = GetLocalizedText(ai.BodyPl, ai.BodyEn, lang)
            })
            .ToList();
    }

    private string GetLocalizedText(string plText, string enText, string lang)
    {
        return lang == "pl" ? plText : enText;
    }
}