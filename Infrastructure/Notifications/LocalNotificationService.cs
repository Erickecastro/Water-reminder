namespace Hydra.Infrastructure.Notifications;

public class LocalNotificationService : Hydra.Core.Interfaces.INotificationService
{
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task ScheduleReminderAsync(int id, DateTime when, string title, string message)
    {
        return Task.CompletedTask;
    }

    public Task CancelReminderAsync(int id)
    {
        return Task.CompletedTask;
    }

    public Task SendImmediateAsync(string title, string message)
    {
        return Task.CompletedTask;
    }
}
