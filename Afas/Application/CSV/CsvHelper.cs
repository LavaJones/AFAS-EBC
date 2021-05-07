using System;
using System.Linq;

namespace Afas.Application.CSV
{

    /// <summary>
    /// Collection of utility and helper methods for dealing with raw csv files.
    /// Should/Could be replaced with a proper 3rd party library.
    /// </summary>
    public static class CsvHelper
    {

        /// <summary>
        /// Ensures the Headers contain safe and usable values removing all known CSV encoding oddities.
        /// </summary>
        public static String[] SanitizeHeaderValues(String[] headers)
        {


            for (int index = 0; index < headers.Count(); index++)
            {
                headers[index] = headers[index].SanitizeHeader();
            }

            return headers;

        }

    }

}
