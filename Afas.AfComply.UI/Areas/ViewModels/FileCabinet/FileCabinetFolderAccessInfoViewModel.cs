using Afas.AfComply.UI.Areas.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Afas.AfComply.Reporting.Core.Models
{
    public class FileCabinetFolderAccessInfoViewModel : BaseViewModel
    {

        [Required]
        public virtual String FolderName { get; set; }
        [Required]
        public virtual int FolderDepth { get; set; }

        public virtual List<FileCabinetFolderAccessInfoViewModel> children { get; set; }

        public virtual bool HasFiles { get; set; }

        public virtual bool HasSubFolders { get { return children.Count > 0; } }

        public virtual string ZipDownloadItemLink
        {
            get
            {
                return this.GetEncyptedLink("ZipDownload");
            }
        }
        public virtual string FolderDeleteItemLink
        {
            get
            {
                return this.GetEncyptedLink("FolderDelete");
            }
        }
    }
}
