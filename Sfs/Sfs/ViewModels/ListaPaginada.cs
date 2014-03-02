using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.ViewModels
{
    public class ListaPaginada<T> where T : class
    {
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<T> Lista { get; set; }

        public ListaPaginada()
        {

            PaginaAtual = 0;
            TotalPaginas = 1;
        }
    }
}