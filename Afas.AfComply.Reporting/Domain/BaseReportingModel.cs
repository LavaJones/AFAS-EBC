using Afas.Domain;
using Afc.Core;
using Afc.Core.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain
{

    public abstract class BaseReportingModel : BaseAfasModel
    {

        /// <summary>
        /// The tables PK
        /// </summary>
        [Key]
        public virtual long ID { get; set; }

        public BaseReportingModel()
            : base()
        {
        }
        
    }

}
