using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;

namespace Sfs.Services
{
    public class ServicoMensageiro : Servico
    {
        public static bool EnviarMensagem(SfsContext context, Pessoa pessoa, string assunto, string remetente, string texto)
        {
            try
            {
                Mensagem msg = new Mensagem
                {
                    Id = Guid.NewGuid(),
                    Assunto = assunto,
                    Remetente = remetente,
                    Texto = texto
                };
                var inbox = context.Inboxes.Single(i => i.IdPessoa == pessoa.Id);
                inbox.Mensagens.Add(msg);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

    }
}