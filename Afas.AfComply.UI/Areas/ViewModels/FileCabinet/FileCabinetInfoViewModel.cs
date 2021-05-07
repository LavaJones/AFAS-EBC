using System;


namespace Afas.AfComply.UI.Areas.ViewModels.FileCabinet
{
    public class FileCabinetInfoViewModel : BaseViewModel
    {

        public virtual String Filename { get; set; }

        public virtual String FileDescription { get; set; }

        public virtual String FileType { get; set; }

        public virtual string DownloadItemLink
        {
            get
            {
                return this.GetEncyptedLink("DownloadFile");
            }
        }
        public virtual string DeletItemLink
        {
            get
            {
                return this.GetEncyptedLink("DeletFile");
            }
        }

    }
}