using Hydra.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Water_reminder;

namespace Hydra.Infrastructure.Navigation;

public class AppNavigation
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserSessionService _sessionService;
    private MainContainerPage? _mainContainerPage;
    private bool _isTransitioning;

    public AppNavigation(IServiceProvider serviceProvider, IUserSessionService sessionService)
    {
        _serviceProvider = serviceProvider;
        _sessionService = sessionService;
    }

    public Window CreateRootWindow()
    {
        return new Window(_sessionService.IsSignedIn ? GetMainPage() : CreateWelcomePage());
    }

    public async Task NavigateToLoginAsync()
    {
        await _sessionService.SignOutAsync();
        await SetRootPageAsync(CreateLoginPage());
    }

    private LoginPage CreateLoginPage()
    {
        var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
        loginPage.SignedIn -= OnSignedIn;
        loginPage.SignedIn += OnSignedIn;
        return loginPage;
    }

    private WelcomePage CreateWelcomePage()
    {
        var welcomePage = _serviceProvider.GetRequiredService<WelcomePage>();
        welcomePage.ContinueRequested -= OnWelcomeContinueRequested;
        welcomePage.ContinueRequested += OnWelcomeContinueRequested;
        return welcomePage;
    }

    private async void OnWelcomeContinueRequested(object? sender, EventArgs e)
    {
        if (sender is WelcomePage welcomePage)
        {
            welcomePage.ContinueRequested -= OnWelcomeContinueRequested;
        }

        await SetRootPageAsync(CreateLoginPage());
    }

    private async void OnSignedIn(object? sender, EventArgs e)
    {
        if (sender is LoginPage loginPage)
        {
            loginPage.SignedIn -= OnSignedIn;
        }

        await SetRootPageAsync(GetMainPage());
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

    private MainContainerPage GetMainPage()
    {
        return _mainContainerPage ??=
            _serviceProvider.GetRequiredService<MainContainerPage>();
    }

    private async Task SetRootPageAsync(Page page)
    {
        var window = Application.Current?.Windows.FirstOrDefault();
        if (window is null || ReferenceEquals(window.Page, page) || _isTransitioning)
        {
            return;
        }

        _isTransitioning = true;

        var outgoing = GetTransitionTarget(window.Page);
        var incoming = GetTransitionTarget(page);

        if (incoming is not null)
        {
            incoming.Opacity = 0;
        }

        if (outgoing is not null)
        {
            await outgoing.FadeTo(0, 95, Easing.SinInOut);
        }

        window.Page = page;

        if (incoming is not null)
        {
            await incoming.FadeTo(1, 125, Easing.SinInOut);
        }

        if (outgoing is not null)
        {
            outgoing.Opacity = 1;
        }

        _isTransitioning = false;
    }

    private static VisualElement? GetTransitionTarget(Page? page)
    {
        if (page is not ContentPage contentPage)
        {
            return null;
        }

        if (contentPage.Content is Grid grid && grid.Children.Count > 1)
        {
            return grid.Children[^1] as VisualElement;
        }

        return contentPage.Content;
    }
}
