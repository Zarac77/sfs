using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sfs.Models
{
    public class Perfil
    {
        public static readonly Guid GUID_PERFIL_ADMINISTRADOR = new Guid("08DD8A2F-F127-442E-978A-FBE87909F8BD");
        public static readonly Guid GUID_PERFIL_ALUNO = new Guid("8E3B8FD3-F3B8-4C55-9191-46AB0F420477");
        public static readonly Guid GUID_PERFIL_PROFESSOR = new Guid("D3FC9FE3-D5A8-4E21-BB8C-AD7186FE8FE8");

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; }

        public List<Pessoa> Pessoas { get; set; }

        public Perfil()
        {
            Pessoas = new List<Pessoa>();
        }
    }
}