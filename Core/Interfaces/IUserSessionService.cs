using Hydra.Core.Models;

namespace Hydra.Core.Interfaces;

public interface IUserSessionService
{
    bool IsSignedIn { get; }
    UserSession? CurrentSession { get; }
    Task InitializeAsync();
    Task SignInAsync(string name, string email, string password);
    Task SignOutAsync();
}
