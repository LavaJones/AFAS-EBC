using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.POCO
{
    public class measurementBreakInService : BaseAfasModel
    {
        public int measurementBreakInserviceId { get; set; }

        public int measurement_id { get; set; }

        public int BreakInServiceId { get; set; }
    }
}
