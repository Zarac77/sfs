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

            return View(new LoginViewModel { Email = "admin@whatever.com.br" });
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
                var pessoa = Context.Pessoas.SingleOrDefault(p => p.Email == viewModel.Email);
                if (pessoa != null && ServicoControleAcesso.CompararSHA1(pessoa.Senha, ServicoControleAcesso.HashSenha(viewModel.Email, viewModel.Senha)))
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Email, false);
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