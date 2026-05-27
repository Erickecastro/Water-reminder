using Hydra.Core.Models;

namespace Hydra.Core.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetFirstUserAsync(CancellationToken cancellationToken = default);
}
