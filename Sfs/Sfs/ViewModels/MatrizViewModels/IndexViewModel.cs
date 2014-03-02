using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.ViewModels.MatrizViewModels {
    public class IndexViewModel : ListaPaginada<Pessoa> {
        public Dictionary<Guid, int> SaidasPessoa { get; set; }

        public IndexViewModel() {
            SaidasPessoa = new Dictionary<Guid, int>();
        }
    }
}