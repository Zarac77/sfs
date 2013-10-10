using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

using Sfs.Models;
using Sfs.ViewModels.PessoaViewModels;

namespace Sfs.Controllers
{   
    public class PessoaController : CustomController
    {
        //
        // GET: /Pessoa/

        private const int PESSOAS_POR_PAGINA = 30;

        public ActionResult Index(IndexViewModel viewModel)
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");

            var pessoas = Context.Pessoas.OrderBy(p => p.Nome).AsQueryable();
            var indiceInicial = (viewModel.PaginaAtual) * PESSOAS_POR_PAGINA;

            if (!String.IsNullOrEmpty(viewModel.Nome))
                pessoas = pessoas.Where(p => p.Nome.Contains(viewModel.Nome));

            if (viewModel.IgnorarInativos)
                pessoas = pessoas.Where(p => p.Ativo);

            viewModel.Pessoas = pessoas
                .OrderBy(p => p.Nome)
                .Skip(indiceInicial)
                .Take(PESSOAS_POR_PAGINA).ToList();
            int totalPessoas = pessoas.Count();
            double totalPaginas = (double)totalPessoas / PESSOAS_POR_PAGINA;
            viewModel.TotalPaginas = (int)Math.Ceiling(totalPaginas);
            return View(viewModel);
        }

        //
        // GET: /Pessoa/Details/5

        public ViewResult Details(System.Guid id)
        {
            Pessoa pessoa = Context.Pessoas.Single(x => x.Id == id);
            return View(pessoa);
        }

        //
        // GET: /Pessoa/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Pessoa/Create

        [HttpPost]
        public ActionResult Create(Pessoa pessoa, List<Perfil> perfisSelecionados)
        {
            if (ModelState.IsValid)
            {
                pessoa.Id = Guid.NewGuid();
                Context.Pessoas.Add(pessoa);
                Context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(pessoa);
        }
        
        //
        // GET: /Pessoa/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
            Pessoa pessoa = Context.Pessoas.Single(x => x.Id == id);
            return View(pessoa);
        }

        //
        // POST: /Pessoa/Edit/5

        [HttpPost]
        public ActionResult Edit(Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                Context.Entry(pessoa).State = EntityState.Modified;
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pessoa);
        }

        //
        // GET: /Pessoa/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            Pessoa pessoa = Context.Pessoas.Single(x => x.Id == id);
            return View(pessoa);
        }

        //
        // POST: /Pessoa/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            Pessoa pessoa = Context.Pessoas.Single(x => x.Id == id);
            Context.Pessoas.Remove(pessoa);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AssociarPerfis(Guid idPessoa)
        {
            var viewModel = new AssociarPerfisViewModel();
            viewModel.Pessoa = Context.Pessoas.Find(idPessoa);
            viewModel.TodosPerfis = Context.Perfis.OrderBy(p => p.Nome).ToList();
            viewModel.PerfisSelecionados = viewModel.Pessoa.Perfis.ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AssociarPerfis(AssociarPerfisViewModel viewModel)
        {
            var pessoa = Context.Pessoas.Find(viewModel.Pessoa.Id);
            pessoa.Perfis.Clear();

            foreach (var novoPerfil in viewModel.PerfisSelecionados)
            {
                var perfil = Context.Perfis.Find(novoPerfil.Id);
                pessoa.Perfis.Add(perfil);
            }

            Context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}