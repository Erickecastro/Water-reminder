using Hydra.Infrastructure.Api;

namespace Hydra.Infrastructure.Api;

public interface IRemoteApi
{
    /// <summary>
    /// Push local hydration entries to the server. Server returns assigned remote IDs.
    /// </summary>
    Task<IEnumerable<HydrationEntryDto>> PushHydrationAsync(IEnumerable<HydrationEntryDto> entries, CancellationToken cancellationToken = default);

    /// <summary>
    /// Pull hydration entries modified since the given UTC timestamp.
    /// </summary>
    Task<IEnumerable<HydrationEntryDto>> PullHydrationAsync(DateTime sinceUtc, CancellationToken cancellationToken = default);
}
