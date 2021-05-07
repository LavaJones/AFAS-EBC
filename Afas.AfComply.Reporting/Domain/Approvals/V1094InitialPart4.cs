using System;
using System.ComponentModel.DataAnnotations;

using Afc.Core;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    /// <summary>
    /// Pulls the live data for the tax year approval and other_ale_member to complete the 1094 Part 4.
    /// </summary>
    public class V1094InitialPart4 : BaseReportingModel
    {

        public V1094InitialPart4() : base()
        {

        }

        [Required]
        public virtual String EIN { set; get; }

        [Required]
        public virtual int EmployerId { get; set; }

        [Required]
        public virtual String Name { get; set; }

    }

}
