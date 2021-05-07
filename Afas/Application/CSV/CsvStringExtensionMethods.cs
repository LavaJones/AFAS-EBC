using System;
using System.Linq;
using System.Text.RegularExpressions;
using Afas.Domain;

namespace Afas.Application.CSV
{

    /// <summary>
    /// Domain specific extensions for the String class.
    /// </summary>
    public static class CsvStringExtensionMethods
    {

        /// <summary>
        /// If there is already an item in the string then it adds a , and space to allow adding another item to the list.
        /// </summary>
        /// <param name="value">The string to add the comma to.</param>
        /// <returns>The original string with a comma added or the string.</returns>
        public static string AddCommaIfNotEmpty(this string value)
        {
            
            if (false == value.IsNullOrEmpty()) { value += ", "; }

            return value;
        
        }

        /// <summary>
        /// If the line 14 Code is 1B, 1C, 1D, 1E, 1J or 1K then Line 15 must be greater than or equal to zero
        /// </summary>
        /// <param name="value">The Line 15 value</param>
        /// <param name="Line14Value">The Line 14 value</param>
        /// <returns>The Line 15 value, or if the One code matches then 0 if line 15 isn't a number</returns>
        public static String ContribForCode(this String value, String Line14Value)
        {
            if (Line14Value.IsNullOrEmpty())
            {
                return value.ReturnNullIfEmpty();
            }

            Line14Value = Line14Value.ToUpper();

            if (Line14Value == "1H")
            {
                return null;
            }

            if (Line14Value == "1B" || Line14Value == "1C" || Line14Value == "1D" || Line14Value == "1E" || Line14Value == "1J" || Line14Value == "1K" )
            {
                double contrib = 0.0;
                double.TryParse(value, out contrib);
                return contrib.ToString("F2");
            }

            return value ?? String.Empty;
        }

        public static String SafeHarborForCode(this String value, String Line14Value)
        {
            if (Line14Value.IsNullOrEmpty())
            {
                return value.ReturnNullIfEmpty();
            }
            
            return value ?? String.Empty;
        }


        public static String RemoveIllegalNameIRSCharacters(this String value)
        {
            if (value == null)
                return null;

            char[] BAD_CHARS = new char[] { '!', '@', '#', '$', '%', '_', '\"', '\'', '/', ',', '.', '`', '?','^','&','*','(',')','_','+','=','{','[',']','}',';',':','|' };

            value = string.Concat(value.Split(BAD_CHARS, StringSplitOptions.RemoveEmptyEntries)).Replace(@"\", "").Replace(@"/", "").Replace(@"""", "").Replace("\"", "");
            return System.Text.RegularExpressions.Regex.Replace(value, @"\s+", " ");
        
        }

        public static String RemoveIllegalBusinessIRSCharacters(this String value)
        {
            if (value == null)
                return null;

            char[] BAD_CHARS = new char[] { '!', '@', '#', '$', '%', '_', '\"', ',', '.', ';', '[', ']','*', '_', '(', ')', '+', '=', '{', '[', ']', '}', ';', ':', '|', '?', '/' };
            
            value = string.Concat(value.Split(BAD_CHARS, StringSplitOptions.RemoveEmptyEntries)).Replace(@"""", "").Replace("\"", "");
           return System.Text.RegularExpressions.Regex.Replace(value, @"\s+", " ");

        }

        public static String RemoveIllegalAddressIRSCharacters(this String value)
        {
            if (value == null)
                return null;

            char[] BAD_CHARS = new char[] { '!', '@', '#', '$', '%', '_', '\"', '\'', ',', '^', '.', ';', '[', ']', '&', '>', '<', '?', '`', '*', ':', '(', ')', '+','=','{','}', '|', };

            value = string.Concat(value.Split(BAD_CHARS, StringSplitOptions.RemoveEmptyEntries)).Replace(@"\", "").Replace(@"/", "").Replace(@"""", "").Replace("\"", "");
            value = System.Text.RegularExpressions.Regex.Replace(value, @"\s+", " ");
            return value;

        }

        public static String RemoveIllegalCityIRSCharacters(this String value)
        {
            if (value == null)
                return null;

            char[] BAD_CHARS = new char[] { '!', '@', '#', '$', '%', '_', '\"', '\'', ',', '.', ';', '[', ']', '&', '-', '>', '<', '?', '`', '*', ':', '(', ')', '+', '=', '{', '}', '|', ' ' };

            value = string.Concat(value.Split(BAD_CHARS, StringSplitOptions.RemoveEmptyEntries)).Replace(@"\", "").Replace(@"/", "").Replace(@"""", "").Replace("\"", "");
            value = System.Text.RegularExpressions.Regex.Replace(value, @"\s+", " ");
            return value;

        }

        /// <summary>
        /// Remove all of the leading and trailing ' and " from the value.
        /// </summary>
        public static String SanitizeHeader(this String value)
        {
            if (value == null)
            {
                return null;
            }

            return value.TrimSingleQuotes().TrimDoubleQuotes();
        }

    }

}
