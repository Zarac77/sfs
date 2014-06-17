using System.Collections.Generic;
using System.Linq;
using Sfs.Models;

namespace Sfs.ViewModels.PessoaViewModels
{
    public class IndexViewModel : ListaPaginada<Pessoa>
    {
        public string Nome { get; set; }
        public bool IgnorarInativos { get; set; }

        public IndexViewModel()
        {
            IgnorarInativos = true;
        }
    }
}