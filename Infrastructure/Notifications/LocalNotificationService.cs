using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification.AndroidOption;

namespace Hydra.Infrastructure.Notifications;

public class LocalNotificationService : Hydra.Core.Interfaces.INotificationService
{
    public async Task InitializeAsync()
    {
        // Initialization for Android/iOS if needed
        await Task.CompletedTask;
    }

    public async Task ScheduleReminderAsync(int id, DateTime when, string title, string message)
    {
        var notification = new NotificationRequest
        {
            NotificationId = id,
            Title = title,
            Description = message,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = when,
                NotifyRepeatInterval = null
            }
        };

        NotificationCenter.Current.Show(notification);
        await Task.CompletedTask;
    }

    public async Task CancelReminderAsync(int id)
    {
        NotificationCenter.Current.Cancel(id);
        await Task.CompletedTask;
    }

    public async Task SendImmediateAsync(string title, string message)
    {
        var notification = new NotificationRequest
        {
            NotificationId = new Random().Next(1000, 9999),
            Title = title,
            Description = message
        };
        NotificationCenter.Current.Show(notification);
        await Task.CompletedTask;
    }
}
