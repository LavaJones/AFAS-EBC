using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using log4net;

namespace Afas.Application.CSV
{
    /// <summary>
    /// Static access to the Regex parsing of a CSV File
    /// </summary>
    public class CsvParse : ICsvParser
    {
        private ILog Log = LogManager.GetLogger(typeof(CsvParse));

        private static string matchingPattern = @",(?!(?<=(?:^|,)\s*""(?:[^""]|""""|\\"")*,)(?:[^""]|""""|\\"")*""\s*(?:,|$))";

        private static Regex commaQuotedExpression = new Regex(matchingPattern, RegexOptions.IgnoreCase);

        /// <summary>
        /// Split the Input based on the rules of the CSV Regex
        /// </summary>
        /// <param name="row">The CSV Row of data</param>
        /// <returns>The split data</returns>
        string[] ICsvParser.SplitRow(string row)
        {
            return CsvParse.SplitRow(row);
        }

        /// <summary>
        /// Split the Input based on the rules of the CSV Regex
        /// </summary>
        /// <param name="row">The CSV Row of data</param>
        /// <returns>The split data</returns>
        public static string[] SplitRow(string row)
        {
            return commaQuotedExpression.Split(row);
        }

        /// <summary>
        /// Combine a row of data into a single line of CSV
        /// </summary>
        /// <param name="rowValues">The list of items to be combined into a single CSV row</param>
        /// <returns>The CSV row of the items</returns>
        string ICsvParser.CombineRow(IEnumerable<string> rowValues)
        {
            return CsvParse.CombineRow(rowValues);
        }

        /// <summary>
        /// Combine a row of data into a single line of CSV
        /// </summary>
        /// <param name="rowValues">The list of items to be combined into a single CSV row</param>
        /// <returns>The CSV row of the items</returns>
        public static string CombineRow(IEnumerable<string> rowValues)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string item in rowValues)
            {
                // Bits of code from: http://stackoverflow.com/questions/6377454/escaping-tricky-string-to-csv-format
                if (item.Contains(",") || item.Contains("\"") || item.Contains("\r") || item.Contains("\n"))
                {
                    builder.Append("\"");
                    foreach (char nextChar in item)
                    {
                        builder.Append(nextChar);
                        if (nextChar == '"')
                            builder.Append("\"");
                    }
                    builder.Append("\"");
                }
                else
                {
                    builder.Append(item);
                }

                builder.Append(',');
            }

            //build it and trim the last ,
            return builder.ToString().TrimEnd(',');
        }
    }
}
