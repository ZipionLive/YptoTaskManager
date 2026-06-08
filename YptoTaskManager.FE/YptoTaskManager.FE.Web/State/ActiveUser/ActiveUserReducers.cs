using Fluxor;

namespace YptoTaskManager.FE.Web.State.ActiveUser;

public static class ActiveUserReducers
{
    [ReducerMethod]
    public static ActiveUserState ReduceSetActiveUser(ActiveUserState state, SetActiveUserAction action)
    {
        return state with
        {
            User = action.User
        };
    }

    [ReducerMethod]
    public static ActiveUserState ReduceClearActiveUser(
        ActiveUserState state,
        ClearActiveUserAction action)
    {
        return state with
        {
            User = null
        };
    }
}
