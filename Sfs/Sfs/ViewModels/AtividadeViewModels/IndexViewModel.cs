using System.Collections.Generic;

using Sfs.Models;
using System;

namespace Sfs.ViewModels.AtividadeViewModels
{
    public class IndexViewModel
    {
        public bool ExibirArquivadas { get; set; }
        public List<Atividade> Atividades { get; set; }

        public IDictionary<Guid, string> EstadosAtividade { get; set; }

        public IndexViewModel()
        {
            ExibirArquivadas = false;
            Atividades = new List<Atividade>();
        }
    }
}