using Afas.Application;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.ImportFormatting.Commands;
using Afas.ImportConverter.Domain.ImportFormatting.Staging;
using Afas.ImportConverter.Domain.ImportFormatting.UploadedData;
using Afas.ImportConverter.Domain.POCO;
using Afc.Core;
using Afc.Core.Domain;
using Afc.Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Application.Services
{
    /// <summary>
    /// A service explosing access to the  domain models.
    /// </summary>
    public class UploadedDataInfoService : ABaseCrudService<BaseUploadedDataInfo>, IUploadedDataInfoService
    {
        protected IUploadedDataInfoRepository Repository { get; private set; }

        /// <summary>
        /// Standard Constructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="Repository">The Repository to get the .</param>
        public UploadedDataInfoService(
            IUploadedDataInfoRepository repository) : 
                base(repository)
        {

            this.Repository = repository;

        }

    }

}
