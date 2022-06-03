using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("sistema-indisponivel")]
    public IActionResult SistemaIndisponivel()
    {
        var modelErro = new ErrorViewModel
        {
            Mensagem = "O sistema está temporariamente indisponível, isto pode ocorrer em momentos de sobrecarga de usuários. Tente novamente mais tarde.",
            Titulo = "Sistema indisponível.",
            ErroCode = 500
        };

        return View("Error", modelErro);
    }

    [Route("erro/{id:length(3,3)}")]
    public IActionResult Error(int id)
    {
        var modelErro = new ErrorViewModel();

        if(id == 500)
        {
            modelErro.Titulo = "Ocorreu um erro!";
            modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
            modelErro.ErroCode = id;
        }
        else if(id == 404)
        {
            modelErro.Titulo = "Ops! A página não foi encontrada!";
            modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
            modelErro.ErroCode = id;
        }
        else if(id == 403)
        {
            modelErro.Titulo = "Acesso negado";
            modelErro.Mensagem = "Você não tem permissão para fazer isto.";
            modelErro.ErroCode = id;
        }
        else
        {
            return StatusCode(404);
        }

        return View("Error", modelErro);
    }
}
