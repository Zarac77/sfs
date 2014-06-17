using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sfs.Models;

namespace Sfs.ViewModels.AtividadeViewModels
{
    public class CreateViewModel
    {
        #region Atributos e propriedades

        public Atividade Atividade { get; set; }
        public Guid IdAtividade { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }
        public string HoraInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }
        public string HoraFim { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataLimiteInscricao { get; set; }
        public string HoraLimiteInscricao { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataLimiteCancelamento { get; set; }
        public string HoraLimiteCancelamento { get; set; }

        public Dictionary<Guid, string> EstadosAtividade { get; set; }

        public string EstadoAtividadeValue { get; set; }

        public List<EstadoAtividade> _EstadosAtividade { get; set; }
        public CreateViewModel() {
            Atividade = new Atividade();
            DataInicio = DateTime.Today;
            DataFim = DateTime.Today;
            DataLimiteCancelamento = DateTime.Today;
            DataLimiteInscricao = DateTime.Today;
            EstadosAtividade = new Dictionary<Guid, string>();

        }
        #endregion
    }
}