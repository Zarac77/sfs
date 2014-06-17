using Sfs.Models;
using Sfs.Services;
using Sfs.ViewModels;
using Sfs.ViewModels.ControleViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sfs.Controllers
{
    public class ControleController : CustomController
    {
        //
        // GET: /Controle/

        public ActionResult Index()
        {
            var atividadesAtivas = ServicoAtividade.GetAtividadesValidadas(Context);
            if (PessoaLogada.IsControle)
                return View(atividadesAtivas);
            else return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
        }

        public ActionResult Ver(Atividade atividade) {
            atividade = Context.Atividades.Find(atividade.Id);
            if (PessoaLogada.IsControle)
                return View(new _ListarViewModel { Atividade = atividade, IdAtividade = atividade.Id, MostrarCheckboxes = true });
            else return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
        }

        [HttpPost]
        public ActionResult ConfirmarPresencas(ConfirmarPresencasViewModel cpvm) {
            foreach (var id in cpvm.IdSelecionados) {
                var ins = Context.Inscricoes.Find(id);
                ins.Presente = true;
                Context.Entry(ins).State = EntityState.Modified;
            }
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
