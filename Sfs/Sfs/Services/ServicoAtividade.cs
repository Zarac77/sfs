using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;

namespace Sfs.Services
{
    public class ServicoAtividade
    {
        public static void CancelarInscricao(SfsContext context, Atividade atividade, Pessoa pessoa)
        {
            atividade = context.Atividades.Find(atividade.Id);
            var inscricao = atividade.Inscricoes.Single(i => i.Pessoa.Id == pessoa.Id);
            context.Inscricoes.Remove(inscricao);
            context.SaveChanges();
        }
    }
}