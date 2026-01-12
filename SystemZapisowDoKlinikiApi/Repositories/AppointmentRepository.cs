using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SystemZapisowDoKlinikiApi.Context;
using SystemZapisowDoKlinikiApi.DTO.AdditionalInformationDtos;
using SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;
using SystemZapisowDoKlinikiApi.DTO.ServiceDtos;
using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Exceptions;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Models.Enums;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicDbContext _context;

    public AppointmentRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task CreateAppointmentGuestAsync(BookAppointmentRequestDto appointmentRequest, int userId)
    {
        await BookAppointment(appointmentRequest, userId);
    }

    private async Task BookAppointment(BookAppointmentRequestDto appointmentRequest, int userId)
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

        var completedAppointments = await _context.CompletedAppointments
            .Where(ca => ca.UserId == userId).Include(completedAppointment => completedAppointment.User)
            .ToListAsync();

        var cancelledAppointments = await _context.CancelledAppointments
            .Where(ca => ca.UserId == userId)
            .ToListAsync();

        var doctorsUsers = _context.Users.Where(u => u.RolesId == 1).ToList();

        var appointmentsDto = appointments
            .GroupBy(a => a.AppointmentGroupId)
            .Select(group => MapToAppointmentDto(group, lang))
            .ToList();

        var completedAppointmentsDto = completedAppointments
            .GroupBy(a => a.AppointmentGroupId)
            .Select(group => MapToCompletedAppointmentDto(group!, doctorsUsers, lang))
            .ToList();

        var cancelledAppointmentsDto = cancelledAppointments
            .GroupBy(a => a.AppointmentGroupId)
            .Select(group => MapToCancelledAppointmentDto(group!, doctorsUsers, lang))
            .ToList();

        var result = appointmentsDto
            .Concat(completedAppointmentsDto)
            .Concat(cancelledAppointmentsDto)
            .OrderByDescending(a => a.StartTime)
            .ToList();

        return result;
    }

    private AppointmentDto MapToCompletedAppointmentDto(IGrouping<string, CompletedAppointment> group,
        List<User> doctorsUsers, string lang)
    {
        return new AppointmentDto
        {
            AppointmentGroupId = group.Key,
            User = MapToUserDto(group.First().User),
            StartTime = group.Min(a => a.StartTime),
            EndTime = group.Max(a => a.EndTime),
            Doctor = MapToUserDto(doctorsUsers.First(u => u.Id == group.First().DoctorId)),
            Services = MapServicesJson(group.Select(a => a.ServicesJson), lang),
            Status = GetLocalizedText("Zakończona", "Completed", lang),
            AdditionalInformation = MapAdditionalInformation(group.Select(a => a.AdditionalInformationJson), lang),
            Notes = group.First().Notes,
            CancellationReason = null,
        };
    }

    private AppointmentDto MapToCancelledAppointmentDto(IGrouping<string, CancelledAppointment> group,
        List<User> doctorsUsers, string lang)
    {
        return new AppointmentDto
        {
            AppointmentGroupId = group.Key,
            User = MapToUserDto(group.First().User),
            StartTime = group.Min(a => a.StartTime),
            EndTime = group.Max(a => a.EndTime),
            Doctor = MapToUserDto(doctorsUsers.First(u => u.Id == group.First().DoctorId)),
            Services = MapServicesJson(group.Select(a => a.ServicesJson), lang),
            Status = GetLocalizedText("Anulowana", "Cancelled", lang),
            CancellationReason = group.First().CancellationReason,
            AdditionalInformation = null,
            Notes = null,
        };
    }

    private List<ServiceDto> MapServicesJson(IEnumerable<string?> servicesJsonList, string lang)
    {
        return servicesJsonList
            .Where(json => !string.IsNullOrEmpty(json))
            .SelectMany(json =>
            {
                var services = JArray.Parse(json!);
                return services.Select(s => new ServiceDto
                {
                    Id = s["Id"]!.Value<int>(),
                    LowPrice = s["LowPrice"]?.Value<decimal?>(),
                    HighPrice = s["HighPrice"]?.Value<decimal?>(),
                    MinTime = s["MinTime"]!.Value<int>(),
                    LanguageCode = lang,
                    Name = s["Translations"]!
                               .FirstOrDefault(t => t["LanguageCode"]!.Value<string>() == lang)?["Name"]
                               ?.Value<string>()
                           ?? s["Translations"]!.First()["Name"]!.Value<string>(),
                    Categories = s["Categories"]!
                        .Select(c => lang == "pl"
                            ? c["NamePl"]!.Value<string>()
                            : c["NameEn"]!.Value<string>())
                        .ToList()!
                });
            })
            .ToList();
    }

    private List<AddInformationOutDto> MapAdditionalInformation(IEnumerable<string?> additionalInfoJsonList,
        string lang)
    {
        return additionalInfoJsonList
            .Where(json => !string.IsNullOrEmpty(json))
            .SelectMany(json =>
            {
                var items = JArray.Parse(json!);
                return items.Select(item => new AddInformationOutDto
                {
                    Id = item["Id"]!.Value<int>(),
                    Body = (lang == "pl"
                        ? item["BodyPl"]!.Value<string>()
                        : item["BodyEn"]!.Value<string>())!
                });
            })
            .ToList();
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(
        int doctorId,
        DateTime date,
        string lang,
        bool showCancelled,
        bool showCompleted)
    {
        var startOfWeek = date.Date;
        var endOfWeek = date.AddDays(7).Date;

        var appointments = await GetAppointmentsQuery()
            .Where(a => a.DoctorBlock.DoctorUserId == doctorId &&
                        a.DoctorBlock.TimeBlock.TimeStart.Date >= startOfWeek &&
                        a.DoctorBlock.TimeBlock.TimeStart.Date < endOfWeek)
            .ToListAsync();

        var doctorsUsers = _context.Users.Where(u => u.Id == doctorId).ToList();

        var appointmentsDto = appointments
            .GroupBy(a => a.AppointmentGroupId)
            .Select(group => MapToAppointmentDto(group, lang))
            .ToList();

        var result = appointmentsDto;

        if (showCompleted)
        {
            var completedAppointments = await _context.CompletedAppointments
                .Where(ca => ca.DoctorId == doctorId &&
                             ca.StartTime.Date >= startOfWeek &&
                             ca.StartTime.Date < endOfWeek)
                .Include(completedAppointment => completedAppointment.User)
                .ToListAsync();

            var completedAppointmentsDto = completedAppointments
                .GroupBy(a => a.AppointmentGroupId)
                .Select(group => MapToCompletedAppointmentDto(group!, doctorsUsers, lang))
                .ToList();

            result = result.Concat(completedAppointmentsDto).ToList();
        }

        if (showCancelled)
        {
            var cancelledAppointments = await _context.CancelledAppointments
                .Where(ca => ca.DoctorId == doctorId &&
                             ca.StartTime.Date >= startOfWeek &&
                             ca.StartTime.Date < endOfWeek)
                .Include(cancelledAppointment => cancelledAppointment.User)
                .ToListAsync();

            var cancelledAppointmentsDto = cancelledAppointments
                .GroupBy(a => a.AppointmentGroupId)
                .Select(group => MapToCancelledAppointmentDto(group!, doctorsUsers, lang))
                .ToList();

            result = result.Concat(cancelledAppointmentsDto).ToList();
        }

        return result;
    }

    public async Task BookAppointmentForRegisteredUserAsync(int userId,
        BookAppointmentRequestDto bookAppointmentRequestDto)
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

            var minTime = await _context.Appointments
                .Where(a => a.AppointmentGroupId == addInfoToAppointmentDto.Id)
                .MinAsync(a => a.DoctorBlock.TimeBlock.TimeStart);

            if (minTime > DateTime.Now)
            {
                throw new BusinessException("CANNOT_COMPLETE",
                    "Cannot mark appointment as completed before it starts.");
            }

            List<AdditionalInformation> addInformation;

            if (!addInfoToAppointmentDto.AddInformationIds.Any())
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

            if (updateAppointmentStatusDto.StatusId == (int)AppointmentStatuses.Completed)
            {
                var minTime = await _context.Appointments
                    .Where(a => a.AppointmentGroupId == updateAppointmentStatusDto.AppointmentId)
                    .MinAsync(a => a.DoctorBlock.TimeBlock.TimeStart);

                if (minTime > DateTime.Now)
                {
                    throw new BusinessException("CANNOT_COMPLETE",
                        "Cannot mark appointment as completed before it starts.");
                }
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


    public async Task<ICollection<AppointmentDto>> GetAppointmentsForReceptionistAsync(
        DateTime mondayDate,
        string lang,
        bool showCancelled,
        bool showCompleted
    )
    {
        var startOfWeek = mondayDate.Date;
        var endOfWeek = startOfWeek.AddDays(7).Date;

        var doctorsUsers = _context.Users.Where(u => u.RolesId == 1).ToList();

        var appointments = await GetAppointmentsQuery()
            .Where(a => a.DoctorBlock.TimeBlock.TimeStart.Date >= startOfWeek &&
                        a.DoctorBlock.TimeBlock.TimeStart.Date < endOfWeek)
            .ToListAsync();

        var appointmentsDto = appointments
            .GroupBy(a => a.AppointmentGroupId)
            .Select(group => MapToAppointmentDto(group, lang))
            .ToList();
        var result = appointmentsDto;

        if (showCompleted)
        {
            var completedAppointments = await _context.CompletedAppointments
                .Where(ca => ca.StartTime.Date >= startOfWeek &&
                             ca.StartTime.Date < endOfWeek)
                .Include(completedAppointment => completedAppointment.User)
                .ToListAsync();
            var completedAppointmentsDto = completedAppointments
                .GroupBy(a => a.AppointmentGroupId)
                .Select(group => MapToCompletedAppointmentDto(group!, doctorsUsers, lang))
                .ToList();
            result = result.Concat(completedAppointmentsDto).ToList();
        }

        if (showCancelled)
        {
            var cancelledAppointments = await _context.CancelledAppointments
                .Where(ca => ca.StartTime.Date >= startOfWeek &&
                             ca.StartTime.Date < endOfWeek)
                .Include(cancelledAppointment => cancelledAppointment.User)
                .ToListAsync();
            var cancelledAppointmentsDto = cancelledAppointments
                .GroupBy(a => a.AppointmentGroupId)
                .Select(group => MapToCancelledAppointmentDto(group!, doctorsUsers, lang))
                .ToList();
            result = result.Concat(cancelledAppointmentsDto).ToList();
        }

        return result;
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

    public async Task CancelAppointmentAsync(CancellationDto cancellationDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var appointments = await _context.Appointments
                .Where(a => a.AppointmentGroupId == cancellationDto.AppointmentGuid)
                .Include(a => a.DoctorBlock)
                .ThenInclude(db => db.DoctorUser)
                .Include(a => a.Services).ThenInclude(service => service.ServicesTranslations)
                .Include(appointment => appointment.DoctorBlock)
                .ThenInclude(doctorBlock => doctorBlock.TimeBlock).Include(appointment => appointment.Services)
                .ThenInclude(service => service.ServiceCategories)
                .AsSplitQuery()
                .ToListAsync();

            if (appointments.IsNullOrEmpty())
            {
                throw new BusinessException("NO_APPOINTMENT",
                    "No appointments found for the provided appointment group ID.");
            }

            var cancelledAppointments = appointments.Select(a => new CancelledAppointment
            {
                Id = a.Id,
                UserId = a.UserId,
                DoctorId = a.DoctorBlock.DoctorUser.UserId,
                StartTime = a.DoctorBlock.TimeBlock.TimeStart,
                EndTime = a.DoctorBlock.TimeBlock.TimeEnd,
                AppointmentGroupId = a.AppointmentGroupId,
                AppointmentStatusId = (int)AppointmentStatuses.Cancelled,
                CancellationReason = cancellationDto.Reason,
                ServicesJson = JsonConvert.SerializeObject(a.Services.Select(s => new
                {
                    s.Id,
                    s.LowPrice,
                    s.HighPrice,
                    s.MinTime,
                    Translations = s.ServicesTranslations.Select(t => new
                    {
                        t.LanguageCode,
                        t.Name
                    }),
                    Categories = s.ServiceCategories.Select(c => new
                    {
                        c.Id,
                        c.NamePl,
                        c.NameEn
                    })
                }))
            }).ToList();

            _context.CancelledAppointments.AddRange(cancelledAppointments);

            _context.Appointments.RemoveRange(appointments);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<AppointmentDto> GetCancelledAppointmentByIdAsync(string appointmentGuid)
    {
        var appointment = await _context.CancelledAppointments
            .Where(a => a.AppointmentGroupId == appointmentGuid)
            .Include(a => a.User)
            .Include(a => a.AppointmentStatus)
            .FirstOrDefaultAsync();

        var appointmentDto = new AppointmentDto
        {
            User = new UserDto
            {
                Id = appointment!.User.Id,
                Name = appointment.User.Name,
                Surname = appointment.User.Surname,
                Email = appointment.User.Email,
                PhoneNumber = appointment.User.PhoneNumber
            },
            AppointmentGroupId = appointment.AppointmentGroupId,
            StartTime = appointment.StartTime,
            EndTime = appointment.EndTime,
            Doctor = null,
            Services = null,
            Status = appointment.AppointmentStatus.NameEn,
            AdditionalInformation = null,
            CancellationReason = appointment.CancellationReason,
            Notes = null
        };

        return appointmentDto;
    }

    public async Task CompleteAppointmentAsync(CompletionDto completionDto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var appointments = await _context.Appointments
                .Where(a => a.AppointmentGroupId == completionDto.AppointmentGroupId)
                .Include(a => a.DoctorBlock)
                .ThenInclude(db => db.DoctorUser)
                .Include(a => a.Services).ThenInclude(service => service.ServicesTranslations)
                .Include(appointment => appointment.DoctorBlock)
                .ThenInclude(doctorBlock => doctorBlock.TimeBlock)
                .Include(appointment => appointment.AdditionalInformations).Include(appointment => appointment.Services)
                .ThenInclude(service => service.ServiceCategories)
                .AsSplitQuery()
                .ToListAsync();

            if (!appointments.Any())
            {
                throw new BusinessException("NO_APPOINTMENT",
                    "No appointments found for the provided appointment group ID.");
            }

            if (appointments.Any(a => a.DoctorBlock.TimeBlock.TimeStart > DateTime.Now))
            {
                throw new BusinessException("CANNOT_COMPLETE",
                    "Cannot mark appointment as completed before it starts.");
            }

            var completedAppointments = appointments.Select(a => new CompletedAppointment
            {
                Id = a.Id,
                UserId = a.UserId,
                DoctorId = a.DoctorBlock.DoctorUser.UserId,
                StartTime = a.DoctorBlock.TimeBlock.TimeStart,
                EndTime = a.DoctorBlock.TimeBlock.TimeEnd,
                AppointmentGroupId = a.AppointmentGroupId,
                AppointmentStatusId = (int)AppointmentStatuses.Completed,
                ServicesJson = JsonConvert.SerializeObject(a.Services.Select(s => new
                {
                    s.Id,
                    s.LowPrice,
                    s.HighPrice,
                    s.MinTime,
                    Translations = s.ServicesTranslations.Select(t => new
                    {
                        t.LanguageCode,
                        t.Name
                    }),
                    Categories = s.ServiceCategories.Select(c => new
                    {
                        c.Id,
                        c.NamePl,
                        c.NameEn
                    })
                })),
                AdditionalInformationJson = JsonConvert.SerializeObject(a.AdditionalInformations.Select(ai => new
                {
                    ai.Id,
                    ai.BodyPl,
                    ai.BodyEn
                })),
                Notes = completionDto.Notes
            }).ToList();

            _context.CompletedAppointments.AddRange(completedAppointments);

            _context.Appointments.RemoveRange(appointments);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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
            AdditionalInformation = MapAdditionalInformation(firstBlock.AdditionalInformations, lang),
            Notes = null,
            CancellationReason = null
        };
    }

    private UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    private List<ServiceDto> MapServices(IGrouping<string?, Appointment> group, string lang)
    {
        return group
            .SelectMany(a => a.Services)
            .DistinctBy(s => s.Id)
            .Select(s => new ServiceDto
            {
                Id = s.Id,
                LowPrice = s.LowPrice,
                HighPrice = s.HighPrice,
                MinTime = s.MinTime,
                LanguageCode = lang,
                Name = s.ServicesTranslations
                    .FirstOrDefault(t => t.LanguageCode == lang)
                    ?.Name,
                Categories = s.ServiceCategories
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