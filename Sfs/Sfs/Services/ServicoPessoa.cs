using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;

namespace Sfs.Services {
    public class ServicoPessoa {
        public static int GetNumeroSaidas(SfsContext context, Pessoa pessoa) {
            pessoa = context.Pessoas.Find(pessoa.Id);
            var viradaSemestre = context.ParametrosSistema.FirstOrDefault().ViradaSemestre;
            var semestre = DateTime.Today < viradaSemestre ? 1 : 2;
            var inscricoes = pessoa.Inscricoes.Where(i => i.Atividade.DataHoraInicio.Year == DateTime.Now.Year);
            if (semestre == 1) {
                inscricoes = inscricoes.Where(i => i.Atividade.DataHoraInicio < viradaSemestre);
            }
            else {
                inscricoes = inscricoes.Where(i => i.Atividade.DataHoraInicio >= viradaSemestre);
            }
            inscricoes = inscricoes.Where(i => i.Validada);
            return inscricoes.Count();
        }

        public static double CalcCoeficienteSorte(SfsContext context, Pessoa pessoa) {
            var atividadesAtivas = ServicoAtividade.GetAtividadesAtivas(context);
            int numeroAtividadadesAtivas = atividadesAtivas.Count();
            int numeroInscricoesAtivas = atividadesAtivas.Where(a => a.Inscricoes.Exists(i => i.Pessoa.Id == pessoa.Id)).Count();
            int numeroInscricoesValidadas = ServicoPessoa.GetNumeroSaidas(context, pessoa);
            double cfs = (1 + (numeroAtividadadesAtivas - numeroInscricoesAtivas)) / (numeroInscricoesValidadas + 1);
            return cfs;
        }

        public static IEnumerable<Pessoa> BuscarPessoas(SfsContext context, Pessoa modelo) {
            if (!String.IsNullOrEmpty(modelo.Matricula)) {
                return new List<Pessoa> { context.Pessoas.Single(p => p.Matricula == modelo.Matricula) };
            }
            var pessoas = context.Pessoas.ToList();
            var info = typeof(Pessoa).GetProperties();
            foreach (var p in info) {
                var value = p.GetValue(modelo);
                var type = value != null ? value.GetType() : null;
                bool isTipoValido = type == typeof(string) || type == typeof(bool);
                if ((isTipoValido && type == typeof(string)) && !String.IsNullOrEmpty(p.GetValue(modelo).ToString())) {
                    pessoas = pessoas.Where(pessoa => p.GetValue(pessoa) != null && p.GetValue(pessoa).ToString().Contains(p.GetValue(modelo).ToString())).ToList();
                }
                else if (isTipoValido) {
                    pessoas = pessoas.Where(pessoa => p.GetValue(modelo).Equals(p.GetValue(pessoa))).ToList();
                }
            }

            return pessoas;
        }
    }
}