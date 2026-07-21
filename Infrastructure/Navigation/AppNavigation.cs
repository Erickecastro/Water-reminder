using Hydra.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Water_reminder;

namespace Hydra.Infrastructure.Navigation;

public class AppNavigation
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserSessionService _sessionService;

    private MainContainerPage? _mainContainerPage;

    public AppNavigation(
        IServiceProvider serviceProvider,
        IUserSessionService sessionService)
    {
        _serviceProvider = serviceProvider;
        _sessionService = sessionService;
    }

    public Window CreateRootWindow()
    {
        return new Window(_sessionService.IsSignedIn
            ? GetMainPage()
            : CreateWelcomePage())
        {
            Title = "Hidraté"
        };
    }

    public async Task NavigateToLoginAsync()
    {
        await _sessionService.SignOutAsync();
        await SetRootPageAsync(CreateLoginPage());
    }

    public void NavigateToMain()
    {
        GetMainPage().ShowHome();
    }

    public void NavigateToHistory()
    {
        GetMainPage().ShowHistory();
    }

    public void NavigateToSettings()
    {
        GetMainPage().ShowSettings();
    }

    private LoginPage CreateLoginPage()
    {
        var page = _serviceProvider.GetRequiredService<LoginPage>();

        page.SignedIn -= OnSignedIn;
        page.SignedIn += OnSignedIn;

        return page;
    }

    private WelcomePage CreateWelcomePage()
    {
        var page = _serviceProvider.GetRequiredService<WelcomePage>();

        page.ContinueRequested -= OnWelcomeContinueRequested;
        page.ContinueRequested += OnWelcomeContinueRequested;

        return page;
    }

    private MainContainerPage GetMainPage()
    {
        return _mainContainerPage ??=
            _serviceProvider.GetRequiredService<MainContainerPage>();
    }

    private async void OnWelcomeContinueRequested(object? sender, EventArgs e)
    {
        if (sender is WelcomePage page)
        {
            page.ContinueRequested -= OnWelcomeContinueRequested;
        }

        await SetRootPageAsync(CreateLoginPage());
    }

    private async void OnSignedIn(object? sender, EventArgs e)
    {
        if (sender is LoginPage page)
        {
            page.SignedIn -= OnSignedIn;
        }

        await SetRootPageAsync(GetMainPage());
    }

    private Task SetRootPageAsync(Page page)
    {
        var window = Application.Current?.Windows.FirstOrDefault();

        if (window is null)
            return Task.CompletedTask;

        if (ReferenceEquals(window.Page, page))
            return Task.CompletedTask;

        window.Page = page;

        return Task.CompletedTask;
    }
}