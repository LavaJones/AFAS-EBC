using System;
using System.ComponentModel.DataAnnotations;

using Afc.Core;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    /// <summary>
    /// Pulls the live data for the employer, tax year approval and approved 1095's to complete the 1094 Part 2.
    /// </summary>
    public class V1094InitialPart2 : BaseReportingModel
    {

        public V1094InitialPart2() : base()
        {

        }

        [Required]
        public virtual int EmployerId { set; get; }

        [Required]
        public virtual Boolean IsAle { set; get; }

        [Required]
        public virtual int Form1095Count { set; get; }

    }

}
