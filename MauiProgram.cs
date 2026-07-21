using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Hydra.Data.Database;
using Hydra.Data.Repositories;
using Hydra.Core.Interfaces;
using Hydra.Core.Services;
using Hydra.Infrastructure.Notifications;
using Hydra.Infrastructure.Navigation;
using Hydra.Infrastructure.Session;
using Plugin.LocalNotification;
using Water_reminder.Controls;
using Water_reminder.Views;

#if ANDROID || IOS || MACCATALYST
using Microsoft.Maui.Handlers;
#endif

#if IOS || MACCATALYST
using UIKit;
#endif

namespace Water_reminder;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if ANDROID || IOS || MACCATALYST
        EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is not BorderlessEntry)
            {
                return;
            }

#if ANDROID
            handler.PlatformView.Background = null;
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            handler.PlatformView.BackgroundTintList =
                Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif

#if IOS || MACCATALYST
            handler.PlatformView.BorderStyle = UITextBorderStyle.None;
#endif
        });
#endif

        builder.Services.AddDbContext<HydraDbContext>(options =>
        {
            try
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "hydra.db");
                options.UseSqlite($"Data Source={dbPath}");
            }
            catch
            {
                options.UseSqlite("Data Source=hydra.db");
            }
        });

        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IHydrationRepository, HydrationRepository>();
        builder.Services.AddTransient<IHydrationService, HydrationService>();
        builder.Services.AddTransient<Hydra.Core.Interfaces.INotificationService, LocalNotificationService>();
        builder.Services.AddTransient<Hydra.Core.Interfaces.ISyncService, Hydra.Infrastructure.Sync.SyncService>();

        builder.Services.AddSingleton<MainContainerPage>();

        builder.Services.AddSingleton<IUserSessionService, UserSessionService>();
        builder.Services.AddSingleton<AppNavigation>();

        builder.Services.AddSingleton<AppShell>();

        builder.Services.AddTransient<WelcomePage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<HistoryPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddSingleton<HomeView>();
        builder.Services.AddSingleton<HistoryView>();
        builder.Services.AddSingleton<SettingsView>();

        builder.Services.AddTransient<Hydra.Presentation.ViewModels.LoginViewModel>();
        builder.Services.AddTransient<Hydra.Presentation.ViewModels.MainViewModel>();
        builder.Services.AddTransient<Hydra.Presentation.ViewModels.HistoryViewModel>();
        builder.Services.AddTransient<Hydra.Presentation.ViewModels.SettingsViewModel>();

        builder.Services.AddHttpClient("HydraApi", client =>
        {
            client.BaseAddress = new Uri("https://api.yourhydra.app/");
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<HydraDbContext>();
            db.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            app.Services.GetService<ILoggerFactory>()
                ?.CreateLogger(nameof(MauiProgram))
                .LogError(ex, "Failed to initialize the local database.");
        }
        return app;
    }
}
