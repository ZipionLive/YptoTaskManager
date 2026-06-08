using Microsoft.AspNetCore.Components;
using YptoTaskManager.FE.Web.Dtos.Tasks;

namespace YptoTaskManager.FE.Web.Components.Tasks;

public partial class TaskCard
{
    [Parameter]
    public required TaskItemDto Task { get; set; }

    [Parameter]
    public EventCallback<TaskItemDto> OnOpen { get; set; }

    private Task OnOpenClicked()
    {
        return OnOpen.InvokeAsync(Task);
    }
}