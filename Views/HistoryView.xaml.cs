using Hydra.Presentation.ViewModels;

namespace Water_reminder.Views;

public partial class HistoryView : ContentView
{
    private readonly HistoryViewModel _viewModel;

    private bool _loaded;


    public HistoryView(HistoryViewModel viewModel)
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
}