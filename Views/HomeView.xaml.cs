using Hydra.Presentation.ViewModels;

namespace Water_reminder.Views;

public partial class HomeView : ContentView
{
    private readonly MainViewModel _vm;

    private bool _loaded;


    public HomeView(MainViewModel viewModel)
    {
        InitializeComponent();

        _vm = viewModel;

        BindingContext = _vm;
    }


    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent is not null && !_loaded)
        {
            _loaded = true;

            Dispatcher.Dispatch(async () =>
            {
                await _vm.LoadDataAsync();
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