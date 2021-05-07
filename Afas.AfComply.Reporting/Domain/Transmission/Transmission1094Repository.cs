using Afas.Domain;
using Afc.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Transmission
{
    public class Transmission1094Repository : BaseDomainRepository<Transmission1094, IReportingDataContext>, ITransmission1094Repository
    {
        public IQueryable<Transmission1094> FilterForTransmissionStatus(TransmissionStatus status)
        {
            return Context.Set<Transmission1094>()
                .FilterForTransmissionStatus(status)
                .FilterForActive();
        }

        public IQueryable<Transmission1094> FilterForTransmissionType(TransmissionTypes type)
        {
            return Context.Set<Transmission1094>()
                .FilterForTransmissionType(type)
                .FilterForActive();
        }

        public IQueryable<Transmission1094> FilterForUniqueRecordId(string uniqueRecordId)
        {
            return Context.Set<Transmission1094>()
                .FilterForUniqueId(uniqueRecordId)
                .FilterForActive();
        }
    }
}
