using Sfs.Models;
using Sfs.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Sfs.Services
{
    public class Servico
    {
        private const int ITENS_POR_PAGINA = 15;

        public static string GetEnumDescricao(object e) {
            var tipoEnum = e.GetType();
            //Só para garantir
            if (tipoEnum.IsEnum) {
                DescriptionAttribute[] descricoes = (DescriptionAttribute[])tipoEnum.GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                var descricao = descricoes.Count() > 0 ? descricoes[0].Description : "";
                return descricao;
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enm"></param>
        /// <returns></returns>
        public static Dictionary<Guid, string> DicFromEstadosAtividade(SfsContext context) {
            var dic = new Dictionary<Guid, string>();
            /*foreach (var e in Enum.GetValues(enm.GetType())) {
                dic.Add(e.ToString(), GetEnumDescricao(e));
            }*/
            var estados = context.EstadosAtividade.ToList();
            for (int i = 0; i < estados.Count(); i++) {
                var estado = estados.Single(e => e.Indice == i);
                dic.Add(estado.Id, estado.Descricao);
            }
                return dic;
        }

        public static ListaPaginada<T> PaginarLista<T>(ListaPaginada<T> listaPaginada, string criterio, bool descending) where T : class {
            var lista = listaPaginada.Lista;
            if(descending)
                lista = lista.OrderByDescending(p => p.GetType().GetProperty(criterio).GetValue(p));
            else
                lista = lista.OrderBy(p => p.GetType().GetProperty(criterio).GetValue(p));
            listaPaginada.TotalPaginas = (int)(lista.Count() / ITENS_POR_PAGINA);
            listaPaginada.Lista = lista
                .Skip(ITENS_POR_PAGINA * listaPaginada.PaginaAtual)
                .Take(ITENS_POR_PAGINA);

            return listaPaginada;
        }

        public static int GetWeekOfYear() {
            return (int)DateTime.Today.DayOfYear / 7;
        }

        /// <summary>
        /// Filtra uma lista à partir de dados de um modelo.
        /// </summary>
        /// <typeparam name="T">Tipo do modelo.</typeparam>
        /// <param name="context">Uma instância da classe de contexto.</param>
        /// <param name="modelo">O objeto modelo cujas informações serão buscadas no banco de dados.</param>
        /// <returns>Retorna uma lista filtrada à partir de um modelo.</returns>
        public static List<Tipo> FiltrarLista<Tipo>(SfsContext context, object modelo) where Tipo : class {
            var lista = context.Set<Tipo>().ToList();
            var info = modelo.GetType().GetProperties();
            foreach (var i in info) {
                var value = i.GetValue(modelo);
                var type = value != null ? value.GetType() : null;
                bool isTipoValido = type == typeof(string) || type == typeof(bool);
                if ((isTipoValido && type == typeof(string)) && !String.IsNullOrEmpty(i.GetValue(modelo).ToString())) {
                    lista = lista
                        .Where(o => i.GetValue(o) != null
                            && i.GetValue(o).ToString().ToUpper().Contains(i.GetValue(modelo).ToString().ToUpper())).ToList();
                }
                else if (isTipoValido) {
                    lista = lista.Where(pessoa => i.GetValue(modelo).Equals(i.GetValue(pessoa))).ToList();
                }
            }
            return lista;
        }

        public static Pessoa GetAdministrador(SfsContext context) {
            return context.Pessoas.SingleOrDefault(p => p.IsAdministrador);
        }
    }
}