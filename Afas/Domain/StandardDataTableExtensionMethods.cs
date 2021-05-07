using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Domain
{
    public static class StandardDataTableExtensionMethods
    {

        private static ILog Log = LogManager.GetLogger(typeof(StandardDataTableExtensionMethods));

        /// <summary>
        /// Check to see if any item in the row is not null or blank.
        /// </summary>
        /// <param name="processedRow">The row to test</param>
        /// <returns>True if every item in the row is null or empty</returns>
        public static bool IsRowBlank(this DataRow row)
        {
            foreach (object obj in row.ItemArray)
            {
                if (obj != null && false == obj.ToString().IsNullOrEmpty())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a list of the headers as strings 
        /// </summary>
        /// <param name="table">The table to get the list of headers for.</param>
        /// <returns>A list of strings representing the headers.</returns>
        public static List<string> GetHeaders(this DataTable table)
        {
            List<string> headers = new List<string>();

            foreach (DataColumn dataColumn in table.Columns)
            {
                headers.Add(dataColumn.ColumnName);
            }

            return headers;
        }

    }

}
