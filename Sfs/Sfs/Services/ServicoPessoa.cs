using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;
using System.Text.RegularExpressions;

namespace Sfs.Services {
    public class ServicoPessoa : Servico {
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

        public static double CalcCoeficienteSortePessoa(SfsContext context, Pessoa pessoa) {
            var atividadesAtivas = ServicoAtividade.GetAtividadesAtivas(context);
            int numeroAtividadadesAtivas = atividadesAtivas.Count();
            int numeroInscricoesAtivas = atividadesAtivas.Where(a => a.Inscricoes.Exists(i => i.Pessoa.Id == pessoa.Id)).Count();
            int numeroInscricoesValidadas = ServicoPessoa.GetNumeroSaidas(context, pessoa);
            double cfs = (1 + (numeroAtividadadesAtivas - numeroInscricoesAtivas)) / (numeroInscricoesValidadas + 1);
            return cfs;
        }

        public static void SetCoeficienteSorteInscricoesPessoa(SfsContext context, Pessoa pessoa) {
            pessoa = context.Pessoas.Find(pessoa.Id);
            var inscricoes = ServicoInscricoes.GetInscricoesAtivasPessoa(context, pessoa);
            foreach (var i in inscricoes) {
                i.CoeficienteSorte = ServicoAtividade.GetCoeficienteSorteInscricao(i);
            }
        }

        public static IEnumerable<Pessoa> BuscarPessoas(SfsContext context, Pessoa modelo) {
            var lista = FiltrarLista<Pessoa>(context, modelo);
            return lista;
        }

        /// <summary>
        /// Faz as alterações necessárias para que uma pessoa possa ser devidamente registrada no sistema.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pessoa"></param>
        public static void RegistrarPessoa(SfsContext context, Pessoa pessoa) {
            pessoa.Id = Guid.NewGuid();
            pessoa.Senha = ServicoControleAcesso.HashSenha(pessoa.Email, pessoa.PreSenha);
            context.Pessoas.Add(pessoa);
            context.Inboxes.Add(new Inbox { Id = Guid.NewGuid(), IdPessoa = pessoa.Id, Pessoa = pessoa });
        }

        /// <summary>
        /// Faz as alterações necessárias para que uma pessoa possa ser devidamente registrada no sistema.
        /// </summary>
        /// <param name="context">A classe de contexto onde o novo usuário será incluído.</param>
        /// <param name="pessoa">A instância de Pessoa que contem os dados de registro.</param>
        /// <param name="saveChanges">As alterações devem ser salvas no banco?</param>
        public static void RegistrarPessoa(SfsContext context, Pessoa pessoa, bool saveChanges) {
            pessoa.Id = Guid.NewGuid();
            pessoa.Senha = ServicoControleAcesso.HashSenha(pessoa.Email, pessoa.PreSenha);
            context.Pessoas.Add(pessoa);
            context.Inboxes.Add(new Inbox { Id = Guid.NewGuid(), IdPessoa = pessoa.Id, Pessoa = pessoa });
            if (saveChanges) context.SaveChanges();
        }
    }
}