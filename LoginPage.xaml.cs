using Hydra.Presentation.ViewModels;

namespace Water_reminder;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;
    private bool _animated;

    public event EventHandler? SignedIn;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _viewModel.SignedIn += (_, _) => SignedIn?.Invoke(this, EventArgs.Empty);
        BindingContext = _viewModel;

        LoginButton.Pressed += OnInteractivePressed;
        LoginButton.Released += OnInteractiveReleased;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.ClearForm();

        if (_animated)
        {
            return;
        }

        TopHero.Opacity = 0;
        TopHero.TranslationY = -20;
        LoginCard.Opacity = 0;
        LoginCard.TranslationY = 24;

        await Task.WhenAll(
            TopHero.FadeTo(1, 300, Easing.CubicOut),
            TopHero.TranslateTo(0, 0, 300, Easing.CubicOut),
            LoginCard.FadeTo(1, 380, Easing.CubicOut),
            LoginCard.TranslateTo(0, 0, 380, Easing.CubicOut));

        _animated = true;
    }

    private async void OnInteractivePressed(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
        {
            await element.ScaleTo(0.97, 90, Easing.CubicOut);
        }
    }

    private async void OnInteractiveReleased(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
        {
            await element.ScaleTo(1, 120, Easing.CubicIn);
        }
    }
}
