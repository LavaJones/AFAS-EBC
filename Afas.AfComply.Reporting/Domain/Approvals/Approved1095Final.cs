using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Afas.AfComply.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    /// <summary>
    /// The Final approved data for the employer
    /// </summary>
    public class Approved1095Final : BaseReportingModel
    {

        public string FileName { get { return string.Format("{0}_{1}_{2}_{3}", this.LastName, this.FirstName, this.SSN.Substring(this.SSN.Length - 4), this.EmployeeResourceId); } }

        [NotMapped]
        public virtual bool Receiving1095
        {
            get
            {
                return (this.part3s.Count() > 0
                        ||
                        (from Approved1095FinalPart2 part2 in this.part2s where part2.Receiving1095C select part2).Count() > 0);
            }
        }

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

        //Not used by code, but printed on form
        public virtual DateTime? DOB { get { return null; } }

        //Not used by code, but printed on form
        public virtual string Suffix { get { return ""; } }


        [NotMapped]

        public virtual bool Printed { get; set; }


        public virtual List<Approved1095FinalPart2> part2s { set; get; }

        public virtual List<Approved1095FinalPart3> part3s { set; get; }

    }
}
