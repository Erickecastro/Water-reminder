using Hydra.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Water_reminder;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;

    public MainPage() : this(GetViewModel())
    {
    }

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _vm = viewModel ?? throw new InvalidOperationException("MainViewModel não pôde ser criado.");
        BindingContext = _vm;

        _ = Task.Run(async () => await _vm.LoadDataAsync());
    }

    private static MainViewModel GetViewModel()
    {
        return IPlatformApplication.Current?.Services.GetService<MainViewModel>()
            ?? throw new InvalidOperationException("Serviço MainViewModel não está registrado.");
    }
}

