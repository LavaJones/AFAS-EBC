using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.DataFormatters.Implementation
{

    /// <summary>
    /// Formatter for values that can be converted into decimal numbers
    /// </summary>
    public class DecimalDataFormatter : IDataFormatter
    {

        /// <summary>
        /// Do the actual Data parsing here for reusability
        /// </summary>
        /// <param name="data">The data to be parsed</param>
        /// <returns>The double or null if unparseable.</returns>
        private double? ParseData(string data) 
        {
            double resultd = 0;

            if (double.TryParse(data, out resultd)) 
            {
                return resultd;
            }

            ////I'm not sure if it is possible to parse a float if double prase failed, this is me being thurogh
            //float resultf = 0;
            //if (float.TryParse(data, out resultf)) 
            //{
            //    return resultf;
            //}

            //int resulti = 0;
            //if(int.TryParse(data, out resulti))
            //{
            //    return resulti;
            //}

            return null;
        }

        /// <summary>
        /// Format the porvided data
        /// </summary>
        /// <param name="data">Data to be formatted.</param>
        /// <returns>Formatted data or the origonal if it could not be formatted.</returns>
        public string FormatData(string data)
        {
            double? result = ParseData(data);

            if (null != result) 
            {
                return ((double)result).ToString("F");
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
            double? result = ParseData(data);
           
            if (null != result)
            {
                return ((double)result).ToString(format);
            }

            // If we could not parse/format the data, then return the raw data
            return data;
        }
    }
}
