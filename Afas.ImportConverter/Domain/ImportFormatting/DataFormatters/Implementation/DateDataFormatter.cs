using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.DataFormatters.Implementation
{

    /// <summary>
    /// Standardised Date Formatter that is interchangeable with other formatters 
    /// </summary>
    public class DateDataFormatter : IDataFormatter
    {
        //nessisary for TryParseExact
        private static DateTimeStyles styles = DateTimeStyles.None;
        //nessisary for TryParseExact
        private static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
        
        /// <summary>
        /// All currently accepted Date formats
        /// </summary>
        private static String[] formats =
            {
                "yyyy/MM/dd",
                "MM/dd/yyyy",
                "M/d/yyyy",
                "MMddyyyy",
                "yyyyMMdd", 
                "yyyy-MM-dd",
                "MM/dd/yyyy hh:mm",
                "MM/dd/yyyy hh:mm:ss tt",
                "MM/d/yyyy hh:mm:ss tt",
                "MM/dd/yyyy h:mm:ss tt",
                "MM/d/yyyy h:mm:ss tt",
                "M/dd/yyyy hh:mm:ss tt",
                "M/d/yyyy hh:mm:ss tt"
            };

        /// <summary>
        /// Do the actual Data parsing here for reusability
        /// </summary>
        /// <param name="data">The data to be parsed</param>
        /// <returns>The date time or null if unparseable.</returns>
        private DateTime? ParseData(string data) 
        {
            DateTime result = new DateTime();
            //If we can exactly parse the data then we will accept it as a valid date
            if (DateTime.TryParseExact(data, formats, culture, styles, out result))
            {
                return result;
            }

            // Make sure that even if the parse works, that it is a reasonable date, 
            // we got some weird results because tryparse was overly agressive at fitting things to dates, like the number 16 became 'Jan 16th of 0001'.
            TimeSpan oneHundredyears = new TimeSpan(100 * 365, 0, 0, 0);
            if (DateTime.TryParse(data, out result)
                && result > DateTime.Now.Subtract(oneHundredyears)
                && result < DateTime.Now.Add(oneHundredyears))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Format the porvided data
        /// </summary>
        /// <param name="data">Data to be formatted.</param>
        /// <returns>Formatted data or the orrigonal if it could not be formatted.</returns>
        public String FormatData(String data)
        {
            DateTime? result = ParseData(data);

            if (result != null)
            {
                return ((DateTime) result).ToShortDateString();
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
        public String FormatData(String data, string format)
        {
            DateTime? result = ParseData(data);

            if (result != null)
            {
                return ((DateTime)result).ToString(format, culture);
            }         

            // If we could not parse/format the data, then return the raw data
            return data;
        }
    }
}
