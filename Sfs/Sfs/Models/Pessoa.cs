using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace Sfs.Models
{
    public class Pessoa
    {
        #region Constantes
        #endregion

        #region Atributos e propriedades

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(20)]
        public string Matricula { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100)]
        public string Senha { get; set; }

        [Required]
        public bool Ativo { get; set; }

        public string Turma { get; set; }

        public virtual List<Inscricao> Inscricoes { get; set; }

        public virtual List<Perfil> Perfis { get; set; }

        [NotMapped]
        public bool IsAdministrador { get { return Perfis.Where(p => p.Id == Perfil.GUID_PERFIL_ADMINISTRADOR).Any(); } }

        [Range(1, Double.MaxValue)]
        [Required]
        public int Pontuacao { get; set; }

        #endregion

        #region Métodos privados
        #endregion

        #region Métodos protegidos
        #endregion

        #region Métodos públicos
        #endregion

        #region Construtores

        public Pessoa()
        {
            Inscricoes = new List<Inscricao>();
            Perfis = new List<Perfil>();
            Ativo = true;
            Pontuacao = 1;
        }

        #endregion
    }
}