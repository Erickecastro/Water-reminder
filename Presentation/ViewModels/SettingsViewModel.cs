using System.Windows.Input;
using Hydra.Core.Interfaces;
using Hydra.Core.Models;

namespace Hydra.Presentation.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly IUserRepository _userRepository;
    private int _dailyGoalMl = 2000;
    private bool _notificationsEnabled = true;
    private AppTheme _selectedTheme = AppTheme.Unspecified;
    private string _statusMessage = "Personalize sua experiência";
    private bool _isBusy;

    public SettingsViewModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        SaveCommand = new Command(async () => await SaveAsync(), () => !IsBusy);
        ToggleThemeCommand = new Command<string>(theme => ApplyTheme(theme));
    }

    public ICommand SaveCommand { get; }
    public ICommand ToggleThemeCommand { get; }

    public int DailyGoalMl
    {
        get => _dailyGoalMl;
        set => SetProperty(ref _dailyGoalMl, Math.Clamp(value, 1000, 6000));
    }

    public bool NotificationsEnabled
    {
        get => _notificationsEnabled;
        set => SetProperty(ref _notificationsEnabled, value);
    }

    public AppTheme SelectedTheme
    {
        get => _selectedTheme;
        set => SetProperty(ref _selectedTheme, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                (SaveCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public async Task LoadAsync()
    {
        var user = await EnsureUserAsync();
        DailyGoalMl = user.DailyGoalMl;
        NotificationsEnabled = user.NotificationsEnabled;
        SelectedTheme = user.PreferredTheme switch
        {
            "light" => AppTheme.Light,
            "dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
        Application.Current!.UserAppTheme = SelectedTheme;
    }

    private async Task SaveAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var user = await EnsureUserAsync();
            user.DailyGoalMl = DailyGoalMl;
            user.NotificationsEnabled = NotificationsEnabled;
            user.PreferredTheme = SelectedTheme switch
            {
                AppTheme.Light => "light",
                AppTheme.Dark => "dark",
                _ => "system"
            };
            user.LastUpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            StatusMessage = "Configurações salvas com sucesso.";
        }
        catch
        {
            StatusMessage = "Erro ao salvar. Tente novamente.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyTheme(string? theme)
    {
        SelectedTheme = theme?.ToLowerInvariant() switch
        {
            "light" => AppTheme.Light,
            "dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };

        if (Application.Current != null)
        {
            Application.Current.UserAppTheme = SelectedTheme;
        }
    }

    private async Task<User> EnsureUserAsync()
    {
        var user = await _userRepository.GetFirstUserAsync();
        if (user != null)
        {
            return user;
        }

        user = new User
        {
            Name = "Você",
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            DailyGoalMl = 2000,
            OnboardingCompleted = true,
            PreferredTheme = "system",
            NotificationsEnabled = true
        };
        await _userRepository.AddAsync(user);
        return user;
    }
}
