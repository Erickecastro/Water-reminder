namespace Hydra.Core.Interfaces;

public interface ISyncService
{
    /// <summary>
    /// Initialize sync service, subscribe to connectivity changes and schedule background sync.
    /// </summary>
    Task InitializeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Trigger a manual sync now.
    /// </summary>
    Task SyncNowAsync(CancellationToken cancellationToken = default);
}
