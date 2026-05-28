namespace Water_reminder;

public partial class AppShell : Shell
{
	public AppShell(MainPage mainPage, HistoryPage historyPage, SettingsPage settingsPage)
	{
		InitializeComponent();

		Items.Clear();
		Items.Add(new TabBar
		{
			Items =
			{
				new ShellContent
				{
					Title = "Início",
					Route = nameof(MainPage),
					Content = mainPage
				},
				new ShellContent
				{
					Title = "Histórico",
					Route = nameof(HistoryPage),
					Content = historyPage
				},
				new ShellContent
				{
					Title = "Ajustes",
					Route = nameof(SettingsPage),
					Content = settingsPage
				}
			}
		});
	}
}
