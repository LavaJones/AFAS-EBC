using Afas.AfComply.Reporting.Domain.TimeFrames;
using Afas.AfComply.Reporting.Domain.Transmission;
using Afas.Application;
using Afc.Core;
using Afc.Core.Domain;
using Afc.Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Application.Services
{
    /// <summary>
    /// A service explosing access to the TimeFrame domain models.
    /// </summary>
    public class Transmission1095Service : ABaseCrudService<Transmission1095>, ITransmission1095Service
    {
        protected ITransmission1095Repository Transmission1095Repository { get; private set; }
        
        /// <summary>
        /// Standard COnstructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="timeFrameRepository">The Repository to get the Time frames.</param>
        public Transmission1095Service(
            ITransmission1095Repository transmission1095Repository) : 
                base(transmission1095Repository)
        {
            
            this.Transmission1095Repository = transmission1095Repository;

        }

    }

}
