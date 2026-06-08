using Fluxor;
using YptoTaskManager.FE.Web.Dtos.Users;

namespace YptoTaskManager.FE.Web.State.ActiveUser;

[FeatureState]
public record ActiveUserState
{
    public UserDto? User { get; init; }
    public bool IsLoggedIn => User is not null;

    private ActiveUserState() { }

    public ActiveUserState(UserDto? user)
    {
        User = user;
    }
}
