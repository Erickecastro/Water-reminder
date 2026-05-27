using Hydra.Core.Models;

namespace Hydra.Core.Interfaces;

public interface IStatsRepository
{
    Task<DailyStatistic?> GetDailyStatisticAsync(int userId, DateTime date, CancellationToken cancellationToken = default);
    Task<IEnumerable<DailyStatistic>> GetRangeAsync(int userId, DateTime start, DateTime end, CancellationToken cancellationToken = default);
}
