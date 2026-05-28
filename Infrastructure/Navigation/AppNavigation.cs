using Hydra.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Water_reminder;

namespace Hydra.Infrastructure.Navigation;

public class AppNavigation
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserSessionService _sessionService;

    public AppNavigation(IServiceProvider serviceProvider, IUserSessionService sessionService)
    {
        _serviceProvider = serviceProvider;
        _sessionService = sessionService;
    }

    public Window CreateRootWindow()
    {
        var loginPage = CreateLoginPage();
        return new Window(loginPage);
    }

    public async Task NavigateToLoginAsync()
    {
        await _sessionService.SignOutAsync();

        var window = Application.Current?.Windows.FirstOrDefault();
        if (window is null)
        {
            return;
        }

        window.Page = CreateLoginPage();
    }

    private LoginPage CreateLoginPage()
    {
        var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
        loginPage.SignedIn -= OnSignedIn;
        loginPage.SignedIn += OnSignedIn;
        return loginPage;
    }

    private void OnSignedIn(object? sender, EventArgs e)
    {
        if (sender is LoginPage loginPage)
        {
            loginPage.SignedIn -= OnSignedIn;
        }

        var window = Application.Current?.Windows.FirstOrDefault();
        if (window is null)
        {
            return;
        }

        window.Page = _serviceProvider.GetRequiredService<AppShell>();
    }
}
