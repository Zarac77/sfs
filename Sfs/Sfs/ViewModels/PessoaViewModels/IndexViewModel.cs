using System.Collections.Generic;

using Sfs.Models;

namespace Sfs.ViewModels.PessoaViewModels
{
    public class IndexViewModel : ListaPaginada
    {
        public string Nome { get; set; }
        public bool IgnorarInativos { get; set; }

        public List<Pessoa> Pessoas { get; set; }

        public IndexViewModel()
        {
            IgnorarInativos = true;
            Pessoas = new List<Pessoa>();
        }
    }
}