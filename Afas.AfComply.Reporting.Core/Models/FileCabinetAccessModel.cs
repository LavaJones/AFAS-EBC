using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Afas.AfComply.Reporting.Core.Models
{
    public  class FileCabinetAccessModel : Model
    {
        [Required]
        public virtual Guid OwnerResourceId { get; set; }

        [Required]
        public virtual bool HasFiles { get; set; }

        public virtual bool HasSubFolders { get; set; }

        [Required]
        public virtual int ApplicationId { get; set; }
        [Required]
        public virtual FileCabinetFolderInfoModel FileCabinetFolderInfo { get; set; }

        public virtual List<FileCabinetAccessModel> children { get; set; }

    }
}
