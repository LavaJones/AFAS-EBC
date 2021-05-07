using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Marketing.Models;
using System.ComponentModel.DataAnnotations;
namespace Afas.AfComply.Reporting.Core.Models
{
    public class Approved1094FinalPart4Model:Model
    {
        [Required]
        public virtual Approved1094FinalPart1Model Approved1094FinalPart1 { get; set; }

        [Required]
        public virtual String EIN { set; get; }

        [Required]
        public virtual String EmployerName { get; set; }
    }
}
