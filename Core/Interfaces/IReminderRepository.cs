using Hydra.Core.Models;

namespace Hydra.Core.Interfaces;

public interface IReminderRepository : IRepository<Reminder>
{
    Task<IEnumerable<Reminder>> GetEnabledForUserAsync(int userId, CancellationToken cancellationToken = default);
}
