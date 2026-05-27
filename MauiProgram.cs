using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Hydra.Data.Database;
using Hydra.Data.Repositories;
using Hydra.Core.Interfaces;
using Hydra.Core.Services;
using Hydra.Infrastructure.Notifications;

namespace Water_reminder;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Configure DI and services
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "hydra.db");
		builder.Services.AddDbContext<HydraDbContext>(options =>
		{
			options.UseSqlite($"Data Source={dbPath}");
		});

		// Repositories
		builder.Services.AddScoped<IUserRepository, UserRepository>();
		builder.Services.AddScoped<IHydrationRepository, HydrationRepository>();

		// Services
		builder.Services.AddScoped<IHydrationService, HydrationService>();
		builder.Services.AddSingleton<INotificationService, LocalNotificationService>();

		// ViewModels and other app services can be registered here
		builder.Services.AddTransient<Hydra.Presentation.ViewModels.MainViewModel>();
		// HTTP client to backend API
		builder.Services.AddHttpClient("HydraApi", client =>
		{
			client.BaseAddress = new Uri(builder.Configuration["Backend:BaseUrl"] ?? "https://api.yourhydra.app/");
		});

		// Sync service
		builder.Services.AddSingleton<Hydra.Core.Interfaces.ISyncService, Hydra.Infrastructure.Sync.SyncService>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
