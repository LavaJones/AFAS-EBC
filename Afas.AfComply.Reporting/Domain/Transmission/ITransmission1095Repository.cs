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
    public interface ITransmission1095Repository : IDomainRepository<Transmission1095>
    {

        IQueryable<Transmission1095> FilterForTransmissionStatus(TransmissionStatus status);


        IQueryable<Transmission1095> FilterForTransmissionType(TransmissionTypes type);


        IQueryable<Transmission1095> FilterForUniqueRecordId(string UniqueRecordId);
    }
}
