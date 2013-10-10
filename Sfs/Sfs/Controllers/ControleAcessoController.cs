using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

using Sfs.Models;
using Sfs.ViewModels.ControleAcessoViewModels;
using Sfs.Services;

namespace Sfs.Controllers
{
    public class ControleAcessoController : CustomController
    {
        //
        // GET: /ControleAcesso/Login

        public ActionResult Login()
        {

            return View(new LoginViewModel { Email = "wcardoso2011@escolasesc.g12.br" });
        }

        public ActionResult AcessoNaoAutorizado()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(viewModel.Email, viewModel.Senha))
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                    var pessoa = Context.Pessoas.Single(p => p.Email == viewModel.Email);
                    string assunto = "Login efetuado com sucesso";
                    string remetente = "Sistema";
                    string texto = "Parabéns! \n Você, " + pessoa.Nome + ", de matrícula " + pessoa.Matricula + ", efetuou o login no sistema com sucesso. \n \n Atenciosamente, O Sistema.";
                    ServicoMensageiro.EnviarMensagem(Context, pessoa, assunto, remetente, texto);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Usuário e/ou senha incorretas.");
            }

            return View(viewModel);
        }

        public ActionResult AlterarSenha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AlterarSenha(AlterarSenhaViewModel viewModel)
        {
            if (ServicoControleAcesso.AlterarSenha(Context, PessoaLogada.Id, viewModel.SenhaAtual, viewModel.NovaSenha,
                viewModel.ConfirmacaoNovaSenha))
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Não foi possível alterar a senha. Verifique a senha atual e a nova senha.");
            return View(viewModel);
        }

        public ActionResult LogOut() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}