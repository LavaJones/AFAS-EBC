using Afas.Application;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.ImportFormatting.Commands;
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
    public class FormatCommandService : ABaseCrudService<AImportFormatCommand>, IFormatCommandService
    {
        protected IFormatCommandRepository Repository { get; private set; }

        private IDefaultFactory<IImportFormatCommand> Factory;

        /// <summary>
        /// Standard Constructor taking the dependencies as parameters. 
        /// </summary>
        /// <param name="Repository">The Repository to get the .</param>
        public FormatCommandService(
            IFormatCommandRepository repository,
            IDefaultFactory<IImportFormatCommand> factory) : 
                base(repository)
        {
            if (null == factory)
            {
                throw new ArgumentNullException("Factory");
            }

            this.Repository = repository;
            this.Factory = factory;
        }

        /// <summary>
        /// Gets a new instance of a specific type of command.
        /// </summary>
        /// <param name="tyoe">The type to get the command for.</param>
        /// <returns>The command or null.</returns>
        IImportFormatCommand IFormatCommandService.GetNewOfType(ImportFormatCommandTypes type)
        {

            return this.Factory.GetById(type.ToString());

        }

    }

}
