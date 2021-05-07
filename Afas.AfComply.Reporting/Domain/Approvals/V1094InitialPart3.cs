
using System;
using System.ComponentModel.DataAnnotations;

using Afc.Core;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    /// <summary>
    /// Pulls the live data for the tax year approval and other_ale_member to complete the 1094 Part 4.
    /// </summary>
    public class V1094InitialPart3 : BaseReportingModel
    {

        public V1094InitialPart3() : base()
        {

        }

        [Required]
        public virtual int EmployerId { get; set; }
        [Required]
        public virtual int NoOfEmployees { get; set; }
        [Required]
        public virtual int Receiving1095C { get; set; }
        [Required]
        public virtual int TaxYear { get; set; }
        [Required]
        public virtual int OfferedInsurance { get; set; }
        [Required]
        public virtual int MonthId { get; set; }


    }

}
