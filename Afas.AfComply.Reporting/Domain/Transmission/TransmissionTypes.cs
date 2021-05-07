using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Transmission
{
    /// <summary>
    /// The IRS defined type of the Transmission
    /// </summary>
    public enum TransmissionTypes
    {
        Corrected = 'C',
        Origonal = 'O',
        Replacement = 'R'
    }
}
