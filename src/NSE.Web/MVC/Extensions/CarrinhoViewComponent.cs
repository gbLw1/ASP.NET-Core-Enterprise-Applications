using Microsoft.AspNetCore.Mvc;
using MVC.Services;

namespace MVC.Extensions;

public class CarrinhoViewComponent : ViewComponent
{
    private readonly IComprasBffService _comprasBffService;

    public CarrinhoViewComponent(IComprasBffService comprasBffService)
    {
        _comprasBffService = comprasBffService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _comprasBffService.ObterQuantidadeCarrinho());
    }
}
