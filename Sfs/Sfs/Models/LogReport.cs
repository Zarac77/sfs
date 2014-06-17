using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sfs.Models
{
    public class LogReport
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        public string Descricao { get; set; }

        public string Log { get; set; }
    }
}