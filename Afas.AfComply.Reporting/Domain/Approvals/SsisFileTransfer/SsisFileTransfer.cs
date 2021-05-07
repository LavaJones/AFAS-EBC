using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Core;
using Afas.Domain;
using Afc.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer
{

    public class SsisFileTransfer : BaseReportingModel
    {

        //[Required]
        //public virtual int SSISFileTransferId { get; set; }
        [MaxLength(50)]
        public virtual String FEIN { get; set; }

        [MaxLength]
        public virtual String FileName { get; set; }

        //public virtual int EntityStatusId { get; set; }

        public DateTime RunTime { get; set; }

        public virtual int EmployerId { get; set; }

    }

  }
