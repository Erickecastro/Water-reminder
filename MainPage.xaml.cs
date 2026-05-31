using Hydra.Presentation.ViewModels;
using Hydra.Infrastructure.Navigation;
using System.ComponentModel;

namespace Water_reminder;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    private readonly AppNavigation _navigation;

    public MainPage(MainViewModel viewModel, AppNavigation navigation)
    {
        InitializeComponent();
        _vm = viewModel;
        _navigation = navigation;
        BindingContext = _vm;
        _vm.PropertyChanged += OnViewModelPropertyChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadDataAsync();
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

    private async void OnHydrationButtonClicked(object? sender, EventArgs e)
    {
        await CupMascot.CelebrateAsync();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.ProgressPercent))
        {
            ProgressCup.Progress = _vm.ProgressPercent;
        }
    }

    private void OnNavigateRequested(object? sender, string tab)
    {
        if (tab == "History")
        {
            _navigation.NavigateToHistory();
        }
        else if (tab == "Settings")
        {
            _navigation.NavigateToSettings();
        }
    }
}

