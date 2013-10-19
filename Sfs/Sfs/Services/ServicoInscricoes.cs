using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Sfs.Services {
    public class ServicoInscricoes {
        public static IEnumerable<Inscricao> GetInscricoesAtivasPessoa(SfsContext context, Pessoa pessoa) {
            var inscricoes = context.Inscricoes.Where(i => i.Pessoa.Id == pessoa.Id && (!i.Validada && i.Atividade.DataLimiteCancelamento > DateTime.Now)).ToList();
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
    }
}