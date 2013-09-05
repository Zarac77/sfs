using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sfs.Models;
using System.Data;

namespace Sfs.Controllers
{
    public class ParametrosController : CustomController
    {
        //
        // GET: /Parametros/

        public ActionResult Edit()
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            var parametros = Context.ParametrosSistema.FirstOrDefault();
            return View(parametros);
        }

        [HttpPost]
        public ActionResult Edit(ParametrosSistema parametros)
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            Context.Entry(parametros).State = EntityState.Modified;
            Context.SaveChanges();
            return View(parametros);
        }

        public ActionResult SetPontuacaoPessoas()
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            foreach (var a in Context.Pessoas)
            {
                a.Pontuacao = Context.ParametrosSistema.FirstOrDefault().PontuacaoInicialAlunos;
            }
            Context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
