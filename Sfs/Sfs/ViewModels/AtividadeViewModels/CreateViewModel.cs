using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sfs.ViewModels.AtividadeViewModels
{
    public class CreateViewModel
    {
        #region Atributos e propriedades

        [Key]
        public Guid Id { get; set; }

        [DisplayName("Descrição")]
        [MaxLength(256)]
        [Required]
        public string Descricao { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data de início")]
        public DateTime? DataInicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data de término")]
        public DateTime? DataFim { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("Hora de início")]
        public DateTime HoraInicio { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("Hora de término")]
        public DateTime HoraFim { get; set; }

        [DisplayName("Data e hora limites para inscrições")]
        public DateTime DataLimiteInscricao { get; set; }

        [DisplayName("Data e hora limites para cancelamento")]
        public DateTime DataLimiteCancelamento { get; set; }

        [DisplayName("Número de vagas")]
        [Range(1, 500)]
        [Required]
        public int NumeroVagas { get; set; }

        #endregion
    }
}