using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Sfs.Models;
using Sfs.ViewModels.AtividadeViewModels;
using Sfs.Services;

namespace Sfs.Controllers
{   
    public class AtividadeController : CustomController
    {
        //
        // GET: /Atividades/

        public ActionResult Index(IndexViewModel viewModel)
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            var atividades = Context.Atividades.Include(atividade => atividade.Inscricoes);

            if (!viewModel.ExibirNaoAprovadas)
                atividades = atividades.Where(a => a.Aprovada);

            viewModel.Atividades = atividades.ToList();
            return View(viewModel);
        }

        //
        // GET: /Atividades/Details/5

        public ViewResult Details(System.Guid id)
        {
            Atividade atividade = Context.Atividades.Single(x => x.Id == id);
            return View(atividade);
        }

        public ActionResult GerarLista(GerarListaViewModel glvm)
        {
            if (!(string.IsNullOrEmpty(glvm.Matricula) && string.IsNullOrEmpty(glvm.Turma)))
            {
                glvm.ResultadoPessoas = Context.Pessoas.Where(p => p.Matricula == glvm.Matricula || p.Turma == glvm.Turma).OrderBy(p => p.Nome);
            }
            glvm.Atividade = Context.Atividades.Find(glvm.IdAtividade);
            return View(glvm);
        }

        [HttpPost]
            public ActionResult ForcarInscricoes(ForcarInscricoesViewModel fivm)
        {
            fivm.Atividade = Context.Atividades.Find(fivm.IdAtividade);
            var novasInscricoes = fivm.IdSelecionados.Where(s => !fivm.Atividade.Inscricoes.Exists(i => i.Id == s));
            foreach (var i in novasInscricoes)
            {
                var insc = new Inscricao { 
                    Pessoa = Context.Pessoas.Find(i),
                    Id = Guid.NewGuid(), IdPessoa = i, 
                    IdAtividade = fivm.Atividade.Id, 
                    Atividade = fivm.Atividade
                };
                Context.Atividades.Find(fivm.IdAtividade).Inscricoes.Add(insc);
            }
            Context.SaveChanges();
            return RedirectToAction("GerarLista", new { IdAtividade = fivm.IdAtividade, Matricula = fivm.CampoMatricula, Turma = fivm.CampoTurma});
        }



        //
        // GET: /Atividades/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Atividades/Create

        [HttpPost]
        public ActionResult Create(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var atividade = new Atividade()
                {
                    Id = Guid.NewGuid(),
                    DataHoraInicio = viewModel.DataInicio,
                    DataHoraFim = viewModel.DataFim,
                    Descricao = viewModel.Atividade.Descricao,
                    DataLimiteCancelamento = viewModel.Atividade.DataLimiteCancelamento,
                    DataLimiteInscricao = viewModel.Atividade.DataLimiteInscricao,
                    NumeroVagas = viewModel.Atividade.NumeroVagas
                };
                Context.Atividades.Add(atividade);
                Context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(viewModel);
        }
        
        //
        // GET: /Atividades/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
            Atividade atividade = Context.Atividades.Find(id);
            CreateViewModel viewModel = new CreateViewModel { Atividade = atividade, IdAtividade = atividade.Id };
            return View(viewModel);
        }

        //
        // POST: /Atividades/Edit/5

        [HttpPost]
        public ActionResult Edit(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var atividade = viewModel.Atividade;
                Context.Entry(atividade).State = EntityState.Modified;
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        //
        // GET: /Atividades/Delete/5
 
        public ActionResult Delete(System.Guid id)
        {
            Atividade atividade = Context.Atividades.Single(x => x.Id == id);
            return View(atividade);
        }

        //
        // POST: /Atividades/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(System.Guid id)
        {
            Atividade atividade = Context.Atividades.Single(x => x.Id == id);
            Context.Atividades.Remove(atividade);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Inscrever(Atividade atividade)
        {
            atividade = Context.Atividades.Single(a => a.Id == atividade.Id);
            Context.Inscricoes.Add(new Inscricao() { IdAtividade = atividade.Id, Id = Guid.NewGuid(), IdPessoa = PessoaLogada.Id});
            
            Context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Desistir(Atividade atividade)
        {
            ServicoAtividade.CancelarInscricao(Context, atividade, PessoaLogada);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CancelarInscricao(Atividade atividade, Pessoa pessoa) {
            ServicoAtividade.CancelarInscricao(Context, atividade, pessoa);
            return RedirectToAction("Listar", new { atividade.Id });
        }

        /*[HttpPost]
        public ActionResult InscreverSe(Atividade atividade) {
            var nome = context.Atividades.Find(atividade.Id);
            return View(nome);
        }*/

        #region PartialViews

        public ActionResult _ProximasAtividades()
        {
            var possuiPerfilAluno = ServicoControleAcesso.VerificarPerfil(Context, PessoaLogada.Id, Perfil.GUID_PERFIL_ALUNO);
            var possuiPerfilProfessor = ServicoControleAcesso.VerificarPerfil(Context, PessoaLogada.Id, Perfil.GUID_PERFIL_PROFESSOR);
            var viewModel = new ProximasAtividadesViewModel()
            {
                Pessoa = PessoaLogada,
                Atividades = Context.Atividades
                    //.Where(a => a.DataHoraInicio >= DateTime.Now) // linha comentada para exibir todas as atividades (debug).
                    .OrderBy(a => a.DataHoraInicio)
                    .ToList(),
                PermitirInscricao = possuiPerfilAluno || possuiPerfilProfessor
            };
            return PartialView(viewModel);
        }

        #endregion
    }
}