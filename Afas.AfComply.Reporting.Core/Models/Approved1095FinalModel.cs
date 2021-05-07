using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Afc.Marketing.Models;

namespace Afas.AfComply.Reporting.Core.Models
{

    /// <summary>
    /// The Final approved data for the employer
    /// </summary>
    public class Approved1095FinalModel : Model
    {

        public virtual bool Printed { get; set; }

        public virtual bool Receiving1095 { get; set; }

        [Required]
        public virtual int TaxYear { set; get; }

        [Required]
        public virtual int EmployerID { set; get; }

        [Required]
        public virtual int EmployeeID { set; get; }
        
        [Required]
        public virtual Guid EmployeeResourceId { set; get; }

        [Required]
        public virtual string FirstName { set; get; }

        public virtual string MiddleName { set; get; }

        [Required]
        public virtual string LastName { set; get; }

        [Required]
        public virtual string SSN { set; get; }

        [Required]
        public virtual string StreetAddress { set; get; }

        [Required]
        public virtual string City { set; get; }

        [Required]
        public virtual string State { set; get; }

        [Required]
        public virtual string Zip { set; get; }

        [Required]
        public virtual bool SelfInsured { set; get; }

        public virtual List<Approved1095FinalPart2Model> part2s { set; get; }

        public virtual List<Approved1095FinalPart3Model> part3s { set; get; }

    }
}
