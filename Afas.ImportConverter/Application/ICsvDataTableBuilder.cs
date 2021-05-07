using System;
using System.Data;
namespace Afas.ImportConverter.Application
{
    /// <summary>
    /// Constructs a Data Table from CSV source data
    /// </summary>
    public interface ICsvDataTableBuilder
    {
        /// <summary>
        /// Fills a Data Table Builder from the stream provided
        /// </summary>
        /// <param name="dataReader">The reader that pulls the data in CSV format</param>
        /// <returns>A datatable Builder that is filled with all the values from the data stream</returns>
        IDataTableBuilder CreateDataTableBuilderFromCsv(System.IO.StreamReader dataReader);

        /// <summary>
        /// Fills a Data Table from the stream provided
        /// </summary>
        /// <param name="dataReader">The reader that pulls the data in CSV format</param>
        /// <returns>A datatable Builder that is filled with all the values from the data stream</returns>
        DataTable CreateDataTableFromCsv(System.IO.StreamReader dataReader);
    }
}
