using Microsoft.AspNetCore.Mvc;

namespace MVC.Extensions;

public class SummaryViewComponent : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(View());
    }
}
