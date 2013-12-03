using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.ViewModels.AtividadeViewModels
{
    public class GerarListaViewModel
    {
        public string Matricula { get; set; }
        public string Turma { get; set; }

        public Guid IdAtividade { get; set; }
        public Atividade Atividade { get; set; }
        public IEnumerable<Pessoa> ResultadoPessoas { get; set; }

        public _ListarViewModel _ListarViewModel { get; set; }
    }
}