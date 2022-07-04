using Core.Comunication;
using Microsoft.AspNetCore.Mvc;

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

    protected void AdicionarErroValidacao(string mensagem)
    {
        ModelState.AddModelError(string.Empty, mensagem);
    }

    protected bool OperacaoValida()
    {
        return ModelState.ErrorCount == 0;
    }
}
