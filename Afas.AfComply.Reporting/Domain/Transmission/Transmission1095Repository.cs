using Afas.Domain;
using Afc.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Transmission
{
    public class Transmission1095Repository : BaseDomainRepository<Transmission1095, IReportingDataContext>, ITransmission1095Repository
    {
        public IQueryable<Transmission1095> FilterForTransmissionStatus(TransmissionStatus status)
        {
            return Context.Set<Transmission1095>()
                .FilterForTransmissionStatus(status)
                .FilterForActive();
        }

        public IQueryable<Transmission1095> FilterForTransmissionType(TransmissionTypes type)
        {
            return Context.Set<Transmission1095>()
                .FilterForTransmissionType(type)
                .FilterForActive();
        }

        public IQueryable<Transmission1095> FilterForUniqueRecordId(string uniqueRecordId)
        {
            return Context.Set<Transmission1095>()
                .FilterForUniqueId(uniqueRecordId)
                .FilterForActive();
        }
    }
}
