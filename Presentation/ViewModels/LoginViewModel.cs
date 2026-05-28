using System.Windows.Input;
using Hydra.Core.Interfaces;

namespace Hydra.Presentation.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IUserSessionService _sessionService;
    private string _name = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isBusy;

    public LoginViewModel(IUserSessionService sessionService)
    {
        _sessionService = sessionService;
        SignInCommand = new Command(async () => await SignInAsync(), () => !IsBusy);
    }

    public event EventHandler? SignedIn;

    public ICommand SignInCommand { get; }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                (SignInCommand as Command)?.ChangeCanExecute();
            }
        }
    }

    public void ClearForm()
    {
        Name = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        ErrorMessage = string.Empty;
    }

    private async Task SignInAsync()
    {
        if (IsBusy)
        {
            return;
        }

        ErrorMessage = string.Empty;
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Preencha nome, e-mail e senha para continuar.";
            return;
        }

        IsBusy = true;
        try
        {
            await _sessionService.SignInAsync(Name, Email, Password);
            SignedIn?.Invoke(this, EventArgs.Empty);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
