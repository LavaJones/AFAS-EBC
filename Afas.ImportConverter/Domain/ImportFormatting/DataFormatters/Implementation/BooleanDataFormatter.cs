using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.Domain;

namespace Afas.ImportConverter.Domain.ImportFormatting.DataFormatters.Implementation
{

    /// <summary>
    /// Formatter for values that can be converted into whole numbers
    /// </summary>
    public class BooleanDataFormatter : IDataFormatter
    {

        /// <summary>
        /// Do the actual Data parsing here for reusability
        /// </summary>
        /// <param name="data">The data to be parsed</param>
        /// <returns>The double or null if unparseable.</returns>
        private bool? ParseData(string data, string format = null)
        {

            if (data.IsNullOrEmpty())
            {
                return null;
            }

            bool result = false;
            if (bool.TryParse(data, out result))
            {
                return result;
            }

            if (data.Equals("0", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("n", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("o", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("f", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("no", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("false", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return false;
            }

            if (data.Equals("1", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("y", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("x", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("t", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("yes", StringComparison.InvariantCultureIgnoreCase) 
                || data.Equals("true", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                return true;
            }

            return null;
        }

        /// <summary>
        /// Format the porvided data
        /// </summary>
        /// <param name="data">Data to be formatted.</param>
        /// <returns>Formatted data or the origonal if it could not be formatted.</returns>
        public string FormatData(string data)
        {
            bool? result = ParseData(data);

            if (null != result)
            {
                return ((bool)result).ToString();
            }

            // If we could not parse the data, then return the raw data
            return data;
        }

        /// <summary>
        /// Format the porvided data
        /// </summary>
        /// <param name="data">Data to be formatted.</param>
        /// <param name="format">Specifies the format to be returned as.</param>
        /// <returns>Formatted data or the origonal if it could not be formatted.</returns>
        public string FormatData(string data, string format)
        {
            bool? result = ParseData(data);

            if (null != result)
            {

                string trueString = true.ToString();
                string falseString = false.ToString();

                if (format.Contains('/') && false == format.IsNullOrEmpty())
                {
                    string[] output = format.Split('/');

                    trueString = output[0];
                    falseString = output[1];
                }

                return (bool)result ? trueString : falseString;

            }

            // If we could not parse the data, then return the raw data
            return data;
        }
    }
}
