﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// Indica se a atividade já está finalizada e pronta para controle.
        /// </summary>
        //public bool Validada { get; set; }


        public bool Aberta { get; set; }

        public virtual List<Inscricao> Inscricoes { get; set; }

        [Required]
        public int Custo { get; set; }

        [DisplayName("Estado da atividade")]
        [ForeignKey("EstadoAtividade")]
        public Guid IdEstadoAtividade { get; set; }

        public virtual EstadoAtividade EstadoAtividade { get; set; }

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
            Inscricoes = new List<Inscricao>();
            Aberta = true;
            Custo = 10;
        }

        #endregion
    }

    public class EstadoAtividade {
        public const int Edicao = 0;
        public const int Aberta = 1;
        public const int Validada = 2;
        public const int Arquivada = 3;
        public const int Cancelada = 4;

        [Key]
        public Guid Id { get; set; }

        public int Indice { get; set; }
        public string Descricao { get; set; }

    }
}