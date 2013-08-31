using System;
using System.Collections.Generic;

using Sfs.Models;

namespace Sfs.ViewModels.PessoaViewModels
{
    public class AssociarPerfisViewModel
    {
        public Pessoa Pessoa { get; set; }

        public List<Perfil> TodosPerfis { get; set; }

        public List<Perfil> PerfisSelecionados { get; set; }

        public AssociarPerfisViewModel()
        {
            TodosPerfis = new List<Perfil>();
            PerfisSelecionados = new List<Perfil>();
        }
    }
}