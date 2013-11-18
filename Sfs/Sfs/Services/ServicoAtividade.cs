using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;
using System.Data;
using System.Data.Entity;
using AutoMapper;
using Sfs.ViewModels.AtividadeViewModels;

namespace Sfs.Services
{
    public class ServicoAtividade : Servico
    {
        public static void CancelarInscricao(SfsContext context, Guid idAtividade, Guid idPessoa)
        {
            var atividade = context.Atividades.Find(idAtividade);
            var pessoa = context.Pessoas.Find(idPessoa);
            if (atividade.Inscricoes.Exists(i => i.IdPessoa == idPessoa)) {
                var inscricao = atividade.Inscricoes.Single(i => i.Pessoa.Id == pessoa.Id);
                var inscricoesAtivas = ServicoInscricoes.GetInscricoesAtivasPessoa(context, pessoa);
                ServicoInscricoes.ElevarInscricao(context, inscricao, inscricoesAtivas.OrderByDescending(i => i.Prioridade).ToList());
                pessoa.Inscricoes.Remove(inscricao);
                atividade.Inscricoes.Remove(inscricao);
                context.Inscricoes.Remove(inscricao);
                context.SaveChanges();
                pessoa.CoeficienteSorte = ServicoPessoa.CalcCoeficienteSorte(context, pessoa);
                context.SaveChanges();
                //RecalcularCoeficienteSorteInscricoes(context, atividade.Id);
                string textoLog = "A inscrição de " + pessoa.Nome + "(" + pessoa.Matricula + ")" + " em " + atividade.Descricao + " foi cancelada.";
                ServicoLog.Log(context, "Inscrições", textoLog);
            }
        }

        public static void Inscrever(SfsContext context, Atividade atividade, Guid idPessoa) {
            var pessoa = context.Pessoas.Find(idPessoa);
            var inscricao = new Inscricao { IdAtividade = atividade.Id, Id = Guid.NewGuid(), IdPessoa = idPessoa};
            atividade = context.Atividades.Find(atividade.Id);
            if (!atividade.Inscricoes.Exists(i => i.IdPessoa == idPessoa)) {
                context.Inscricoes.Add(inscricao);
                context.SaveChanges();
                pessoa = context.Pessoas.Find(idPessoa);
                pessoa.CoeficienteSorte = ServicoPessoa.CalcCoeficienteSorte(context, pessoa);
                context.SaveChanges();
                RecalcularCoeficienteSorteInscricoes(context, atividade.Id);
                var inscricoesAtivas = ServicoInscricoes.GetInscricoesAtivasPessoa(context, pessoa);
                ServicoInscricoes.SetNovaPrioridade(context, inscricao, inscricoesAtivas.ToList());
                string textoLog = pessoa.Nome + "(" + pessoa.Matricula + ")" + " foi inscrito(a) em " + atividade.Descricao + ".";
                ServicoLog.Log(context, "Inscrições", textoLog);
            }

            
        }

        public static double GetCoeficienteSorteAtividade(SfsContext context, Atividade atividade) {
            double coef = 0;
            foreach (var i in atividade.Inscricoes) {
                coef += i.Pessoa.CoeficienteSorte;
            }
            return coef;
        }
        
        public static void RecalcularCoeficienteSorteInscricoes(SfsContext context, Guid idAtividade) {
            var atividade = context.Atividades.Find(idAtividade);
            //double coefAtv = GetCoeficienteSorteAtividade(context, atividade);
            Random rnd = new Random();
            foreach (var ins in atividade.Inscricoes) {
                ins.Pessoa.CoeficienteSorte = ServicoPessoa.CalcCoeficienteSorte(context, ins.Pessoa);
                ins.CoeficienteSorte = ins.Fixada ? Double.MaxValue : ins.Pessoa.CoeficienteSorte + (rnd.NextDouble() / 10000);
            }
        }

        public static void RecalcularCSInscricoesAtivas(SfsContext context) {
            GetAtividadesAtivas(context).Each(a => RecalcularCoeficienteSorteInscricoes(context, a.Id));
            context.SaveChanges();
        }

        public static IEnumerable<Atividade> GetAtividadesAtivas(SfsContext context) {
            //Critério temporário, pode vir a mudar.
            var atvs = context.Atividades.Where(a => !a.Validada && !a.Cancelada);
            return atvs;
        }

        public static void FixarInscricao(SfsContext context, Guid idInscricao) {
            var inscricao = context.Inscricoes.Find(idInscricao);
            inscricao.Fixada = true;
            context.SaveChanges();
            RecalcularCoeficienteSorteInscricoes(context, inscricao.Atividade.Id);
        }

        public static void DesafixarInscricao(SfsContext context, Guid idInscricao) {
            var inscricao = context.Inscricoes.Find(idInscricao);
            inscricao.Fixada = false;
            context.SaveChanges();
            RecalcularCoeficienteSorteInscricoes(context, inscricao.Atividade.Id);
        }

        public static bool CreateOrEdit(SfsContext context, CreateViewModel viewModel) {
            bool resultado;
            var atividade = viewModel.Atividade;
            DateTime dataHoraInicio;
            DateTime dataHoraFim;
            resultado = DateTime.TryParse(viewModel.DataInicio.ToString("dd/MM/yyyy") + " " + viewModel.HoraInicio, out dataHoraInicio);
            resultado = DateTime.TryParse(viewModel.DataFim.ToString("dd/MM/yyyy") + " " + viewModel.HoraFim, out dataHoraFim);
            atividade.DataHoraInicio = dataHoraInicio;
            atividade.DataHoraFim = dataHoraFim;
            if (viewModel.Atividade.Id != Guid.Empty) {
                context.Entry(atividade).State = EntityState.Modified;
            }
            else {
                atividade.Id = Guid.NewGuid();
                context.Atividades.Add(atividade);
            }
            RecalcularCSInscricoesAtivas(context);
            if (resultado) {
                context.SaveChanges();
            }
            else return resultado;
            return true;
        }

        public static CreateViewModel CreateViewModel(SfsContext context, Guid id) {
            var atividade = context.Atividades.Find(id);
            var cvm = new CreateViewModel {
                Atividade = atividade,
                IdAtividade = atividade.Id,
                DataInicio = DateTime.Parse(atividade.DataHoraInicio.ToString("dd/MM/yyyy")),
                HoraInicio = atividade.DataHoraInicio.TimeOfDay.ToString("c"),
                DataFim = DateTime.Parse(atividade.DataHoraFim.ToString("dd/MM/yyyy")),
                HoraFim = atividade.DataHoraFim.TimeOfDay.ToString("c"),
                DataLimiteCancelamento = DateTime.Parse(atividade.DataLimiteCancelamento.ToString("dd/MM/yyyy")),
                HoraLimiteCancelamento = atividade.DataLimiteCancelamento.TimeOfDay.ToString("c"),
                DataLimiteInscricao = DateTime.Parse(atividade.DataLimiteInscricao.ToString("dd/MM/yyyy")),
                HoraLimiteInscricao = atividade.DataLimiteInscricao.TimeOfDay.ToString("c")
            };
            return cvm;
        }   

        public static void ValidarAtividade(SfsContext context, Guid idAtividade) {
            //TODO: Validar atividade e fazer correções que garantam a consistência da mesma.
        }
    }
}