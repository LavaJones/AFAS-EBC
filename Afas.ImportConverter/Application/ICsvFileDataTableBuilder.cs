using System;
using System.Data;
namespace Afas.ImportConverter.Application
{
    /// <summary>
    /// Builds a Data Table from a CSV File
    /// </summary>
    public interface ICsvFileDataTableBuilder
    {
        /// <summary>
        /// Fills a Data Table Builder from the File provided
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns>A Data Table storing all the data</returns>
        IDataTableBuilder CreateDataTableBuilderFromCsvFile(string sourceFilePath);

        /// <summary>
        /// Fills a Data Table from the File provided
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns>A Data Table storing all the data</returns>
        DataTable CreateDataTableFromCsvFile(string sourceFilePath);
    }
}
