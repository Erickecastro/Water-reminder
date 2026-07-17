using Hydra.Presentation.ViewModels;

namespace Water_reminder.Views;

public partial class SettingsView : ContentView
{
    private readonly SettingsViewModel _viewModel;

    private bool _loaded;


    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        BindingContext = _viewModel;
    }


    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent is not null && !_loaded)
        {
            _loaded = true;

            Dispatcher.Dispatch(async () =>
            {
                await _viewModel.LoadAsync();
            });
        }
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
}