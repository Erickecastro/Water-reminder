using Hydra.Core.Interfaces;
using Hydra.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Data.Repositories;

public class HydrationRepository : RepositoryBase<HydrationEntry>, IHydrationRepository
{
    private readonly DbContext _context;

    public HydrationRepository(HydraDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HydrationEntry>> GetForDateAsync(int userId, DateTime date, CancellationToken cancellationToken = default)
    {
        var start = date.Date;
        var end = start.AddDays(1);
        return await _context.Set<HydrationEntry>()
            .Where(h => h.UserId == userId && h.IntakeTime >= start && h.IntakeTime < end)
            .OrderByDescending(h => h.IntakeTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<HydrationEntry>> GetUnsyncedAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<HydrationEntry>()
            .Where(h => h.UserId == userId && h.SyncStatus != Hydra.Core.Enums.SyncStatus.Synced)
            .OrderBy(h => h.LastModifiedUtc)
            .ToListAsync(cancellationToken);
    }
}
