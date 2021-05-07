using System;
using Afas.ImportConverter.Domain.POCO;

namespace Afas.ImportConverter.Application
{

    /// <summary>
    /// Subcomponent responsible for processing the datatable into a useable format
    /// </summary>
    public interface IDataProcessor
    {

        /// <summary>
        /// Processes this Data Table based on the provided Meta Data
        /// </summary>
        /// <param name="metaData">The data and Meta-Data that needs processing</param>
        /// <returns>True if the processing succeded, false if it failed.</returns>
        bool ProcessImportDataTable(ImportData metaData);

    }
}
