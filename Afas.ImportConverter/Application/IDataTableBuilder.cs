using System;
namespace Afas.ImportConverter.Application
{
    /// <summary>
    /// Simplified Data Table constructor
    /// </summary>
    public interface IDataTableBuilder
    {
        /// <summary>
        /// Add a single row to the Data Table
        /// </summary>
        /// <param name="dataTable">The table to add to</param>
        /// <param name="rowValues">The values to add</param>
        void AddRowToTable(string[] rowValues);

        /// <summary>
        /// Creates and returns the data table that this object has been building.
        /// </summary>
        /// <returns>The built data table.</returns>
        System.Data.DataTable Build();

        /// <summary>
        /// Clear out all the data in the builder and reinitalize it with a nameless table
        /// </summary>
        void ClearBuilder();

        /// <summary>
        /// Parse the headers to create the data table columns, an Invalid Header will cause default integer headers to be used.
        /// </summary>
        /// <param name="fileReader">The File stream whose first line should be the header</param>
        /// <param name="dataTable">The data table to build the headers for.</param>
        void ParseHeaders(string[] headers);

        /// <summary>
        /// Reinitializes the table builder, clearing out any built data
        /// </summary>
        /// <param name="tableName">The new table's name</param>
        void ResetBuilder(string tableName);
    }
}
