using YptoTaskManager.FE.Web.Dtos.Users;

namespace YptoTaskManager.FE.Web.State.ActiveUser;

public record SetActiveUserAction(UserDto User);

public record ClearActiveUserAction;