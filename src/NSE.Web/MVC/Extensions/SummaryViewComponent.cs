using Microsoft.AspNetCore.Mvc;

namespace MVC.Extensions;

public class SummaryViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
