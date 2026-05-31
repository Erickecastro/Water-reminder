namespace Water_reminder;

public partial class WelcomePage : ContentPage
{
    public event EventHandler? ContinueRequested;

    public WelcomePage()
    {
        InitializeComponent();
        ContinueButton.Pressed += OnButtonPressed;
        ContinueButton.Released += OnButtonReleased;
        ExitButton.Pressed += OnButtonPressed;
        ExitButton.Released += OnButtonReleased;
    }

    private void OnContinueClicked(object? sender, EventArgs e)
    {
        ContinueRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnExitClicked(object? sender, EventArgs e)
    {
        Application.Current?.Quit();
    }

    private async void OnButtonPressed(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
        {
            await element.ScaleTo(0.94, 90, Easing.CubicOut);
        }
    }

    private async void OnButtonReleased(object? sender, EventArgs e)
    {
        if (sender is VisualElement element)
        {
            await element.ScaleTo(1, 130, Easing.CubicIn);
        }
    }
}
