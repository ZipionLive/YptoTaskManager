using Microsoft.AspNetCore.Components;
using YptoTaskManager.FE.Web.Dtos.Tasks;
using YptoTaskManager.FE.Web.Dtos.Users;

namespace YptoTaskManager.FE.Web.Components.Tasks;

public partial class TaskCard
{
    [Parameter]
    public required TaskItemDto Task { get; set; }

    [Parameter]
    public IReadOnlyCollection<UserDto> Users { get; set; } = [];

    [Parameter]
    public EventCallback<TaskItemDto> OnOpen { get; set; }

    private IEnumerable<string> AssignedUserNames =>
        Users
            .Where(u => Task.AssignedUserIds.Contains(u.Id))
            .Select(u => $"{u.FirstName} {u.LastName}");

    private Task OnOpenClicked()
    {
        return OnOpen.InvokeAsync(Task);
    }
}