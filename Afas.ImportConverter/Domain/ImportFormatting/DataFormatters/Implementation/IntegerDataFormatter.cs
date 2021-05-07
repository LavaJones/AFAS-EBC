using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.DataFormatters.Implementation
{

    /// <summary>
    /// Formatter for values that can be converted into whole numbers
    /// </summary>
    public class IntegerDataFormatter : IDataFormatter
    {

        /// <summary>
        /// Do the actual Data parsing here for reusability
        /// </summary>
        /// <param name="data">The data to be parsed</param>
        /// <returns>The double or null if unparseable.</returns>
        private int? ParseData(string data) 
        {
            int resulti = 0;
            if(int.TryParse(data, out resulti))
            {
                return resulti;
            }

            double resultd = 0;
            if (double.TryParse(data, out resultd)) 
            {
                return (int)resultd;
            }

            //I'm not sure if it is possible to parse a float if double prase failed, this is me being thurogh
            float resultf = 0;
            if (float.TryParse(data, out resultf)) 
            {
                return (int)resultf;
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
            int? result = ParseData(data);

            if (null != result) 
            {
                return ((int)result).ToString();
            }

            // If we could not parse/format the data, then return the raw data
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
            int? result = ParseData(data);
           
            if (null != result)
            {
                return ((int) result).ToString(format);
            }

            // If we could not parse/format the data, then return the raw data
            return data;
        }
    }
}
