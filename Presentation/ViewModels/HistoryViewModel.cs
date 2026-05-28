using System.Collections.ObjectModel;
using System.Windows.Input;
using Hydra.Core.Interfaces;

namespace Hydra.Presentation.ViewModels;

public class HistoryViewModel : ViewModelBase
{
    private readonly IHydrationService _hydrationService;
    private readonly IUserRepository _userRepository;
    private string _todaySummary = "0 ml hoje";
    private bool _isBusy;

    public HistoryViewModel(IHydrationService hydrationService, IUserRepository userRepository)
    {
        _hydrationService = hydrationService;
        _userRepository = userRepository;
        RefreshCommand = new Command(async () => await LoadAsync());
    }

    public ObservableCollection<HistoryItem> Entries { get; } = new();
    public ICommand RefreshCommand { get; }

    public string TodaySummary
    {
        get => _todaySummary;
        set => SetProperty(ref _todaySummary, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var user = await _userRepository.GetFirstUserAsync();
            Entries.Clear();

            if (user is null)
            {
                TodaySummary = "Sem registros por enquanto";
                return;
            }

            var entries = (await _hydrationService.GetTodayEntriesAsync(user.Id))
                .OrderByDescending(e => e.IntakeTime)
                .ToList();

            TodaySummary = $"{entries.Sum(e => e.AmountMl)} ml hoje";
            foreach (var entry in entries)
            {
                Entries.Add(new HistoryItem(
                    entry.AmountMl,
                    entry.IntakeTime.ToLocalTime().ToString("HH:mm"),
                    entry.Source ?? "manual"));
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}

public record HistoryItem(int AmountMl, string TimeText, string Source);
