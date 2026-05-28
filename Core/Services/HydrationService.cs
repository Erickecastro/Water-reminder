using Hydra.Core.Interfaces;
using Hydra.Core.Models;

namespace Hydra.Core.Services;

public class HydrationService : IHydrationService
{
    private readonly IHydrationRepository _hydrationRepository;
    private readonly IUserRepository _userRepository;

    public HydrationService(IHydrationRepository hydrationRepository, IUserRepository userRepository)
    {
        _hydrationRepository = hydrationRepository;
        _userRepository = userRepository;
    }

    public async Task AddIntakeAsync(HydrationEntry entry, CancellationToken cancellationToken = default)
    {
        entry.CreatedAt = DateTime.UtcNow;
        entry.LastModifiedUtc = DateTime.UtcNow;
        entry.SyncStatus = Hydra.Core.Enums.SyncStatus.Pending;
        await _hydrationRepository.AddAsync(entry, cancellationToken);
        // TODO: update stats, xp, streaks, triggers, sync
    }

    public async Task<IEnumerable<HydrationEntry>> GetTodayEntriesAsync(int userId, CancellationToken cancellationToken = default)
    {
        var date = DateTime.UtcNow.Date;
        return await _hydrationRepository.GetForDateAsync(userId, date, cancellationToken);
    }

    public async Task<int> GetTodayTotalAsync(int userId, CancellationToken cancellationToken = default)
    {
        var entries = await GetTodayEntriesAsync(userId, cancellationToken);
        return entries.Sum(e => e.AmountMl);
    }

    public Task ClearTodayAsync(int userId, CancellationToken cancellationToken = default)
    {
        return _hydrationRepository.ClearForDateAsync(userId, DateTime.UtcNow.Date, cancellationToken);
    }
}
