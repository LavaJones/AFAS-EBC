using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.FileCabinet
{
    public class FileCabinetAccess : BaseReportingModel
    {
        [Required]
        public virtual Guid OwnerResourceId { get; set; }

        [Required]
        public virtual bool HasFiles { get; set; }

        public virtual bool HasSubFolders { get; set; }

        [Required]
        public virtual int ApplicationId { get; set; }
        [Required]
        public virtual FileCabinetFolderInfo FileCabinetFolderInfo { get; set; }

        [NotMapped]
        public virtual List<FileCabinetAccess> children { get; set; }


    }
}
