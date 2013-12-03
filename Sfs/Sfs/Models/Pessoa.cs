using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace Sfs.Models
{
    /// <summary>
    /// Representa um usuário no sistema.
    /// </summary>
    public class Pessoa
    {
        #region Constantes
        #endregion

        #region Atributos e propriedades

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// O nome real da pessoa.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        /// <summary>
        /// O número de registro da pessoa na escola. É diferente para alunos e funcionários.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Matricula { get; set; }

        /// <summary>
        /// O e-mail que é usado como nome de usuário.
        /// </summary>
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// A representação binária da senha, usa SHA1 para armezenamento seguro.
        /// </summary>
        [DataType(DataType.Password)]
        public byte[] Senha { get; set; }

        /// <summary>
        /// A representação da senha em forma de texto, usada para registro de novas pessoas.
        /// </summary>
        [DataType(DataType.Password)]
        [NotMapped]
        public string PreSenha { get; set; }

        /// <summary>
        /// A pessoa ainda faz parte da instituição?
        /// </summary>
        [Required]
        public bool Ativo { get; set; }

        /// <summary>
        /// Para alunos, vai de A a K.
        /// </summary>
        public string Turma { get; set; }

        /// <summary>
        /// Ano em que a pessoa passou a participar da instituição.
        /// </summary>
        public int? AnoEntrada { get; set; }

        /// <summary>
        /// As inscrições desde que a pessoa esteve ativa.
        /// </summary>
        public virtual List<Inscricao> Inscricoes { get; set; }

        /// <summary>
        /// Perfis associados a esse usuário. Define quais áreas do site ele pode acessar.
        /// </summary>
        public virtual List<Perfil> Perfis { get; set; }

        /// <summary>
        /// Esse usuário é um administrador do sistema? Interfere nas ações que ele pode realizar.
        /// </summary>
        [NotMapped]
        public bool IsAdministrador { get { return Perfis.Where(p => p.Id == Perfil.GUID_PERFIL_ADMINISTRADOR).Any(); } }

        /// <summary>
        /// Esse usuário realize controle nas atividades? Se sim, ele terá acesso às ferramentas de confirmação de presença.
        /// </summary>
        [NotMapped]
        public bool IsControle { get { return Perfis.Where(p => p.Id == Perfil.GUID_PERFIL_CONTROLE).Any(); } }

        /// <summary>
        /// Os pontos de que o usuário dispõe para inscrições.
        /// </summary>
        [Range(0, int.MaxValue)]
        [Required]
        public int Pontuacao { get; set; }

        /// <summary>
        /// Caso informado, guarda o telefone da pessoa. É utilizado para consulta na listagem de atividades.
        /// </summary>
        [Range(1, 20)]
        public string Telefone { get; set; }

        /// <summary>
        /// Este coeficiente, baseado no número de inscrições validadas durante o semestre e inscrições na semana atual,
        /// determina a prioridade da saída do aluno em uma atividade. Quanto maior, mais provável a saída.
        /// </summary>
        [Required]
        public double CoeficienteSorte { get; set; }

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
            Pontuacao = 0;
            Telefone = "";
            AnoEntrada = 0;
        }

        #endregion
    }
}