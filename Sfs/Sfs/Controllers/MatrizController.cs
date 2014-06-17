using Sfs.Models;
using Sfs.Services;
using Sfs.ViewModels.MatrizViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sfs.Controllers {
    public class MatrizController : CustomController {

        public ActionResult Index(IndexViewModel viewModel) {
            viewModel.Lista = Context.Pessoas.AsEnumerable();
            viewModel = (IndexViewModel)Servico.PaginarLista<Pessoa>(viewModel, "Matricula", false);
            return View(viewModel);
        }

        public ActionResult Details(Guid id) {
            var pessoa = Context.Pessoas.Find(id);
            return View(pessoa);
        }
    }
}