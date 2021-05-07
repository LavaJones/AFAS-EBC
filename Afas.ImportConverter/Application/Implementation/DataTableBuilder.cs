using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Afas.ImportConverter.Application.Implementation
{

    /// <summary>
    /// Builds a Data Table 
    /// </summary>
    public class DataTableBuilder : IDataTableBuilder
    {

        private ILog Log = LogManager.GetLogger(typeof(DataTableBuilder));

        /// <summary>
        /// The Validator to be used to ensure that the headers are correct.
        /// </summary>
        private IHeaderValidator HeaderValidator;

        /// <summary>
        /// The Data table that we are building
        /// </summary>
        private DataTable dataTable;

        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="headerValidator">Validator conforming to the rules of header validation</param>
        public DataTableBuilder(IHeaderValidator headerValidator)
        {
            if(null == headerValidator)
            {
                throw new ArgumentNullException("headerValidator");
            }
            HeaderValidator = headerValidator;

            dataTable = new DataTable();
        }
        
        /// <summary>
        /// Parse the headers to create the data table columns, an Invalid Header will cause default integer headers to be used.
        /// </summary>
        /// <param name="fileReader">The File stream whose first line should be the header</param>
        /// <param name="dataTable">The data table to build the headers for.</param>
        public void ParseHeaders(string[] headers) 
        {
            if (HeaderValidator.ValidateHeaders(headers))
            {
                foreach (String header in headers)
                {
                    //Add the headers 
                    dataTable.Columns.Add(header.Trim());
                }
            }
            else
            {
                for (int i = 0; i < headers.Length; i++ )
                {
                    //add a generic Int header as a placeholder
                    dataTable.Columns.Add(i.ToString());
                }

                // since this row isn't a header we need to add it to the data table
                this.AddRowToTable(headers);
            }
        }

        /// <summary>
        /// Add a single row to the Data Table
        /// </summary>
        /// <param name="dataTable">The table to add to</param>
        /// <param name="rowValues">The values to add</param>
        public void AddRowToTable(string[] rowValues)
        {
            DataRow dataRow = dataTable.NewRow();

            for (int i = 0; i < rowValues.Length; i++)
            {

                dataRow[i] = rowValues[i];

            }

            dataTable.Rows.Add(dataRow);
        }

        /// <summary>
        /// Creates and returns the data table that this object has been building.
        /// </summary>
        /// <returns>The built data table.</returns>
        public DataTable Build()
        {
            return dataTable.Copy();        
        }

        /// <summary>
        /// Reinitializes the table builder, clearing out any built data
        /// </summary>
        /// <param name="tableName">The new table's name</param>
        public void ResetBuilder(string tableName)
        {
            dataTable = new DataTable(tableName);
        }        

        /// <summary>
        /// Clear out all the data in the builder and reinitalize it with a nameless table
        /// </summary>
        public void ClearBuilder()
        {
            dataTable = new DataTable();
        }
    }
}