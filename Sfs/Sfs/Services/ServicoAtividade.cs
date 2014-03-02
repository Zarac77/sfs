using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;
using System.Data;
using System.Data.Entity;
using AutoMapper;
using Sfs.ViewModels.AtividadeViewModels;
using System.ComponentModel;
using Sfs.ViewModels;
using System.Web.Mvc;

namespace Sfs.Services
{

    //Em alguns pontos passando ModelState null. Mudar.
    public class ServicoAtividade : Servico
    {
        public delegate string AcaoInscricao(SfsContext context, Atividade atividade, Guid id, ModelStateDictionary modelState);

        public static string CancelarInscricao(SfsContext context, Atividade atividade, Guid idPessoa, ModelStateDictionary modelState)
        {
            //Coleta de dados:
            Inscricao inscricao = null;
            var pessoa = context.Pessoas.Find(idPessoa);
            var inscricoesAtivas = ServicoInscricoes.GetInscricoesAtivasPessoa(context, pessoa);
            try {
                inscricao = atividade.Inscricoes.SingleOrDefault(i => i.IdPessoa == idPessoa);
            }
            catch {
                var inscricoes = atividade.Inscricoes.Where(i => i.IdPessoa == idPessoa).Skip(1);
                if (inscricoes.Count() > 1) {
                    foreach(var ins in inscricoes) {
                        RemoverInscricao(context, atividade, ins, pessoa, inscricoesAtivas);
                    }
                }
                inscricao = inscricoes.FirstOrDefault();
            }
            
            
            atividade = context.Atividades.Find(atividade.Id);

            //Validação:
            var existeInscricao = inscricao != null;
            var dataLimite = DateTime.Now < atividade.DataLimiteCancelamento;
            var cancelamentoPossivel = dataLimite && existeInscricao;

            if (!cancelamentoPossivel) {
                modelState.AddModelError("", "Não foi possível cancelar a inscrição.");
                return String.Empty;
            }
                
            //Lógica:
            RemoverInscricao(context, atividade, inscricao, pessoa, inscricoesAtivas);
            pessoa.CoeficienteSorte = ServicoPessoa.CalcCoeficienteSortePessoa(context, pessoa);
            ServicoPessoa.SetCoeficienteSorteInscricoesPessoa(context, pessoa);
            string textoLog = "A inscrição de " + pessoa.Nome + " (" + pessoa.Matricula + ") em " + atividade.Descricao + " foi cancelada.";
            return ServicoLog.FormatarLog(textoLog);
        }

        private static void RemoverInscricao(SfsContext context, Atividade atividade, Inscricao inscricao, Pessoa pessoa, IEnumerable<Inscricao> inscricoesAtivas) {
            ServicoInscricoes.ElevarInscricao(context, inscricao, inscricoesAtivas.OrderByDescending(i => i.Prioridade).ToList());
            pessoa.Inscricoes.Remove(inscricao);
            atividade.Inscricoes.Remove(inscricao);
            context.Inscricoes.Remove(inscricao);
            context.SaveChanges();
        }

        public static string Inscrever(SfsContext context, Atividade atividade, Guid idPessoa, ModelStateDictionary modelState) {
            //Coleta de dados:
            var pessoa = context.Pessoas.Find(idPessoa);
            var inscricao = new Inscricao { IdAtividade = atividade.Id, Id = Guid.NewGuid(), IdPessoa = idPessoa};
            atividade = context.Atividades.Find(atividade.Id);

            //Validação:
            var existeInscricao = atividade.Inscricoes.Exists(i => i.IdPessoa == idPessoa);
            var pontuacaoSuficiente = atividade.Custo < pessoa.Pontuacao;
            var dataLimite = DateTime.Now < atividade.DataLimiteInscricao;
            var inscricaoPossivel = !existeInscricao && pontuacaoSuficiente && dataLimite;
            if (!inscricaoPossivel) {
                modelState.AddModelError("", "Não foi possível realizar a inscrição.");
                return String.Empty;
            }

            //Lógica:
            context.Inscricoes.Add(inscricao);
            pessoa.CoeficienteSorte = ServicoPessoa.CalcCoeficienteSortePessoa(context, pessoa);
            ServicoPessoa.CalcCoeficienteSortePessoa(context, pessoa);
            var inscricoesAtivas = ServicoInscricoes.GetInscricoesAtivasPessoa(context, pessoa);
            ServicoInscricoes.SetNovaPrioridade(context, inscricao, inscricoesAtivas.ToList());
            string textoLog = pessoa.Nome + " (" + pessoa.Matricula + ")" + " foi inscrito(a) em " + atividade.Descricao + ".";
            ServicoPessoa.SetCoeficienteSorteInscricoesPessoa(context, pessoa);
            return ServicoLog.FormatarLog(textoLog);   
        }

