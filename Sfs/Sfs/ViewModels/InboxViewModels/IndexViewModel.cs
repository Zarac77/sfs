using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;

namespace Sfs.ViewModels.InboxViewModels
{
    public class IndexViewModel : ListaPaginada
    {
        public IEnumerable<Mensagem> Mensagens { get; set; }
        public IndexViewModel()
        {
        }
    }
}