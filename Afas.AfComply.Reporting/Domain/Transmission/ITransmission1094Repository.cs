using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Transmission
{
    /// <summary>
    /// Time Frame specific repository.
    /// </summary>
    public interface ITransmission1094Repository : IDomainRepository<Transmission1094>
    {

        IQueryable<Transmission1094> FilterForTransmissionStatus(TransmissionStatus status);


        IQueryable<Transmission1094> FilterForTransmissionType(TransmissionTypes type);


        IQueryable<Transmission1094> FilterForUniqueRecordId(string UniqueRecordId);

    }
}
