using Afas.AfComply.UI.Areas.ViewModels;
using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class FileCabinetFolderInfoViewModel : BaseViewModel
    {

        [Required]
        public virtual String FolderName { get; set; }
        [Required]
        public virtual int FolderDepth { get; set; }

        public virtual List<FileCabinetFolderInfoModel> children { get; set; }

        public virtual string ZipDownloadItemLink
        {
            get
            {
                return this.GetEncyptedLink("ZipDownload");
            }
        }

    }
}
