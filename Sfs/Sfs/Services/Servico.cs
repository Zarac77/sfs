using Sfs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.Services
{
    public class Servico
    {
        private const int ITENS_POR_PAGINA = 15;

        public static bool AdicionarEntry(object entry, ref List<object> lista)
        {
            lista.Add(entry);
            return true;
        }

        public static object PaginarLista(ListaPaginada lista, string criterio) {
            
            var indiceInicial = lista.PaginaAtual * ITENS_POR_PAGINA;
            int totalItens = lista.Lista.Count();
            lista.Lista = lista
                            .Lista
                            .OrderBy(p => p.GetType().GetProperty(criterio).GetValue(p))
                            .Skip(indiceInicial)
                            .Take(ITENS_POR_PAGINA).ToList();
            double totalPaginas = (double)totalItens / ITENS_POR_PAGINA;
            lista.TotalPaginas = (int)Math.Ceiling(totalPaginas);
            return lista;
        }

        public static int GetWeekOfYear() {
            return (int)DateTime.Today.DayOfYear / 7;
        }
    }
}