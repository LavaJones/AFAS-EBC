using Afc.Marketing.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Request
{
    public class FileCabinetFilesRequests : BaseRequest
    {

        public FileCabinetFilesRequests() : base()
        {

        }
        public virtual int CurrentApplicationId { get; set; }

        public virtual Guid ResourceId { get; set; }

        public virtual Guid OwnerResourceId { get; set; }

    }
}
