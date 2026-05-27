namespace Hydra.Core.Interfaces;

public interface INotificationService
{
    Task InitializeAsync();
    Task ScheduleReminderAsync(int id, DateTime when, string title, string message);
    Task CancelReminderAsync(int id);
    Task SendImmediateAsync(string title, string message);
}
