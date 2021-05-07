using System;

namespace Afas.AfComply.UI.Code.POCOs
{
    public class EmployerFile 
    {
        public string EmployerName{ get; set; }
        public int EmployerId{ get; set; }
        public string FileFullPath{ get; set; }
        public long Length{ get; set; }
        public string FileName{ get; set; }
        public DateTime CreationTimeUtc{ get; set; }
        
    }
}