using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sfs.Models;

namespace Sfs.ViewModels.InboxViewModels
{
    public class IndexViewModel
    {
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<Mensagem> Mensagens { get; set; }
        public bool[] Delete { get; set; }
       /* public IEnumerable<DeleteInfo> DeleteInfos { get; set; }

        public struct DeleteInfo 
        {
            public bool Deletar { get; set; }
            public Guid MensagemId { get; set; }
            public DeleteInfo()
            {
                Deletar = false;
            }
        }*/

        public IndexViewModel()
        {
            PaginaAtual = 0;
            TotalPaginas = 1;
            Delete = new bool[10];
        }
    }
}