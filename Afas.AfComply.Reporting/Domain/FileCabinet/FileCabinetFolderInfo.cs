using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Afas.AfComply.Reporting.Domain.FileCabinet
{


    public class FileCabinetFolderInfo : BaseReportingModel
    {
        [Required]
        public virtual String FolderName { get; set; }
        [Required]
        public virtual int FolderDepth { get; set; }
        [Required]
        public virtual int ApplicationId { get; set; }

        public virtual List<FileCabinetFolderInfo> children { get; set; }     
    }
}
