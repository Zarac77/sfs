using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.ViewModels {
    public class _ListarViewModel {
        public Atividade Atividade { get; set; }
        public Guid IdAtividade { get; set; }
        public bool MostrarCheckboxes { get; set; }
    }
}