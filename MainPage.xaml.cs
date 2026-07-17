using Hydra.Presentation.ViewModels;
using Hydra.Infrastructure.Navigation;

namespace Water_reminder;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    private readonly AppNavigation _navigation;

    private bool _loaded;

    public MainPage(MainViewModel viewModel, AppNavigation navigation)
    {
        InitializeComponent();
        _vm = viewModel;
        _navigation = navigation;
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_loaded)
            return;

        _loaded = true;

        Dispatcher.Dispatch(async () =>
        {
            await _vm.LoadDataAsync();
        });
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