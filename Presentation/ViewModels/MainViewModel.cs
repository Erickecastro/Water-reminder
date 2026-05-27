using System.Windows.Input;
using Hydra.Core.Interfaces;
using Hydra.Core.Models;

namespace Hydra.Presentation.ViewModels;

public class MainViewModel
{
    private readonly IHydrationService _hydrationService;
    private readonly IUserRepository _userRepository;
    private int _todayTotal = 0;
    private int _dailyGoal = 2000;
    private string _progressText = "0 / 2000 ml";

    public MainViewModel(IHydrationService hydrationService, IUserRepository userRepository)
    {
        _hydrationService = hydrationService;
        _userRepository = userRepository;
        Add100Command = new Command(async () => await QuickAddAsync(100));
        Add250Command = new Command(async () => await QuickAddAsync(250));
        Add500Command = new Command(async () => await QuickAddAsync(500));
        Add1000Command = new Command(async () => await QuickAddAsync(1000));
        LoadDataCommand = new Command(async () => await LoadDataAsync());
    }

    public ICommand Add100Command { get; }
    public ICommand Add250Command { get; }
    public ICommand Add500Command { get; }
    public ICommand Add1000Command { get; }
    public ICommand LoadDataCommand { get; }

    public int TodayTotal
    {
        get => _todayTotal;
        set { _todayTotal = value; OnPropertyChanged(nameof(TodayTotal)); }
    }

    public int DailyGoal
    {
        get => _dailyGoal;
        set { _dailyGoal = value; OnPropertyChanged(nameof(DailyGoal)); }
    }

    public string ProgressText
    {
        get => _progressText;
        set { _progressText = value; OnPropertyChanged(nameof(ProgressText)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }

    private async Task QuickAddAsync(int ml)
    {
        var user = await _userRepository.GetFirstUserAsync();
        if (user == null)
        {
            user = new Hydra.Core.Models.User
            {
                Name = "You",
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                DailyGoalMl = 2000,
                OnboardingCompleted = false
            };
            await _userRepository.AddAsync(user);
        }

        var entry = new HydrationEntry
        {
            UserId = user.Id,
            AmountMl = ml,
            IntakeTime = DateTime.UtcNow,
            IsQuickAdd = true,
            Source = "quick_add"
        };

        await _hydrationService.AddIntakeAsync(entry);
        await LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        var user = await _userRepository.GetFirstUserAsync();
        if (user == null)
        {
            DailyGoal = 2000;
            TodayTotal = 0;
        }
        else
        {
            DailyGoal = user.DailyGoalMl;
            TodayTotal = await _hydrationService.GetTodayTotalAsync(user.Id);
        }

        ProgressText = $"{TodayTotal} / {DailyGoal} ml";
    }
}
