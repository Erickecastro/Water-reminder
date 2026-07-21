using Water_reminder.Views;

namespace Water_reminder;

public partial class MainContainerPage : ContentPage
{
    private readonly HomeView _homeView;
    private readonly HistoryView _historyView;
    private readonly SettingsView _settingsView;

    private string _currentTab = "Home";


    public MainContainerPage(
        HomeView homeView,
        HistoryView historyView,
        SettingsView settingsView)
    {
        InitializeComponent();

        _homeView = homeView;
        _historyView = historyView;
        _settingsView = settingsView;


        ShowHome();
    }



    public async void ShowHome()
    {
        await SwitchContent(_homeView, "Home");
    }



    public async void ShowHistory()
    {
        await SwitchContent(_historyView, "History");
    }



    public async void ShowSettings()
    {
        await SwitchContent(_settingsView, "Settings");
    }




    private async Task SwitchContent(View view, string targetTab)
    {
        if (ContentHost.Content == view)
            return;



        UpdateBottomBar(targetTab);



        int currentIndex = GetTabIndex(_currentTab);
        int targetIndex = GetTabIndex(targetTab);


        int direction = targetIndex > currentIndex ? 1 : -1;


        var oldView = ContentHost.Content as VisualElement;


        view.TranslationX = 40 * direction;
        view.Opacity = 0;



        ContentHost.Content = view;



        if (oldView != null)
        {
            await oldView.TranslateTo(
                -40 * direction,
                0,
                160,
                Easing.CubicIn);
        }




        await Task.WhenAll(

            view.TranslateTo(
                0,
                0,
                220,
                Easing.CubicOut),


            view.FadeTo(
                1,
                220,
                Easing.CubicOut)

        );




        if (oldView != null)
        {
            oldView.TranslationX = 0;
        }



        _currentTab = targetTab;
    }





    private void OnNavigateRequested(object? sender, string tab)
    {
        switch(tab)
        {
            case "Home":
                ShowHome();
                break;


            case "History":
                ShowHistory();
                break;


            case "Settings":
                ShowSettings();
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