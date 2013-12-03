using Sfs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfs.ViewModels.ControleViewModels {
    public class ConfirmarPresencasViewModel {
        public Guid IdAtividade { get; set; }
        public Atividade Atividade { get; set; }
        /// <summary>
        /// Ids das pessoas selecionadas para confirmação da presença.
        /// </summary>
        public Guid[] IdSelecionados { get; set; }
    }
}