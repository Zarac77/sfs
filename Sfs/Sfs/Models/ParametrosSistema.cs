using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sfs.Models
{
    public class ParametrosSistema
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [DisplayName("Pontos Iniciais")]
        [Range(1, Double.MaxValue)]
        public int PontuacaoInicialAlunos { get; set; }
    }
}