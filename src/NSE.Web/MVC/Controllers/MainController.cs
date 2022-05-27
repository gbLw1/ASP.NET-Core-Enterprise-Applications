using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace MVC.Controllers;

public class MainController : Controller
{
    protected bool ResponseResultPossuiErros(ResponseResult? resposta)
    {
        if (resposta?.Errors?.Mensagens?.Any() ?? false)
        {
            resposta.Errors.Mensagens.ForEach(erro => ModelState.AddModelError(string.Empty, erro));
            return true;
        }

        return false;
    }
}
