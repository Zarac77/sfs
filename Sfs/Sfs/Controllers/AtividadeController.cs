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

            //Sem sentido após a mudança Aprovada -> Validada. Favor rever.
            if (!viewModel.ExibirNaoAprovadas)
                atividades = atividades.Where(a => !a.Validada);

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
                glvm.ResultadoPessoas = ServicoPessoa.BuscarPessoas(Context, new Pessoa { Matricula = glvm.Matricula, Turma = glvm.Turma} );
            }
            glvm.Atividade = Context.Atividades.Include(a => a.Inscricoes).Single(a => a.Id == glvm.IdAtividade);
            glvm.Atividade.Inscricoes = glvm.Atividade.Inscricoes.OrderByDescending(i => i.CoeficienteSorte).ToList();
            return View(glvm);
        }

        [HttpPost]
        public ActionResult RedirectToListarAction(Guid[] ids, Guid idAtividade, string actionName) {
            ListarViewModel lvm = new ListarViewModel {
                IdSelecionados = ids,
                IdAtividade = idAtividade
            };
            return View(actionName, lvm);
        }

        [HttpPost]
        public ActionResult CancelarInscricoes(ListarViewModel lvm)
        {
            foreach(var id in lvm.IdSelecionados) {
                ServicoAtividade.CancelarInscricao(Context, lvm.IdAtividade, id);
            }
            //ServicoAtividade.RecalcularCSInscricoesAtivas(Context);
            return RedirectToAction("GerarLista", new { IdAtividade = lvm.IdAtividade });
        }

        [HttpPost]
        public ActionResult ForcarInscricoes(ListarViewModel lvm)
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            lvm.Atividade = Context.Atividades.Find(lvm.IdAtividade);
            var novasInscricoes = lvm.IdSelecionados.Where(s => !lvm.Atividade.Inscricoes.Exists(i => i.IdPessoa == s));
            novasInscricoes.Each(id => ServicoAtividade.Inscrever(Context, lvm.Atividade, id));
            //ServicoAtividade.RecalcularCSInscricoesAtivas(Context);
            return RedirectToAction("GerarLista", new { IdAtividade = lvm.IdAtividade, Matricula = lvm.CampoMatricula, Turma = lvm.CampoTurma});
        }
        
        public ActionResult FixarInscricoes(ListarViewModel lvm) {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            lvm.IdSelecionados.Each(id => ServicoAtividade.FixarInscricao(Context, id));
            //ServicoAtividade.RecalcularCSInscricoesAtivas(Context);
            return RedirectToAction("GerarLista", new { IdAtividade = lvm.IdAtividade, Matricula = lvm.CampoMatricula, Turma = lvm.CampoTurma });
        }

        [HttpPost]
        public ActionResult DesafixarInscricoes(ListarViewModel lvm) {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            lvm.IdSelecionados.Each(id => ServicoAtividade.DesafixarInscricao(Context, id));
            //ServicoAtividade.RecalcularCSInscricoesAtivas(Context);
            return RedirectToAction("GerarLista", new { IdAtividade = lvm.IdAtividade, Matricula = lvm.CampoMatricula, Turma = lvm.CampoTurma});
        }



        //
        // GET: /Atividades/Create

        public ActionResult Create()
        {
            return View(new CreateViewModel());
        } 

        //
        // POST: /Atividades/Create

        [HttpPost]
        public ActionResult Create(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ServicoAtividade.CreateOrEdit(Context, viewModel);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
        
        //
        // GET: /Atividades/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
            var viewModel = ServicoAtividade.CreateViewModel(Context, id);
            return View(viewModel);
        }

        //
        // POST: /Atividades/Edit/5

        [HttpPost]
        public ActionResult Edit(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
               /* 
                var atividade = viewModel.Atividade;
                Context.Entry(atividade).State = EntityState.Modified;
                Context.SaveChanges();
                
                * */
                ServicoAtividade.CreateOrEdit(Context, viewModel);
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
            //ServicoAtividade.RecalcularCSInscricoesAtivas(Context);
            return RedirectToAction("Index");
        }

        public ActionResult Inscrever(Atividade atividade)
        {
            atividade = Context.Atividades.Find(atividade.Id);
            ServicoAtividade.Inscrever(Context, atividade, PessoaLogada.Id);
            //ServicoAtividade.RecalcularCSInscricoesAtivas(Context);
       
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Desistir(Atividade atividade)
        {
            ServicoAtividade.CancelarInscricao(Context, atividade.Id, PessoaLogada.Id);
            //ServicoAtividade.RecalcularCSInscricoesAtivas(Context);
            atividade = Context.Atividades.Find(atividade.Id);
            return RedirectToAction("Index", "Home");
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
                Atividades = ServicoAtividade.GetAtividadesAtivas(Context),
                PermitirInscricao = possuiPerfilAluno || possuiPerfilProfessor
            };
            return PartialView(viewModel);
        }

        #endregion
    }
}