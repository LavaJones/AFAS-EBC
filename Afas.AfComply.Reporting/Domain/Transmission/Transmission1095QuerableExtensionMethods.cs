using Afas.AfComply.Reporting.Domain;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Transmission
{
    public static class Transmission1095QuerableExtensionMethods
    {

        public static IQueryable<Transmission1095> FilterForUniqueId(this IQueryable<Transmission1095> transmissions, string uniqueId)
        {
            return (
                    from transmission in transmissions
                    where transmission.UniqueRecordId == uniqueId
                    select transmission
                    );
        }

        public static IQueryable<Transmission1095> FilterForTransmissionStatus(this IQueryable<Transmission1095> transmissions, TransmissionStatus status)
        {
            return (
                    from transmission in transmissions
                    where transmission.TransmissionStatus == status
                    select transmission
                    );
        }

        public static IQueryable<Transmission1095> FilterForTransmissionType(this IQueryable<Transmission1095> transmissions, TransmissionTypes type)
        {
            return (
                    from transmission in transmissions
                    where transmission.TransmissionType == type
                    select transmission
                    );
        }

    }
}
