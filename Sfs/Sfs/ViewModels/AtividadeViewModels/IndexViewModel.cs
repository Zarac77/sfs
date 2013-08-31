using System.Collections.Generic;

using Sfs.Models;

namespace Sfs.ViewModels.AtividadeViewModels
{
    public class IndexViewModel
    {
        public bool ExibirNaoAprovadas { get; set; }
        public List<Atividade> Atividades { get; set; }

        public IndexViewModel()
        {
            ExibirNaoAprovadas = false;
            Atividades = new List<Atividade>();
        }
    }
}