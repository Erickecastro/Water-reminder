using Hydra.Core.Interfaces;
using Hydra.Infrastructure.Navigation;

namespace Water_reminder;

public partial class App : Application
{
	private readonly IServiceProvider _serviceProvider;

	public App(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var sessionService = _serviceProvider.GetRequiredService<IUserSessionService>();
		sessionService.InitializeAsync().GetAwaiter().GetResult();

		return _serviceProvider.GetRequiredService<AppNavigation>().CreateRootWindow();
	}
}