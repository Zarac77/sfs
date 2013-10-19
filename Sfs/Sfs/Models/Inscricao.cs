using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfs.Models
{
    /// <summary>
    /// Representa uma inscrição de uma pessoa em uma atividade.
    /// </summary>
    public class Inscricao
    {
        #region Constantes
        #endregion

        #region Atributos e propriedades
            
        [Key]
        public Guid Id { get; set; }

        /*[Range(1, 10)]
        [Required]
        public int Prioridade { get; set; }
         * 
         * Necessita de reformulação: a vinculação entre o inscrito e sua prioridade é necessária;
         */

        [Required]
        public bool Fixada { get; set; }

        /// <summary>
        /// Indica se a pessoa esteve presente ou não na atividade.
        /// </summary>
        public bool Presente { get; set; }

        [ForeignKey("Pessoa")]
        public Guid IdPessoa { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        [ForeignKey("Atividade")]
        public Guid IdAtividade { get; set; }
        public virtual Atividade Atividade { get; set; }

        [Required]
        public double CoeficienteSorte { get; set; }

        /// <summary>
        /// Indica se a inscrição foi concretizada (com ou sem presença).
        /// </summary>
        public bool Validada { get; set; }

        public int Prioridade { get; set; }

        #endregion

        #region Métodos privados
        #endregion

        #region Métodos protegidos
        #endregion

        #region Métodos públicos
        #endregion

        #region Construtores

        public Inscricao()
        {
            Fixada = false;
            Presente = false;
            CoeficienteSorte = 0;
            Validada = false;
        }

        #endregion
    }
}