namespace Water_reminder;

public partial class AppShell : Shell
{
	public AppShell(MainPage mainPage, HistoryPage historyPage, SettingsPage settingsPage)
	{
		InitializeComponent();
		Navigated += OnShellNavigated;

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

	private async void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
	{
		if (CurrentPage is not ContentPage page || page.Content is not VisualElement content)
		{
			return;
		}

		content.Opacity = 0;
		content.TranslationY = 10;
		await Task.WhenAll(
			content.FadeTo(1, 220, Easing.CubicOut),
			content.TranslateTo(0, 0, 220, Easing.CubicOut));
	}
}
