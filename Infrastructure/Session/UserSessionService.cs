using System.Text.Json;
using Hydra.Core.Interfaces;
using Hydra.Core.Models;
using Microsoft.Maui.Storage;

namespace Hydra.Infrastructure.Session;

public class UserSessionService : IUserSessionService
{
    private const string SessionKey = "hidrate_user_session_v1";

    public bool IsSignedIn => CurrentSession is not null;
    public UserSession? CurrentSession { get; private set; }

    public Task InitializeAsync()
    {
        try
        {
            var json = Preferences.Default.Get(SessionKey, string.Empty);
            CurrentSession = string.IsNullOrWhiteSpace(json)
                ? null
                : JsonSerializer.Deserialize<UserSession>(json);
        }
        catch
        {
            CurrentSession = null;
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

        SaveSession();
        return Task.CompletedTask;
    }

    public Task UpdateProfileAsync(string name, string email, string password)
    {
        CurrentSession = new UserSession
        {
            Name = name.Trim(),
            Email = email.Trim(),
            Password = password
        };

        SaveSession();
        return Task.CompletedTask;
    }

    public Task SignOutAsync()
    {
        CurrentSession = null;
        Preferences.Default.Remove(SessionKey);
        return Task.CompletedTask;
    }

    private void SaveSession()
    {
        if (CurrentSession is null)
        {
            Preferences.Default.Remove(SessionKey);
            return;
        }

        Preferences.Default.Set(SessionKey, JsonSerializer.Serialize(CurrentSession));
    }
}
