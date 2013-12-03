using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.ViewModels.AtividadeViewModels
{
    public class ListarViewModel
    {
        public Guid IdAtividade { get; set; }
        public string CampoMatricula { get; set; }
        public string CampoTurma { get; set; }
        public Guid[] IdSelecionados { get; set; }
        public bool HaFixadas { get; set; }
        public bool HaInscritos { get; set; }

        public ListarViewModel() {
            HaFixadas = false;
            HaInscritos = false;
        }
    }
}