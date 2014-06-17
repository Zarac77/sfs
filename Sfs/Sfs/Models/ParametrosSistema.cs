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
        [Range(0, Double.MaxValue)]
        public int PontuacaoInicialAlunos { get; set; }

        public DateTime ViradaSemestre { get; set; }

        [DisplayName("Tempo entre atividades, em minutos")]
        public int TempoEntreAtividades { get; set; }

        public ParametrosSistema() {
            ViradaSemestre = new DateTime(2013, 7, 1);
            TempoEntreAtividades = 60;
        }
    }
}