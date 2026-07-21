using Water_reminder.Views;

namespace Water_reminder;

public partial class MainContainerPage : ContentPage
{
    private readonly HomeView _homeView;
    private readonly HistoryView _historyView;
    private readonly SettingsView _settingsView;

    private string _currentTab = "Home";
    private bool _isNavigating;

    public MainContainerPage(
        HomeView homeView,
        HistoryView historyView,
        SettingsView settingsView)
    {
        InitializeComponent();

        _homeView = homeView;
        _historyView = historyView;
        _settingsView = settingsView;

        ContentHost.Content = _homeView;
        UpdateBottomBar("Home");
    }

    public Task ShowHome()
        => SwitchContent(_homeView, "Home");

    public Task ShowHistory()
        => SwitchContent(_historyView, "History");

    public Task ShowSettings()
        => SwitchContent(_settingsView, "Settings");

    private async Task SwitchContent(View view, string targetTab)
    {
        if (_isNavigating)
            return;

        if (ReferenceEquals(ContentHost.Content, view))
            return;

        _isNavigating = true;

        try
        {
            UpdateBottomBar(targetTab);

            int currentIndex = GetTabIndex(_currentTab);
            int targetIndex = GetTabIndex(targetTab);

            int direction = targetIndex > currentIndex ? 1 : -1;

            var oldView = ContentHost.Content as VisualElement;

            view.TranslationX = 24 * direction;
            view.InputTransparent = true;

            ContentHost.Content = view;

            Task? outgoingAnimation = null;

            if (oldView != null)
            {
                oldView.InputTransparent = true;

                outgoingAnimation = oldView.TranslateTo(
                    -24 * direction,
                    0,
                    180,
                    Easing.CubicIn);
            }

            await view.TranslateTo(
                0,
                0,
                180,
                Easing.CubicOut);

            if (outgoingAnimation != null)
                await outgoingAnimation;

            if (oldView != null)
            {
                oldView.TranslationX = 0;
                oldView.InputTransparent = false;
            }

            view.InputTransparent = false;

            _currentTab = targetTab;
        }
        finally
        {
            _isNavigating = false;
        }
    }

    private async void OnNavigateRequested(object? sender, string tab)
    {
        switch (tab)
        {
            case "Home":
                await ShowHome();
                break;

            case "History":
                await ShowHistory();
                break;

            case "Settings":
                await ShowSettings();
                break;
        }
    }

    private void UpdateBottomBar(string tab)
    {
        BottomNavigation.ActiveTab = tab;
    }

    private static int GetTabIndex(string tab)
    {
        return tab switch
        {
            "Home" => 0,
            "History" => 1,
            "Settings" => 2,
            _ => 0
        };
    }
}