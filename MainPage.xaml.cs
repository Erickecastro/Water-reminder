using Hydra.Presentation.ViewModels;
using System.ComponentModel;

namespace Water_reminder;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    private bool _animated;
    private double _lastProgress;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _vm = viewModel;
        BindingContext = _vm;
        _vm.PropertyChanged += OnViewModelPropertyChanged;
        ProgressTrack.SizeChanged += async (_, _) => await AnimateProgressFill(_vm.ProgressPercent, 1);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadDataAsync();
        await AnimateProgressFill(_vm.ProgressPercent, 380);

        if (_animated)
        {
            return;
        }

        HeaderContainer.Opacity = 0;
        HeaderContainer.TranslationY = -14;
        ContentContainer.Opacity = 0;
        ContentContainer.TranslationY = 18;

        await Task.WhenAll(
            HeaderContainer.FadeTo(1, 280, Easing.CubicOut),
            HeaderContainer.TranslateTo(0, 0, 280, Easing.CubicOut),
            ContentContainer.FadeTo(1, 360, Easing.CubicOut),
            ContentContainer.TranslateTo(0, 0, 360, Easing.CubicOut));

        _animated = true;
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

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.ProgressPercent))
        {
            await AnimateProgressFill(_vm.ProgressPercent, 450);
        }
    }

    private async Task AnimateProgressFill(double progress, uint duration)
    {
        if (ProgressTrack.Width <= 0)
        {
            return;
        }

        var clamped = Math.Clamp(progress, 0, 1);
        var target = ProgressTrack.Width * clamped;
        ProgressFill.AnchorX = 0;
        ProgressFill.WidthRequest = ProgressTrack.Width * _lastProgress;
        await ProgressFill.LayoutTo(new Rect(0, 0, target, ProgressTrack.Height), duration, Easing.CubicInOut);
        _lastProgress = clamped;
    }
}

