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
        public DateTime DataInicio
        {
            get { return Atividade.DataHoraInicio; }
            set { Atividade.DataHoraInicio = value; }
        }
        public string HoraInicio
        {
            get { return Atividade.DataHoraInicio.TimeOfDay.ToString(); }
            set { var a = Atividade.DataHoraInicio.ToString("dd/MM/yyyy") + " " + value;
            Atividade.DataHoraInicio = DateTime.Parse(a);
            }
        }

        public DateTime DataFim
        {
            get { return Atividade.DataHoraFim.Date; }
            set { Atividade.DataHoraFim = value; }
        }
        public string HoraFim
        {
            get { return Atividade.DataHoraFim.TimeOfDay.ToString(); }
            set { Atividade.DataHoraFim = DateTime.Parse(Atividade.DataHoraFim + " " + value); }
        }
        #endregion
    }
}