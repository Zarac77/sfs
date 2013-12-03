using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Sfs.Models;
using Sfs.ViewModels.AtividadeViewModels;
using Sfs.Services;
using Sfs.ViewModels;

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
            PrepararIndexViewModel(viewModel);
            return View(viewModel);
        }

        private void PrepararIndexViewModel(IndexViewModel viewModel) {
            var atividades = Context.Atividades.Include(atividade => atividade.Inscricoes);

            if (!viewModel.ExibirArquivadas)
                atividades = atividades.Where(a => a.EstadoAtividade.Indice != EstadoAtividade.Arquivada);

            viewModel.EstadosAtividade = ServicoAtividade.DicFromEstadosAtividade(Context);

            viewModel.Atividades = atividades.ToList();
        }

        //
        // GET: /Atividades/Details/5

        public ViewResult Details(System.Guid id)
        {
            Atividade atividade = Context.Atividades.Single(x => x.Id == id);
            return View(atividade);
        }

        //Mandar para a classe de serviço
        public ActionResult GerarLista(GerarListaViewModel glvm)
        {
            glvm = PrepararGerarLista(Context, glvm, PessoaLogada);
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
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            string log = ServicoAtividade.InscricoesAcaoLote(Context, lvm.IdAtividade, lvm.IdSelecionados, ServicoAtividade.CancelarInscricao, this.ModelState);
            ServicoLog.Log(Context, "Inscrições", log);
            return RedirectToAction("GerarLista", new { IdAtividade = lvm.IdAtividade });
        }

        [HttpPost]
        public ActionResult ForcarInscricoes(ListarViewModel lvm)
        {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            string log = ServicoAtividade.InscricoesAcaoLote(Context, lvm.IdAtividade, lvm.IdSelecionados, ServicoAtividade.Inscrever, this.ModelState);
            ServicoLog.Log(Context, "Inscrições", log);
            return RedirectToAction("GerarLista", new { IdAtividade = lvm.IdAtividade, Matricula = lvm.CampoMatricula, Turma = lvm.CampoTurma});
        }
        
        public ActionResult FixarInscricoes(ListarViewModel lvm) {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            ServicoAtividade.SetFixadoInscricao(Context, lvm.IdSelecionados, lvm.IdAtividade, true);
            return RedirectToAction("GerarLista");
        }

        [HttpPost]
        public ActionResult DesafixarInscricoes(ListarViewModel lvm) {
            if (!PessoaLogada.IsAdministrador)
                return RedirectToAction("AcessoNaoAutorizado", "ControleAcesso");
            ServicoAtividade.SetFixadoInscricao(Context, lvm.IdSelecionados, lvm.IdAtividade, false);
            return RedirectToAction("GerarLista", new { IdAtividade = lvm.IdAtividade, Matricula = lvm.CampoMatricula, Turma = lvm.CampoTurma});
        }



        //
        // GET: /Atividades/Create

        public ActionResult Create()
        {
            return View(PrepararCreateViewModel(Context, Guid.Empty));
        } 

        //
        // POST: /Atividades/Create

        [HttpPost]
        public ActionResult Create(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ServicoAtividade.CreateOrEdit(Context, viewModel);
                ServicoAtividade.RecalcularCSInscricoesAtivas(Context);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
        
        //
        // GET: /Atividades/Edit/5
 
        public ActionResult Edit(System.Guid id)
        {
            var viewModel = PrepararCreateViewModel(Context, id);
            return View(viewModel);
        }

        //
        // POST: /Atividades/Edit/5

        [HttpPost]
        public ActionResult Edit(CreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
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
            ServicoAtividade.RecalcularCSInscricoesAtivas(Context);
            return RedirectToAction("Index");
        }

        public ActionResult Inscrever(Atividade atividade)
        {
            var log = ServicoAtividade.Inscrever(Context, atividade, PessoaLogada.Id, this.ModelState);
            if (this.ModelState.Count > 0) {
                var viewModel = new IndexViewModel();
                PrepararIndexViewModel(viewModel);
                return View("Index", viewModel);
            }
            ServicoLog.Log(Context, "Inscrições", log);
            Context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Desistir(Atividade atividade)
        {
            atividade = Context.Atividades.Include(a => a.Inscricoes).Single(a => a.Id == atividade.Id);
            var log = ServicoAtividade.CancelarInscricao(Context, atividade, PessoaLogada.Id, null);
            ServicoLog.Log(Context, "Inscrições", log);
            Context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        #region Preparar ViewModels
        /// <summary>
        /// Completa o View Model para gerar a lista de inscritos da atividade.
        /// </summary>
        /// <param name="context">A classe de contexto.</param>
        /// <param name="glvm">O View Model passado anteriormente (vazio ou não).</param>
        /// <returns></returns>
        public static GerarListaViewModel PrepararGerarLista(SfsContext context, GerarListaViewModel glvm, Pessoa pessoaLogada) {
            glvm.Atividade = context.Atividades.Single(a => a.Id == glvm.IdAtividade);
            glvm._ListarViewModel = new _ListarViewModel {
                Atividade = glvm.Atividade,
                IdAtividade = glvm.IdAtividade,
                MostrarCheckboxes = pessoaLogada.IsAdministrador || pessoaLogada.IsControle
            };
            glvm.Atividade.Inscricoes = glvm.Atividade.Inscricoes.OrderByDescending(i => i.CoeficienteSorte).ToList();
            //Caso contrário, uma pesquisa vazia sempre retornará a todos.
            if (String.IsNullOrEmpty(glvm.Matricula) && String.IsNullOrEmpty(glvm.Turma))
                return glvm;
            var pessoaModelo = new Pessoa { Matricula = glvm.Matricula, Turma = glvm.Turma, Ativo = true, AnoEntrada = null };
            glvm.ResultadoPessoas = ServicoPessoa.BuscarPessoas(context, pessoaModelo);
            return glvm;
        }

        public static CreateViewModel PrepararCreateViewModel(SfsContext context, Guid id) {
            CreateViewModel cvm;
            if (id != Guid.Empty) {
                var atividade = context.Atividades.Find(id);
                cvm = new CreateViewModel {
                    Atividade = atividade,
                    IdAtividade = atividade.Id,
                    DataInicio = DateTime.Parse(atividade.DataHoraInicio.ToString("dd/MM/yyyy")),
                    HoraInicio = atividade.DataHoraInicio.TimeOfDay.ToString("c"),
                    DataFim = DateTime.Parse(atividade.DataHoraFim.ToString("dd/MM/yyyy")),
                    HoraFim = atividade.DataHoraFim.TimeOfDay.ToString("c"),
                    DataLimiteCancelamento = DateTime.Parse(atividade.DataLimiteCancelamento.ToString("dd/MM/yyyy")),
                    HoraLimiteCancelamento = atividade.DataLimiteCancelamento.TimeOfDay.ToString("c"),
                    DataLimiteInscricao = DateTime.Parse(atividade.DataLimiteInscricao.ToString("dd/MM/yyyy")),
                    HoraLimiteInscricao = atividade.DataLimiteInscricao.TimeOfDay.ToString("c"),
                    _EstadosAtividade = context.EstadosAtividade.ToList()
                };
            }
            else {
                var guid = Guid.NewGuid();
                cvm = new CreateViewModel {
                    Atividade = new Atividade { Id = guid },
                    IdAtividade = guid
                };
            }
            cvm.EstadosAtividade = Servico.DicFromEstadosAtividade(context);
            return cvm;
        }

        #endregion
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