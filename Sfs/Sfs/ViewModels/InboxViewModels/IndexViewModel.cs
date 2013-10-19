using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;

namespace Sfs.ViewModels.InboxViewModels
{
    public class IndexViewModel : ListaPaginada
    {

        public List<Mensagem> Mensagens { 
            get { 
                var lista = new List<Mensagem> (Lista.Cast<Mensagem>());
                return lista;
            } }
        public IndexViewModel()
        {
        }
    }
}