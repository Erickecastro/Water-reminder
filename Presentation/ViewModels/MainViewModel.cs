using System.Windows.Input;
using Hydra.Core.Interfaces;
using Hydra.Core.Models;
using Microsoft.Maui.Controls;

namespace Hydra.Presentation.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IHydrationService _hydrationService;
    private readonly IUserRepository _userRepository;
    private readonly IUserSessionService _sessionService;
    private readonly SemaphoreSlim _addLock = new(1, 1);
    private int _todayTotal = 0;
    private int _dailyGoal = 2000;
    private string _progressText = "0 / 2000 ml";
    private double _progressPercent = 0;
    private int _remainingMl = 2000;
    private bool _isBusy;
    private string _statusLabel = "Faltam 2000 ml para sua meta";
    private string _motivationMessage = "Seu corpo agradece cada gole.";
    private string _lastIntakeLabel = "Nenhum registro hoje";
    private string _welcomeText = "Dizer que esqueceu de beber água não será mais desculpa!";

    public MainViewModel(
        IHydrationService hydrationService,
        IUserRepository userRepository,
        IUserSessionService sessionService)
    {
        _hydrationService = hydrationService;
        _userRepository = userRepository;
        _sessionService = sessionService;
        Add100Command = new Command(async () => await QuickAddAsync(100));
        Add250Command = new Command(async () => await QuickAddAsync(250));
        Add500Command = new Command(async () => await QuickAddAsync(500));
        Add1000Command = new Command(async () => await QuickAddAsync(1000));
        LoadDataCommand = new Command(async () => await LoadDataAsync());
        ResetTodayCommand = new Command(async () => await ResetTodayAsync(), () => !IsBusy);
    }

    public ICommand Add100Command { get; }
    public ICommand Add250Command { get; }
    public ICommand Add500Command { get; }
    public ICommand Add1000Command { get; }
    public ICommand LoadDataCommand { get; }
    public ICommand ResetTodayCommand { get; }

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
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                (ResetTodayCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public string StatusLabel
    {
        get => _statusLabel;
        set { _statusLabel = value; OnPropertyChanged(nameof(StatusLabel)); }
    }

    public string MotivationMessage
    {
        get => _motivationMessage;
        set => SetProperty(ref _motivationMessage, value);
    }

    public string LastIntakeLabel
    {
        get => _lastIntakeLabel;
        set => SetProperty(ref _lastIntakeLabel, value);
    }

    public string WelcomeText
    {
        get => _welcomeText;
        set => SetProperty(ref _welcomeText, value);
    }

    public async Task ResetTodayAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var user = await EnsureUserAsync();
            if (user is null)
            {
                return;
            }

            await _hydrationService.ClearTodayAsync(user.Id);
            await LoadDataAsync();
            StatusLabel = "Consumo de hoje zerado";
            MotivationMessage = "Tudo pronto para recomeçar!";
        }
        catch
        {
            StatusLabel = "Não foi possível zerar agora.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task QuickAddAsync(int ml)
    {
        if (IsBusy) return;

        await _addLock.WaitAsync();
        try
        {
            IsBusy = true;

            var user = await EnsureUserAsync();
            if (user is null)
            {
                StatusLabel = "Não foi possível acessar o usuário.";
                return;
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
            WelcomeText = "Dizer que esqueceu de beber água não será mais desculpa!";

            var user = await EnsureUserAsync();
            if (user == null)
            {
                DailyGoal = 2000;
                TodayTotal = 0;
                LastIntakeLabel = "Sem registros hoje";
            }
            else
            {
                DailyGoal = user.DailyGoalMl;
                var todayEntries = (await _hydrationService.GetTodayEntriesAsync(user.Id))
                    .OrderByDescending(e => e.IntakeTime)
                    .ToList();
                TodayTotal = todayEntries.Sum(e => e.AmountMl);
                LastIntakeLabel = todayEntries.Count == 0
                    ? "Nenhum registro hoje"
                    : $"Último registro: {todayEntries[0].AmountMl} ml às {todayEntries[0].IntakeTime.ToLocalTime():HH:mm}";
            }

            ProgressPercent = DailyGoal > 0 ? Math.Min(1.0, TodayTotal / (double)DailyGoal) : 0;
            RemainingMl = Math.Max(0, DailyGoal - TodayTotal);
            ProgressText = $"{TodayTotal} / {DailyGoal} ml";
            StatusLabel = RemainingMl <= 0 ? "Meta alcançada!" : $"Faltam {RemainingMl} ml para sua meta";
            MotivationMessage = RemainingMl <= 0
                ? "Excelente! Continue mantendo seu ritmo."
                : RemainingMl <= 500
                    ? "Você está quase lá. Só mais um pouco!"
                    : "Seu corpo agradece cada gole.";
        }
        catch (Exception)
        {
            DailyGoal = 2000;
            TodayTotal = 0;
            ProgressPercent = 0;
            RemainingMl = 2000;
            ProgressText = "0 / 2000 ml";
            StatusLabel = "Não foi possível atualizar os dados agora.";
            LastIntakeLabel = "Sem dados";
            MotivationMessage = "Vamos retomar sua hidratação.";
        }
    }

    private async Task<User?> EnsureUserAsync()
    {
        var user = await _userRepository.GetFirstUserAsync();
        if (user != null)
        {
            var sessionName = _sessionService.CurrentSession?.Name;
            if (!string.IsNullOrWhiteSpace(sessionName) && !string.Equals(user.Name, sessionName, StringComparison.Ordinal))
            {
                user.Name = sessionName;
                user.LastUpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
            }
            return user;
        }

        user = new User
        {
            Name = _sessionService.CurrentSession?.Name ?? "Você",
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            DailyGoalMl = 2000,
            OnboardingCompleted = true,
            PreferredTheme = "system"
        };

        await _userRepository.AddAsync(user);
        return user;
    }
}
