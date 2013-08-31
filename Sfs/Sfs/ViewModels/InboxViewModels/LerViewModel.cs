using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.ViewModels.InboxViewModels
{
    public class LerViewModel
    {
        public int PaginaOrigem { get; set; }
        public Mensagem Mensagem { get; set; }

        public LerViewModel()
        {
            PaginaOrigem = 0;
        }
    }
}