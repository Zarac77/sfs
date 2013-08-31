using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sfs.Models
{
    /// <summary>
    /// Caixa de Entrada para sistema de mensagens interno.
    /// </summary>
    public class Inbox
    {
        [Key]
        public Guid Id { get; set; }
        public virtual List<Mensagem> Mensagens { get; set; }
        [ForeignKey("Pessoa")]
        public Guid IdPessoa { get; set; }
        public Pessoa Pessoa { get; set; }

        public Inbox()
        {
            Mensagens = new List<Mensagem>();
        }
    }
}