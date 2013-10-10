using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;
using System.Data;

namespace Sfs.Services
{
    public class ServicoAtividade : Servico
    {
        public static void CancelarInscricao(SfsContext context, Guid IdAtividade, Guid IdPessoa)
        {
            var atividade = context.Atividades.Find(IdAtividade);
            var pessoa = context.Pessoas.Find(IdPessoa);
            var inscricao = atividade.Inscricoes.Single(i => i.Pessoa.Id == pessoa.Id);
            pessoa.Inscricoes.Remove(inscricao);
            atividade.Inscricoes.Remove(inscricao);
            
            context.Entry(pessoa).State = EntityState.Modified;
            context.Entry(inscricao).State = EntityState.Modified;
            context.Inscricoes.Remove(inscricao);

            pessoa.CoeficienteSorte = ServicoPessoa.CalcCoeficienteSorte(context, pessoa);
            RecalcularCoeficienteSorteInscricoes(context, atividade);
            context.SaveChanges();
        }

        public static void Inscrever(SfsContext context, Atividade atividade, Guid IdPessoa) {
            var pessoa = context.Pessoas.Find(IdPessoa);
            var inscricao = new Inscricao { IdAtividade = atividade.Id, Id = Guid.NewGuid(), IdPessoa = IdPessoa};
            context.Inscricoes.Add(inscricao);
            context.SaveChanges();
            pessoa = context.Pessoas.Find(IdPessoa);
            pessoa.CoeficienteSorte = ServicoPessoa.CalcCoeficienteSorte(context, pessoa);
            context.SaveChanges();
            RecalcularCoeficienteSorteInscricoes(context, atividade);
        }

        public static double GetCoeficienteSorteAtividade(SfsContext context, Atividade atividade) {
            double coef = 0;
            foreach (var i in atividade.Inscricoes) {
                coef += i.Pessoa.CoeficienteSorte;
            }
            return coef;
        }
        
        public static void RecalcularCoeficienteSorteInscricoes(SfsContext context, Atividade atividade) {
            atividade = context.Atividades.Find(atividade.Id);
            double coefAtv = GetCoeficienteSorteAtividade(context, atividade);
            Random rnd = new Random();
            foreach (var ins in atividade.Inscricoes) {
                ins.CoeficienteSorte = ins.Fixada ? 1.0 : (ins.Pessoa.CoeficienteSorte / coefAtv) + (rnd.NextDouble() / 10000);
            }

            context.SaveChanges();
        }

        public static IEnumerable<Atividade> GetAtividadesAtivas(SfsContext context) {
            //Critério temporário, pode vir a mudar.
            var atvs = context.Atividades.Where(a => !a.Validada && !a.Cancelada);
            return atvs;
        }
    }
}