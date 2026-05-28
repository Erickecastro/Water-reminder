using Hydra.Core.Interfaces;
using Hydra.Core.Models;
using Microsoft.Maui.Storage;

namespace Hydra.Infrastructure.Session;

/// <summary>
/// Sessão apenas em memória (modo teste). Ao fechar o app, o login é exibido novamente.
/// </summary>
public class UserSessionService : IUserSessionService
{
    private const string LegacySessionKey = "hydra_user_session_v1";

    public bool IsSignedIn => CurrentSession is not null;
    public UserSession? CurrentSession { get; private set; }

    public Task InitializeAsync()
    {
        CurrentSession = null;

        // Remove dados de versões anteriores que persistiam credenciais.
        try
        {
            Preferences.Default.Remove(LegacySessionKey);
        }
        catch
        {
            // ignore
        }

        return Task.CompletedTask;
    }

    public Task SignInAsync(string name, string email, string password)
    {
        CurrentSession = new UserSession
        {
            Name = name.Trim(),
            Email = email.Trim(),
            Password = password
        };

        return Task.CompletedTask;
    }

    public Task SignOutAsync()
    {
        CurrentSession = null;
        return Task.CompletedTask;
    }
}
