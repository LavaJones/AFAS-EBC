using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Application.CSV
{
    /// <summary>
    /// This Interface defines the methods required to parse csv
    /// </summary>
    public interface ICsvParser
    {
        /// <summary>
        /// Split the Input based on the rules of CSV parsing
        /// </summary>
        /// <param name="row">The CSV Row of data as a single string</param>
        /// <returns>The split data items</returns>
        string[] SplitRow(string row);

        /// <summary>
        /// Combine a row of data into a single line of CSV
        /// </summary>
        /// <param name="rowValues">The list of items to be combined into a single CSV row</param>
        /// <returns>The CSV row of the items</returns>
        string CombineRow(IEnumerable<string> rowValues);
    }
}

