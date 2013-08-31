using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sfs.Models
{
    public class Mensagem
    {
        [Key]
        public Guid Id { get; set; }
        
        [StringLength(1000)]
        [Required]
        public string Texto { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Remetente { get; set; }

        [StringLength(250)]
        public string Assunto { get; set; }

        [Required]
        public bool Lida { get; set; }

        [Required]
        public DateTime DataEnvio { get; set; }

        [Required]
        public bool Deletar { get; set; }

        public Mensagem()
        {
            DataEnvio = DateTime.Now;
            Lida = false;
            Deletar = false;
        }
    }
}