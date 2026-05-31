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
        UserAppTheme = AppTheme.Dark;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var sessionService = _serviceProvider.GetRequiredService<IUserSessionService>();
        try
        {
            sessionService.InitializeAsync().GetAwaiter().GetResult();
        }
        catch
        {
            // Keep startup resilient; the login screen can recover with an empty session.
        }

        return _serviceProvider.GetRequiredService<AppNavigation>().CreateRootWindow();
    }
}
