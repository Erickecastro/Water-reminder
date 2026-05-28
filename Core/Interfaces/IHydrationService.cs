using Hydra.Core.Models;

namespace Hydra.Core.Interfaces;

public interface IHydrationService
{
    Task AddIntakeAsync(HydrationEntry entry, CancellationToken cancellationToken = default);
    Task<IEnumerable<HydrationEntry>> GetTodayEntriesAsync(int userId, CancellationToken cancellationToken = default);
    Task<int> GetTodayTotalAsync(int userId, CancellationToken cancellationToken = default);
    Task ClearTodayAsync(int userId, CancellationToken cancellationToken = default);
}
