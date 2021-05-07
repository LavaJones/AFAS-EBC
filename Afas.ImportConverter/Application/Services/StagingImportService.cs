using Afas.Application;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.ImportFormatting.Commands;
using Afas.ImportConverter.Domain.ImportFormatting.Staging;
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
    public class StagingImportService : ABaseCrudService<StagingImport>, IStagingImportService
    {
        protected IStagingImportRepository Repository { get; private set; }

        /// <summary>
        /// Standard Constructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="Repository">The Repository to get the .</param>
        public StagingImportService(
            IStagingImportRepository repository) : 
                base(repository)
        {

            this.Repository = repository;

        }

        StagingImport IStagingImportService.UpdateAfterProcessing(StagingImport import, String authorizingUser)
        {
            if (import.Modified == null)
            {
                throw new ArgumentNullException("import.Modified");
            }
            if (import.EnsureIsWellFormed.Count > 0)
            {
                throw new ArgumentException("Import Argument is not well formed.");
            }

            StagingImport result = new StagingImport();
            result.CreatedBy = authorizingUser;
            result.ModifiedBy = authorizingUser;
            result.ModifiedDate = DateTime.Now;
            result.Original = import.Modified;
            result.Modified = null;
            result.UploadInfo = import.UploadInfo;
            result.EntityStatus = EntityStatusEnum.Active;

            if (result.EnsureIsWellFormed.Count > 0)
            {
                throw new ArgumentException("Result of Import is not well formed.");
            }
            Repository.InsertOnSubmit(result);

            StagingImport data = Repository.GetByResourceId(import.ResourceId);

            data.Modified = import.Modified;
            data.EntityStatus = EntityStatusEnum.Inactive;
            data.ModifiedBy = authorizingUser;
            data.ModifiedDate = DateTime.Now;

            if (data.EnsureIsWellFormed.Count > 0)
            {
                throw new ArgumentException("Import Argument is not well formed.");
            }

            Repository.SaveChanges();

            return Repository.GetByResourceId(result.ResourceId);

        }

    }

}
