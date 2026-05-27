using Hydra.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Water_reminder;

public partial class MainPage : ContentPage
{
	private readonly MainViewModel _vm;

	public MainPage()
	{
		InitializeComponent();

		var services = Application.Current?.Handler?.MauiContext?.Services;
        if (services != null)
        {
            _vm = services.GetService<MainViewModel>() ?? new MainViewModel(
                services.GetRequiredService<Hydra.Core.Interfaces.IHydrationService>(),
                services.GetRequiredService<Hydra.Core.Interfaces.IUserRepository>()
            );
        }
        else
        {
            _vm = new MainViewModel(null!, null!);
        }

        BindingContext = _vm;

        // Load initial data
        _ = Task.Run(() => _vm.LoadDataAsync());
	}
}

