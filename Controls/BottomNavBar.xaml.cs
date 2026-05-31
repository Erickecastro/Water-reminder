namespace Water_reminder.Controls;

public partial class BottomNavBar : ContentView
{
    public static readonly BindableProperty ActiveTabProperty = BindableProperty.Create(
        nameof(ActiveTab),
        typeof(string),
        typeof(BottomNavBar),
        "Home",
        propertyChanged: static (bindable, _, _) => ((BottomNavBar)bindable).UpdateVisualState());

    public string ActiveTab
    {
        get => (string)GetValue(ActiveTabProperty);
        set => SetValue(ActiveTabProperty, value);
    }

    public event EventHandler<string>? NavigateRequested;

    public BottomNavBar()
    {
        InitializeComponent();
        Loaded += (_, _) => UpdateVisualState();
    }

    private async void OnHomeTapped(object? sender, TappedEventArgs e)
    {
        await SelectAsync(HomeItem, "Home");
    }

    private async void OnHistoryTapped(object? sender, TappedEventArgs e)
    {
        await SelectAsync(HistoryItem, "History");
    }

    private async void OnSettingsTapped(object? sender, TappedEventArgs e)
    {
        await SelectAsync(SettingsItem, "Settings");
    }

    private async Task SelectAsync(VisualElement element, string tab)
    {
        if (ActiveTab != tab)
        {
            NavigateRequested?.Invoke(this, tab);
        }

        await Task.WhenAll(
            element.ScaleTo(0.94, 60, Easing.CubicOut),
            element.FadeTo(0.86, 60, Easing.CubicOut));

        await Task.WhenAll(
            element.ScaleTo(1, 90, Easing.CubicOut),
            element.FadeTo(1, 80, Easing.CubicOut));
    }

    private void UpdateVisualState()
    {
        ApplyItem(HomeItem, HomeIcon, HomeText, ActiveTab == "Home");
        ApplyItem(HistoryItem, HistoryIcon, HistoryText, ActiveTab == "History");
        ApplyItem(SettingsItem, SettingsIcon, SettingsText, ActiveTab == "Settings");
    }

    private static void ApplyItem(Border item, Label icon, Label text, bool active)
    {
        item.BackgroundColor = Color.FromArgb(active ? "#2E7CFF" : "#00000000");
        icon.TextColor = Color.FromArgb(active ? "#FFFFFF" : "#8EA3BF");
        text.TextColor = Color.FromArgb(active ? "#FFFFFF" : "#8EA3BF");
        item.Scale = active ? 1.03 : 1;
    }
}