        public static double GetCoeficienteSorteInscricao(Inscricao inscricao) {
            Random rnd = new Random();
            var resultado = inscricao.Fixada ? Double.MaxValue : inscricao.Pessoa.CoeficienteSorte + (rnd.NextDouble() / 10000);
            return resultado;
        }

        public static string InscricoesAcaoLote(SfsContext context, Guid idAtividade, Guid[] ids, AcaoInscricao acao, ModelStateDictionary modelState) {
            var atividade = context.Atividades.Find(idAtividade);
            string logCompleto = "";
            if (ids != null) {
                foreach (var id in ids) {
                    var log = acao(context, atividade, id, modelState);
                    if (!String.IsNullOrEmpty(log)) logCompleto += log;
                }
            }
            context.SaveChanges();
            return logCompleto;
        }
        
        public static void RecalcularCoeficienteSorteInscricoes(SfsContext context, Guid idAtividade) {
            var atividade = context.Atividades.Find(idAtividade);
            Random rnd = new Random();
            foreach (var ins in atividade.Inscricoes) {
                ins.Pessoa.CoeficienteSorte = ServicoPessoa.CalcCoeficienteSortePessoa(context, ins.Pessoa);
                ins.CoeficienteSorte = ins.Fixada ? Double.MaxValue : ins.Pessoa.CoeficienteSorte + (rnd.NextDouble() / 10000);
            }
        }

        //Para uso em caso de exclusão de atividades
        public static void RecalcularCSInscricoesAtivas(SfsContext context) {
            GetAtividadesAtivas(context).Each(a => RecalcularCoeficienteSorteInscricoes(context, a.Id));
            context.SaveChanges();
        }

        public static IEnumerable<Atividade> GetAtividadesAtivas(SfsContext context) {
            var atvs = context.Atividades.Where(a => a.EstadoAtividade.Indice != EstadoAtividade.Arquivada && a.EstadoAtividade.Indice != EstadoAtividade.Cancelada);
            return atvs;
        }

        public static IEnumerable<Atividade> GetAtividadesValidadas(SfsContext context) {
            var atvs = context.Atividades.Where(a => a.EstadoAtividade.Indice == EstadoAtividade.Validada);
            return atvs;
        }

        /// <summary>
        /// Atribui um valor à propriedade "Fixada" da inscrição, definindo sua prioridade (máxima ou normal) na atividade.
        /// </summary>
        /// <param name="context">Uma instância da classe de contexto.</param>
        /// <param name="idPessoas">Uma coleção com as ids das pessoas cujas inscrições serão fixadas.</param>
        /// <param name="idAtividade">A id da atividade na qual a inscrição consta.</param>
        /// <param name="valor">Fixada ou não fixada?</param>
        public static void SetFixadoInscricao(SfsContext context, Guid[] idPessoas, Guid idAtividade, bool valor) {
            foreach (var id in idPessoas) {
                var inscricao = context.Inscricoes.SingleOrDefault(i => i.IdAtividade == idAtividade && i.IdPessoa == id);
                inscricao.Fixada = valor;
                inscricao.CoeficienteSorte = GetCoeficienteSorteInscricao(inscricao);
            }
            context.SaveChanges();
        }

        public static bool CreateOrEdit(SfsContext context, CreateViewModel viewModel) {
            var atividade = viewModel.Atividade;
            atividade.DataHoraInicio = DateTime.Parse(viewModel.DataInicio.ToString("dd/MM/yyyy") + " " + viewModel.HoraInicio);
            atividade.DataHoraFim = DateTime.Parse(viewModel.DataFim.ToString("dd/MM/yyyy") + " " + viewModel.HoraFim);
            atividade.DataLimiteInscricao = DateTime.Parse(viewModel.DataLimiteInscricao.ToString("dd/MM/yyyy") + " " + viewModel.HoraLimiteInscricao);
            atividade.DataLimiteCancelamento = DateTime.Parse(viewModel.DataLimiteCancelamento.ToString("dd/MM/yyyy") + " " + viewModel.HoraLimiteCancelamento);
            atividade.IdEstadoAtividade = viewModel.Atividade.IdEstadoAtividade;
            atividade.EstadoAtividade = context.EstadosAtividade.Find(atividade.IdEstadoAtividade);
            if (atividade.Id != Guid.Empty) {
                context.Entry(atividade).State = EntityState.Modified;
            }
            else {
                atividade.Id = Guid.NewGuid();
                context.Atividades.Add(atividade);
            }
            context.SaveChanges();
            switch (atividade.EstadoAtividade.Indice) {
                case EstadoAtividade.Validada:
                    ValidarAtividade(context, atividade);
                    ConfirmarLista(context, atividade.Id);
                    break;
                case EstadoAtividade.Arquivada:
                    ArquivarAtividade(context, atividade.Id);
                    break;
            }
            return true;
        }

