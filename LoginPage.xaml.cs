using Hydra.Presentation.ViewModels;

namespace Water_reminder;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public event EventHandler? SignedIn;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;

        _viewModel.SignedIn += (_, _) => SignedIn?.Invoke(this, EventArgs.Empty);

        LoginButton.Pressed += OnInteractivePressed;
        LoginButton.Released += OnInteractiveReleased;

        NameEntry.Focused += (_, _) => AnimateLineAsync(NameLine, true);
        NameEntry.Unfocused += (_, _) => AnimateLineAsync(NameLine, false);
        EmailEntry.Focused += (_, _) => AnimateLineAsync(EmailLine, true);
        EmailEntry.Unfocused += (_, _) => AnimateLineAsync(EmailLine, false);
        PasswordEntry.Focused += (_, _) => AnimateLineAsync(PasswordLine, true);
        PasswordEntry.Unfocused += (_, _) => AnimateLineAsync(PasswordLine, false);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.ClearForm();
    }

    private async void OnInteractivePressed(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
            await element.ScaleTo(0.97, 90, Easing.CubicOut);
    }

    private async void OnInteractiveReleased(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
            await element.ScaleTo(1, 120, Easing.CubicIn);
    }

    private static async void AnimateLineAsync(BoxView line, bool focused)
    {
        line.Color = Color.FromArgb(focused ? "#8FB8FF" : "#35436B");
        await line.ScaleTo(focused ? 1.01 : 1, 120, Easing.CubicOut);
    }
}
