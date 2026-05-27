using Hydra.Core.Interfaces;
using Hydra.Core.Models;
using Hydra.Infrastructure.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Networking;
using Microsoft.Maui.Storage;
using System.Net.Http.Json;
using System.Linq;

namespace Hydra.Infrastructure.Sync;

public class SyncService : ISyncService
{
    private readonly IHydrationRepository _hydrationRepo;
    private readonly IUserRepository _userRepo;
    private readonly HttpClient _httpClient;
    private readonly ILogger<SyncService> _logger;
    private bool _initialized = false;

    private const string LastSyncKey = "Hydra_LastSyncUtc";

    public SyncService(IHydrationRepository hydrationRepo, IUserRepository userRepo, IHttpClientFactory httpClientFactory, ILogger<SyncService> logger)
    {
        _hydrationRepo = hydrationRepo;
        _userRepo = userRepo;
        _httpClient = httpClientFactory.CreateClient("HydraApi");
        _logger = logger;
    }

    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_initialized) return Task.CompletedTask;

        Connectivity.Current.ConnectivityChanged += Connectivity_ConnectivityChanged;
        _initialized = true;

        // Fire-and-forget initial sync when online
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            _ = Task.Run(() => SyncNowAsync(cancellationToken));
        }

        return Task.CompletedTask;
    }

    private void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess == NetworkAccess.Internet)
        {
            _ = Task.Run(() => SyncNowAsync());
        }
    }

    public async Task SyncNowAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userRepo.GetFirstUserAsync(cancellationToken);
            if (user == null)
            {
                _logger.LogDebug("No user found locally; skipping sync.");
                return;
            }

            // Push local changes
            var unsynced = (await _hydrationRepo.GetUnsyncedAsync(user.Id, cancellationToken)).ToList();
            if (unsynced.Any())
            {
                _logger.LogInformation("Pushing {Count} hydration entries", unsynced.Count);

                var dtos = unsynced.Select(u => new HydrationEntryDto
                {
                    Id = u.RemoteId,
                    UserId = u.UserId,
                    AmountMl = u.AmountMl,
                    IntakeTime = u.IntakeTime,
                    LastModifiedUtc = u.LastModifiedUtc
                }).ToList();

                var pushed = await PushHydrationAsync(dtos, cancellationToken);

                // Map remote ids back to local
                foreach (var p in pushed)
                {
                    var local = unsynced.FirstOrDefault(x => x.IntakeTime == p.IntakeTime && x.AmountMl == p.AmountMl);
                    if (local != null)
                    {
                        local.RemoteId = p.Id;
                        local.SyncStatus = Hydra.Core.Enums.SyncStatus.Synced;
                        await _hydrationRepo.UpdateAsync(local, cancellationToken);
                    }
                }
            }

            // Pull remote changes since last sync
            var lastSyncUtc = Preferences.Get(LastSyncKey, DateTime.MinValue.ToString("o"));
            DateTime lastSync = DateTime.MinValue;
            if (DateTime.TryParse(lastSyncUtc, out var parsed)) lastSync = parsed;

            _logger.LogInformation("Pulling hydration entries since {Since}", lastSync);
            var pulled = await PullHydrationAsync(lastSync, cancellationToken);

            foreach (var p in pulled)
            {
                // naive mapping: find by remote id
                var existing = (await _hydrationRepo.FindAsync(x => x.RemoteId == p.Id, cancellationToken)).FirstOrDefault();
                if (existing == null)
                {
                    var newLocal = new HydrationEntry
                    {
                        UserId = p.UserId,
                        AmountMl = p.AmountMl,
                        IntakeTime = p.IntakeTime,
                        CreatedAt = DateTime.UtcNow,
                        RemoteId = p.Id,
                        LastModifiedUtc = p.LastModifiedUtc,
                        SyncStatus = Hydra.Core.Enums.SyncStatus.Synced,
                        Source = "remote"
                    };

                    await _hydrationRepo.AddAsync(newLocal, cancellationToken);
                }
                else
                {
                    // conflict resolution: latest LastModifiedUtc wins
                    if (p.LastModifiedUtc > existing.LastModifiedUtc)
                    {
                        existing.AmountMl = p.AmountMl;
                        existing.IntakeTime = p.IntakeTime;
                        existing.LastModifiedUtc = p.LastModifiedUtc;
                        existing.SyncStatus = Hydra.Core.Enums.SyncStatus.Synced;
                        await _hydrationRepo.UpdateAsync(existing, cancellationToken);
                    }
                }
            }

            Preferences.Set(LastSyncKey, DateTime.UtcNow.ToString("o"));
            _logger.LogInformation("Sync completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sync");
        }
    }

    private async Task<IEnumerable<HydrationEntryDto>> PushHydrationAsync(IEnumerable<HydrationEntryDto> dtos, CancellationToken cancellationToken)
    {
        try
        {
            var resp = await _httpClient.PostAsJsonAsync("api/sync/hydration/push", dtos, cancellationToken);
            resp.EnsureSuccessStatusCode();
            var result = await resp.Content.ReadFromJsonAsync<IEnumerable<HydrationEntryDto>>(cancellationToken: cancellationToken);
            return result ?? Array.Empty<HydrationEntryDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to push hydration to server");
            return Array.Empty<HydrationEntryDto>();
        }
    }

    private async Task<IEnumerable<HydrationEntryDto>> PullHydrationAsync(DateTime sinceUtc, CancellationToken cancellationToken)
    {
        try
        {
            var resp = await _httpClient.GetAsync($"api/sync/hydration/pull?sinceUtc={Uri.EscapeDataString(sinceUtc.ToString("o"))}", cancellationToken);
            resp.EnsureSuccessStatusCode();
            var result = await resp.Content.ReadFromJsonAsync<IEnumerable<HydrationEntryDto>>(cancellationToken: cancellationToken);
            return result ?? Array.Empty<HydrationEntryDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to pull hydration from server");
            return Array.Empty<HydrationEntryDto>();
        }
    }
}
