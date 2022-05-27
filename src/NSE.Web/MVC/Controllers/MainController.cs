using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace MVC.Controllers;

public class MainController : Controller
{
    protected bool ResponseResultPossuiErros(ResponseResult? resposta)
    {
        if (resposta != null && resposta.Errors!.Mensagens!.Any())
        {
            return true;
        }

        return false;
    }
}
