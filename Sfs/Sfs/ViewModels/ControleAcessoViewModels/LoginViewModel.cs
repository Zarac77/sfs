using System.ComponentModel.DataAnnotations;

namespace Sfs.ViewModels.ControleAcessoViewModels
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Senha { get; set; }
    }
}