        private static void ConfirmarLista(SfsContext context, Guid idAtividade) {
            var atividade = context.Atividades.Include(a => a.Inscricoes).Single(a => a.Id.Equals(idAtividade));
            foreach (var ins in atividade.Inscricoes) {
                string texto = ins.Pessoa.Nome + ",<br/>" +
                    "Sua inscrição em " + atividade.Descricao + " foi confirmada. A sua presença é requerida no ponto de encontro em " + atividade.DataHoraInicio + ".<br/>" +
                    "Por favor evite atrasos, a chegada antes do horário é apreciada.<br/>" +
                    "Não esqueça dos documentos e do crachá. Um bom passeio!<br/>" +
                    "Atenciosamente, Equipe de Final de Semana.";
                ServicoMensageiro.EnviarMensagem(context, ins.Pessoa, "Sua inscrição em " + atividade.Descricao + " foi confirmada.", "Sistema", texto);
            }
            context.SaveChanges();
        }

        public static void ValidarAtividade(SfsContext context, Atividade atividade) {
            //TODO: Validar atividade e fazer correções que garantam a consistência da mesma.
            var inscricoesCanceladas = new List<Pessoa>();
            foreach (var ins in atividade.Inscricoes) {
                string motivo;
                if (!ServicoInscricoes.ConfirmarInscricao(context, ins, out motivo)) {
                    var log = ServicoLog.FormatarLog(CancelarInscricao(context, atividade, ins.Id, null) +" Motivo: " + motivo);
                    ServicoLog.Log(context, "Inscrições", log);
                    ServicoMensageiro.EnviarMensagem(
                        context,
                        ins.Pessoa,
                        "A sua inscrição em uma atividade foi cancelada.",
                        "O Sistema",
                        "A sua inscrição em " + atividade.Descricao + " foi cancelada. <br/>" +
                        "Motivo: " + motivo + " <br/>" +
                        "A inscrição de maior prioridade foi mantida. Pedimos desculpas pelo transtorno. <br/>" +
                        "Em caso de dúvidas, contate a Equipe de Final de Semana.");
                    inscricoesCanceladas.Add(ins.Pessoa);
                }
            }
            string nomesCancelados = "";
            foreach (var pessoa in inscricoesCanceladas) {
                nomesCancelados += pessoa.Nome + "<br/>";
            }
            if (inscricoesCanceladas.Any()) {
                ServicoMensageiro.EnviarMensagem(
                        context,
                        GetAdministrador(context),
                        "Algumas inscrições foram automaticamente canceladas.",
                        "O Sistema",
                        "As seguintes inscrições foram canceladas por conflito de horário ou falta de pontos:<br/>" +
                        nomesCancelados +
                        "Para mais informações, cheque o log de Atividades."
                        );
            }
            var inscricoes = atividade.Inscricoes.OrderByDescending(i => i.CoeficienteSorte);
            var confirmados = inscricoes.Take(atividade.NumeroVagas);
       
            foreach (var ins in confirmados) {
                ins.Pessoa.Pontuacao -= atividade.Custo;
            }
            //Parte de validação a seguir
            context.SaveChanges();
        }

       
        public static void ArquivarAtividade(SfsContext context, Guid idAtividade) {
            var atividade = context.Atividades.Include(a => a.Inscricoes).Single(a => a.Id == idAtividade);
            var listaInscricoes = atividade.Inscricoes.OrderByDescending(i => i.CoeficienteSorte);
            var listaConfirmados = listaInscricoes.Take(atividade.NumeroVagas);
            var listaEspera = listaInscricoes.Skip(atividade.NumeroVagas);
            foreach (var ins in listaConfirmados) {
                ins.Validada = true;
            }
            /*foreach (var ins in listaEspera.Where(i => i.Presente)) {
                ins.Validada = true;
            }*/
            context.SaveChanges();
        }
    }
}