using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Sfs.Services {
    public class ServicoInscricoes {
        public static IEnumerable<Inscricao> GetInscricoesAtivasPessoa(SfsContext context, Pessoa pessoa) {
            var inscricoes = context.Inscricoes.Where(i => i.Pessoa.Id == pessoa.Id && !i.Validada).ToList();
            inscricoes.Each(i => i.Pessoa = context.Pessoas.Find(i.IdPessoa));
            return inscricoes;
        }

        public static void SetNovaPrioridade(SfsContext context, Inscricao inscricao, List<Inscricao> listaCompleta) {
            inscricao = context.Inscricoes.Find(inscricao.Id);
            if (listaCompleta.Count > 1) {
                inscricao.Prioridade = listaCompleta.Count - 1;
                for(int i = 0; i < listaCompleta.Count - 1; i++) {
                    DescerPrioridade(context, inscricao, listaCompleta, false);
                }
            }
            else {
                inscricao.Prioridade = 0;
            }

            context.SaveChanges();
        }

        public static void DescerPrioridade(SfsContext context, Inscricao inscricao, List<Inscricao> listaCompleta, bool saveChanges) {
            listaCompleta = listaCompleta.OrderByDescending(i => i.Prioridade).ToList();
            inscricao = context.Inscricoes.Find(inscricao.Id);
            int indexVizinha = listaCompleta.IndexOf(inscricao) + 1;
            int prioridadeAntiga = inscricao.Prioridade;
            if (indexVizinha < listaCompleta.Count) {
                inscricao.Prioridade = listaCompleta[indexVizinha].Prioridade;
                listaCompleta[indexVizinha].Prioridade = prioridadeAntiga;
            }
            if (saveChanges) context.SaveChanges();
        }

        public static void SubirPrioridade(SfsContext context, Inscricao inscricao, List<Inscricao> listaCompleta, bool saveChanges) {
            listaCompleta = listaCompleta.OrderByDescending(i => i.Prioridade).ToList();
            inscricao = context.Inscricoes.Find(inscricao.Id);
            int indexVizinha = listaCompleta.IndexOf(inscricao) - 1;
            int prioridadeAntiga = inscricao.Prioridade;
            if (indexVizinha >= 0) {
                inscricao.Prioridade = listaCompleta[indexVizinha].Prioridade;
                listaCompleta[indexVizinha].Prioridade = prioridadeAntiga;
            }
            if (saveChanges) context.SaveChanges();
        }

        /// <summary>
        /// Usado logo antes de remover uma inscrição, evita espaços vazios na prioridade.
        /// </summary>
        public static void ElevarInscricao(SfsContext context, Inscricao inscricao, List<Inscricao> listaCompleta) {
            for(int i = 0; i < listaCompleta.Count - 1; i++) {
                SubirPrioridade(context, inscricao, listaCompleta, false);
            }
            context.SaveChanges();
        }

        public static bool ConfirmarInscricao(SfsContext context, Inscricao inscricao, out string motivo) {
            var conflitosHorario = GetInscricoesConflitoHorario(context, inscricao);
            var inscricaoPrioritaria = GetInscricaoPrioritaria(conflitosHorario);
            if(conflitosHorario.Count > 1) {
                motivo = "Conflito de horário com as atividades ";
                foreach (var ins in conflitosHorario) {
                    motivo += ins.Atividade.Descricao + ";";
                }
            }
            motivo = "";
            return inscricaoPrioritaria.Equals(inscricao);
        }
        private static List<Inscricao> GetInscricoesConflitoHorario(SfsContext context, Inscricao inscricao) {
            //Coleta de dados:
            var pessoa = context.Pessoas.Find(inscricao.IdPessoa);
            var inscricoes = GetInscricoesAtivasPessoa(context, pessoa);
            var tempoIntervalo = context.ParametrosSistema.FirstOrDefault().TempoEntreAtividades;
            
            //Inicialização:
            var listaConflitos = new List<Inscricao> { inscricao }; //A inscrição a ser validada deve ser sempre a primeira.

            //Validação:
            foreach (var ins in inscricoes.Where(i => i != inscricao)) {
                bool conflito;
                if (ins.Atividade.DataHoraInicio > inscricao.Atividade.DataHoraInicio) {
                    conflito = (ins.Atividade.DataHoraInicio - inscricao.Atividade.DataHoraFim).Minutes < tempoIntervalo;
                }
                else {
                    conflito = (inscricao.Atividade.DataHoraInicio - ins.Atividade.DataHoraFim).Minutes < tempoIntervalo;
                }
                if (conflito) {
                    listaConflitos.Add(ins);
                }
            }
            return listaConflitos;
        }

        private static Inscricao GetInscricaoPrioritaria(List<Inscricao> conflitos) {
            conflitos.Sort((i1, i2) => i1.Prioridade.CompareTo(i2.Prioridade));
            return conflitos.First();
        }
    }
}