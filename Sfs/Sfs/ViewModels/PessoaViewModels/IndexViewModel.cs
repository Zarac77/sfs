using System.Collections.Generic;
using System.Linq;
using Sfs.Models;

namespace Sfs.ViewModels.PessoaViewModels
{
    public class IndexViewModel : ListaPaginada
    {
        public string Nome { get; set; }
        public bool IgnorarInativos { get; set; }
        public List<Pessoa> Pessoas {
            get {
                var lista = new List<Pessoa> (Lista.Cast<Pessoa>());
                return lista;
            }
        }

        public IndexViewModel()
        {
            IgnorarInativos = true;
        }
    }
}