using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sfs.ViewModels.ControleAcessoViewModels
{
    public class AlterarSenhaViewModel
    {
        [DataType(DataType.Password)]
        [DisplayName("Senha atual")]
        [StringLength(50)]
        public string SenhaAtual { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Nova senha")]
        [StringLength(50)]
        public string NovaSenha { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirmação de nova senha")]
        [StringLength(50)]
        public string ConfirmacaoNovaSenha { get; set; }
    }
}