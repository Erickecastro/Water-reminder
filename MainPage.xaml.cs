using Hydra.Presentation.ViewModels;

namespace Water_reminder;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    private bool _animated;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _vm = viewModel;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadDataAsync();

        if (_animated)
        {
            return;
        }

        HeaderContainer.Opacity = 0;
        HeaderContainer.TranslationY = -14;
        ContentContainer.Opacity = 0;
        ContentContainer.TranslationY = 18;
        PrimaryActionButton.Scale = 0.94;

        await Task.WhenAll(
            HeaderContainer.FadeTo(1, 280, Easing.CubicOut),
            HeaderContainer.TranslateTo(0, 0, 280, Easing.CubicOut),
            ContentContainer.FadeTo(1, 360, Easing.CubicOut),
            ContentContainer.TranslateTo(0, 0, 360, Easing.CubicOut),
            PrimaryActionButton.ScaleTo(1, 260, Easing.SpringOut));

        _animated = true;
    }
}

