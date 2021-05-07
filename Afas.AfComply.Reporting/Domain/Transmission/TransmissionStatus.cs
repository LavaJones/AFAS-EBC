using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Transmission
{
    public enum TransmissionStatus
    {
        Accepted = 1,
        AcceptedWithErrors = 2,
        Regected = 3,
        Processing = 4,
        NotFound = 5
    }
}
