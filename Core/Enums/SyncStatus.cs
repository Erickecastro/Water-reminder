namespace Hydra.Core.Enums;

/// <summary>
/// Synchronization status for entities
/// </summary>
public enum SyncStatus
{
    /// <summary>New or updated locally and not yet pushed</summary>
    Pending = 0,
    
    /// <summary>Successfully synchronized with server</summary>
    Synced = 1,
    
    /// <summary>Marked for deletion locally and not yet removed on server</summary>
    Deleted = 2,
    
    /// <summary>Conflict detected and needs resolution</summary>
    Conflict = 3
}
