using Hydra.Core.Models;

namespace Hydra.Core.Interfaces;

public interface IHydrationRepository : IRepository<HydrationEntry>
{
    Task<IEnumerable<HydrationEntry>> GetForDateAsync(int userId, DateTime date, CancellationToken cancellationToken = default);
    Task<IEnumerable<HydrationEntry>> GetUnsyncedAsync(int userId, CancellationToken cancellationToken = default);
    Task ClearForDateAsync(int userId, DateTime date, CancellationToken cancellationToken = default);
}
