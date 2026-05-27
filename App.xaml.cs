using Microsoft.Extensions.DependencyInjection;

namespace Water_reminder;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		// Start background services like sync
		var services = Application.Current?.Handler?.MauiContext?.Services;
		var sync = services?.GetService<Hydra.Core.Interfaces.ISyncService>();
		_ = sync?.InitializeAsync();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}