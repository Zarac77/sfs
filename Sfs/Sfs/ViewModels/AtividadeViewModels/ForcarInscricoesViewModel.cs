using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.ViewModels.AtividadeViewModels
{
    public class ForcarInscricoesViewModel
    {
        public Guid IdAtividade { get; set; }
        public Atividade Atividade { get; set; }
        public string CampoMatricula { get; set; }
        public string CampoTurma { get; set; }
        public Guid[] IdSelecionados { get; set; }
    }
}