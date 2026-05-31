using Hydra.Presentation.ViewModels;
using Hydra.Infrastructure.Navigation;

namespace Water_reminder;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;
    private readonly AppNavigation _navigation;

    public SettingsPage(SettingsViewModel viewModel, AppNavigation navigation)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _navigation = navigation;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }

    private async void OnActionPressed(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
        {
            await element.ScaleTo(0.96, 90, Easing.CubicOut);
        }
    }

    private async void OnActionReleased(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
        {
            await element.ScaleTo(1, 120, Easing.CubicIn);
        }
    }

    private void OnNavigateRequested(object? sender, string tab)
    {
        if (tab == "Home")
        {
            _navigation.NavigateToMain();
        }
        else if (tab == "History")
        {
            _navigation.NavigateToHistory();
        }
    }
}
