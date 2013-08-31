using System.Collections.Generic;

using Sfs.Models;

namespace Sfs.ViewModels.AtividadeViewModels
{
    public class ProximasAtividadesViewModel
    {
        public Pessoa Pessoa { get; set; }

        public IEnumerable<Atividade> Atividades { get; set; }

        public bool PermitirInscricao { get; set; }
    }
}