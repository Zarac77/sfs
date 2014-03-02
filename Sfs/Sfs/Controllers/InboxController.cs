using Sfs.Models;
using Sfs.Services;
using Sfs.ViewModels;
using Sfs.ViewModels.InboxViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sfs.Controllers
{
    public class InboxController : CustomController
    {
        private const int MENSAGENS_POR_PAGINA = 10;
        //
        // GET: /Inbox/

        public ActionResult Index(IndexViewModel ivm)
        {
            ivm.Lista = Context.Inboxes.Single(i => i.IdPessoa == PessoaLogada.Id).Mensagens;
            ivm = (IndexViewModel)Servico.PaginarLista(ivm, "DataEnvio", true);
            return View(ivm);
        }

        public ActionResult Ler(Guid idMsg, int pagina)
        {
            var mensagem = Context.Mensagens.Find(idMsg);
            mensagem.Lida = true;
            Context.SaveChanges();
            LerViewModel viewModel = new LerViewModel { Mensagem = mensagem, PaginaOrigem = pagina };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Delete(Guid[] guid) {
            foreach (var i in guid)
            {
                if(i != Guid.Empty)
                Context.Mensagens.Remove(Context.Mensagens.Find(i));
            }
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
