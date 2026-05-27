using Hydra.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Water_reminder;

public partial class MainPage : ContentPage
{
	private readonly MainViewModel _vm;

	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		_vm = viewModel;
		BindingContext = _vm;

		_ = Task.Run(async () => await _vm.LoadDataAsync());
	}
}

