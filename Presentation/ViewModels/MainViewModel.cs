using System.ComponentModel;
using System.Windows.Input;
using Hydra.Core.Interfaces;
using Hydra.Core.Models;

namespace Hydra.Presentation.ViewModels;

public class MainViewModel
{
    private readonly IHydrationService _hydrationService;
    private readonly IUserRepository _userRepository;
    private readonly SemaphoreSlim _addLock = new(1, 1);
    private int _todayTotal = 0;
    private int _dailyGoal = 2000;
    private string _progressText = "0 / 2000 ml";
    private double _progressPercent = 0;
    private int _remainingMl = 2000;
    private bool _isBusy;
    private string _statusLabel = "Faltam 2000 ml para sua meta";

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

    public double ProgressPercent
    {
        get => _progressPercent;
        set { _progressPercent = value; OnPropertyChanged(nameof(ProgressPercent)); }
    }

    public int RemainingMl
    {
        get => _remainingMl;
        set { _remainingMl = value; OnPropertyChanged(nameof(RemainingMl)); }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(nameof(IsBusy)); }
    }

    public string StatusLabel
    {
        get => _statusLabel;
        set { _statusLabel = value; OnPropertyChanged(nameof(StatusLabel)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
    }

    private async Task QuickAddAsync(int ml)
    {
        if (IsBusy) return;

        await _addLock.WaitAsync();
        try
        {
            IsBusy = true;

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
        catch (Exception)
        {
            // Keep the app alive even if the storage layer fails during testing.
            StatusLabel = "Não foi possível registrar agora. Tente novamente.";
        }
        finally
        {
            IsBusy = false;
            _addLock.Release();
        }
    }

    public async Task LoadDataAsync()
    {
        try
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

            ProgressPercent = DailyGoal > 0 ? Math.Min(1.0, TodayTotal / (double)DailyGoal) : 0;
            RemainingMl = Math.Max(0, DailyGoal - TodayTotal);
            ProgressText = $"{TodayTotal} / {DailyGoal} ml";
            StatusLabel = RemainingMl <= 0 ? "Meta alcançada!" : $"Faltam {RemainingMl} ml para sua meta";
        }
        catch (Exception)
        {
            DailyGoal = 2000;
            TodayTotal = 0;
            ProgressPercent = 0;
            RemainingMl = 2000;
            ProgressText = "0 / 2000 ml";
            StatusLabel = "Não foi possível atualizar os dados agora.";
        }
    }
}
