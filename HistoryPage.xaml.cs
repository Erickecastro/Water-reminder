using Hydra.Presentation.ViewModels;
using Hydra.Infrastructure.Navigation;

namespace Water_reminder;

public partial class HistoryPage : ContentPage
{
    private readonly HistoryViewModel _viewModel;
    private readonly AppNavigation _navigation;

    public HistoryPage(HistoryViewModel viewModel, AppNavigation navigation)
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

    private async void OnClearHistoryClicked(object? sender, EventArgs e)
    {
        if (_viewModel.Entries.Count == 0)
        {
            return;
        }

        await Task.WhenAll(
            HistoryList.FadeTo(0, 180, Easing.CubicIn),
            HistoryList.TranslateTo(0, 12, 180, Easing.CubicIn));

        await _viewModel.ClearTodayAsync();

        HistoryList.TranslationY = -8;
        await Task.WhenAll(
            HistoryList.FadeTo(1, 220, Easing.CubicOut),
            HistoryList.TranslateTo(0, 0, 220, Easing.CubicOut));
    }

    private void OnNavigateRequested(object? sender, string tab)
    {
        if (tab == "Home")
        {
            _navigation.NavigateToMain();
        }
        else if (tab == "Settings")
        {
            _navigation.NavigateToSettings();
        }
    }
}
