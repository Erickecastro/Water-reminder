using Hydra.Core.Interfaces;
using Hydra.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Data.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly DbContext _context;

    public UserRepository(HydraDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetFirstUserAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<User>().OrderBy(u => u.Id).FirstOrDefaultAsync(cancellationToken);
    }
}
