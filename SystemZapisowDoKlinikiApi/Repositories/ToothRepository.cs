using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class ToothRepository : IToothRepository
{
    private readonly ClinicDbContext _context;

    public ToothRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<ToothModelOutDTO> GetToothModelAsync(ToothModelRequest request)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId);
        if (!userExists)
            throw new ArgumentException("User not found", nameof(request.UserId));

        var teethNames = await _context.ToothNames
            .ToDictionaryAsync(
                tn => tn.ToothNumber,
                tn => request.Language == "pl" ? tn.NamePl : tn.NameEn
            );

        var teeth = await _context.Teeth
            .Include(t => t.ToothStatus)
            .ThenInclude(s => s.ToothStatusTranslations)
            .Include(t => t.ToothStatus)
            .ThenInclude(s => s.Category)
            .ThenInclude(c => c.Translations)
            .Where(t => t.UserId == request.UserId)
            .Select(t => new ToothOutDTO
            {
                ToothNumber = t.ToothNumber,
                ToothName = teethNames.ContainsKey(t.ToothNumber)
                    ? teethNames[t.ToothNumber]
                    : "Unknown",
                Status = new ToothStatusOutDto
                {
                    StatusId = t.ToothStatus.Id,
                    StatusName = t.ToothStatus.ToothStatusTranslations
                        .Where(tr => tr.LanguageCode == request.Language)
                        .Select(tr => tr.Name)
                        .FirstOrDefault() ?? "Brak tłumaczenia",
                    CategoryId = t.ToothStatus.CategoryId,
                    CategoryName = t.ToothStatus.Category.Translations
                        .Where(ct => ct.LanguageCode == request.Language)
                        .Select(ct => ct.Name)
                        .FirstOrDefault() ?? "Brak tłumaczenia"
                }
            })
            .OrderBy(t => t.ToothNumber)
            .ToListAsync();

        var result = new ToothModelOutDTO
        {
            Teeth = teeth
        };

        return result;
    }

    public async Task UpdateToothModelAsync(ToothModelInDTO request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var tooth in request.Teeth)
            {
                var toothEntity = await _context.Teeth
                    .FirstOrDefaultAsync(t => t.UserId == request.UserId && t.ToothNumber == tooth.ToothNumber);

                if (toothEntity == null)
                {
                    throw new ArgumentException(
                        $"Tooth number {tooth.ToothNumber} for user {request.UserId} not found.");
                }

                toothEntity.ToothStatusId = tooth.StatusId;
                _context.Teeth.Update(toothEntity);
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

    public async Task<ToothStatusesDto> GetToothStatusesAsync(string language)
    {
        var statuses = await _context.ToothStatuses
            .Include(ts => ts.ToothStatusTranslations)
            .Include(ts => ts.Category)
            .ThenInclude(c => c.Translations)
            .Select(ts =>
                new ToothStatusOutDto()
                {
                    StatusId = ts.Id,
                    StatusName = ts.ToothStatusTranslations
                        .FirstOrDefault(t => t.LanguageCode == language)!.Name,
                    CategoryId = ts.CategoryId,
                    CategoryName = ts.Category.Translations
                        .FirstOrDefault(ct => ct.LanguageCode == language)!.Name
                }
            )
            .ToListAsync();
        var statusesDto = new ToothStatusesDto
        {
            StatusesByCategories = statuses
                .GroupBy(s => s.CategoryName)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                )
        };
        return statusesDto;
    }
}