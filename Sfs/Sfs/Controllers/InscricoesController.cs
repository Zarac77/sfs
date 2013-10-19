using Sfs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sfs.Controllers
{
    public class InscricoesController : CustomController
    {
        //
        // GET: /Inscricoes/

        public ActionResult Index()
        {
            var inscricoes = ServicoInscricoes.GetInscricoesAtivasPessoa(Context, PessoaLogada).OrderByDescending(i => i.Prioridade).ToList();
            return View(inscricoes);
        }


        public ActionResult SubirInscricao(Guid idInscricao) {
            var inscricao = Context.Inscricoes.Find(idInscricao);
            var inscricoes = ServicoInscricoes.GetInscricoesAtivasPessoa(Context, PessoaLogada).ToList();
            ServicoInscricoes.SubirPrioridade(Context, inscricao, inscricoes, true);
            return RedirectToAction("Index");
        }

        public ActionResult DescerInscricao(Guid idInscricao) {
            var inscricao = Context.Inscricoes.Find(idInscricao);
            var inscricoes = ServicoInscricoes.GetInscricoesAtivasPessoa(Context, PessoaLogada).ToList();
            ServicoInscricoes.DescerPrioridade(Context, inscricao, inscricoes, true);
            return RedirectToAction("Index");
        }
    }
}
