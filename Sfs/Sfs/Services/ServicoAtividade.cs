using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;
using System.Data;
using System.Data.Entity;
using AutoMapper;

namespace Sfs.Services
{
    public class ServicoAtividade : Servico
    {
        public static void CancelarInscricao(SfsContext context, Guid idAtividade, Guid IdPessoa)
        {
            var atividade = context.Atividades.Find(idAtividade);
            var pessoa = context.Pessoas.Find(IdPessoa);
            var inscricao = atividade.Inscricoes.Single(i => i.Pessoa.Id == pessoa.Id);
            var inscricoesAtivas = ServicoInscricoes.GetInscricoesAtivasPessoa(context, pessoa);
            ServicoInscricoes.ElevarInscricao(context, inscricao, inscricoesAtivas.OrderByDescending(i => i.Prioridade).ToList());
            pessoa.Inscricoes.Remove(inscricao);
            atividade.Inscricoes.Remove(inscricao);
            context.Inscricoes.Remove(inscricao);
            context.SaveChanges();
            pessoa.CoeficienteSorte = ServicoPessoa.CalcCoeficienteSorte(context, pessoa);
            context.SaveChanges();
            RecalcularCoeficienteSorteInscricoes(context, atividade.Id);
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
            double coefAtv = GetCoeficienteSorteAtividade(context, atividade);
            Random rnd = new Random();
            foreach (var ins in atividade.Inscricoes) {
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
    }
}