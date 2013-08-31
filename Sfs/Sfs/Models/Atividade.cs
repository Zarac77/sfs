using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sfs.Models
{
    public class Atividade
    {
        #region Constantes
        #endregion

        #region Atributos e propriedades

        [Key]
        public Guid Id { get; set; }

        [DisplayName("Descrição")]
        [MaxLength(256)]
        [Required]
        public string Descricao { get; set; }

        [DisplayName("Data e hora de início")]
        public DateTime DataHoraInicio { get; set; }

        [DisplayName("Data e hora de término")]
        public DateTime DataHoraFim { get; set; }

        [DisplayName("Data e hora limites para inscrições")]
        public DateTime DataLimiteInscricao { get; set; }

        [DisplayName("Data e hora limites para cancelamento")]
        public DateTime DataLimiteCancelamento { get; set; }

        [DisplayName("Número de vagas")]
        [Range(1, 500)]
        [Required]
        public int NumeroVagas { get; set; }

        /// <summary>
        /// Indica se a atividade está aprovada para inscrições.
        /// </summary>
        public bool Aprovada { get; set; } //Sujeita a deprecação. Conforme conversado com a coordenadora de final de semana.

        public virtual List<Inscricao> Inscricoes { get; set; }

        #endregion

        #region Métodos privados
        #endregion

        #region Métodos protegidos
        #endregion

        #region Métodos públicos
        #endregion

        #region Construtores

        public Atividade()
        {
            Aprovada = false;
            Inscricoes = new List<Inscricao>();
        }

        #endregion
    }
}