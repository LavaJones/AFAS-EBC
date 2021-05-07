using System;


namespace Afas.AfComply.UI.Areas.ViewModels.FileCabinet
{
    public class FileCabinetAccessViewModel : BaseViewModel
    {
     
        public virtual bool HasFiles { get; set; }

        public virtual bool HasSubFolders { get; set; }
    
    }
}