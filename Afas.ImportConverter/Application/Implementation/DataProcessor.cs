using Afas.ImportConverter.Domain.POCO;
using Afas.ImportConverter.Domain.ImportFormatting;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Afas.ImportConverter.Application.Services;
using Afc.Core.Application;

namespace Afas.ImportConverter.Application.Implementation
{
    /// <summary>
    /// This class specalizes in processing data
    /// </summary>
    public class DataProcessor : IDataProcessor
    {

        private ILog Log = LogManager.GetLogger(typeof(DataProcessor));

        /// <summary>
        /// factory to get all the Format Commands
        /// </summary>
        private IFormatCommandService Service;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service">This requires a factory to get all the Format Commands</param>
        public DataProcessor(IFormatCommandService service, ITransactionContext TransactionContext)
        {
            if (null == service)
            {
                throw new ArgumentNullException("Service");
            }

            this.Service = service;
            this.Service.Context = TransactionContext;

        }

        /// <summary>
        /// Processes this Data Table based on the provided Meta Data
        /// </summary>
        /// <param name="metaData">The meta Data that describes this data</param>
        /// <returns>True if the processing succeded, false if it failed.</returns>
        public bool ProcessImportDataTable(ImportData metaData)
        {

            if (null == metaData)
            {
                throw new ArgumentNullException("metaData");
            }

            if (null == metaData.Data)
            {
                throw new ArgumentNullException("dataTable");
            }

            try
            {

                List<IImportFormatCommand> Commands = Service.GetAllEntities().ToList<IImportFormatCommand>();

                Commands.Sort();

                // Apply all applicable commands in order
                foreach (IImportFormatCommand Command in Commands)
                {
                    if (Command.AppliesTo(metaData.MetaData))
                    {

                        Command.ApplyTo(metaData);
                    }
                }

                // apply all applicable commands in reverse order
                Commands.Reverse();
                foreach (IImportFormatCommand Command in Commands)
                {
                    if (Command.AppliesTo(metaData.MetaData))
                    {

                        Command.ApplyTo(metaData);
                    }
                }

            }
            catch (Exception ex)
            {

                Log.Error("Exception While Processing ImportFormatCommands.", ex);
                return false;

            }

            return true;

        }
    }
}