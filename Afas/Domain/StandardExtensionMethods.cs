using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Afas.Domain
{

    /// <summary>
    /// This class contains the high level extension methods that are used everywhere.
    /// </summary>
    public static class StandardExtensionMethods
    {
        private static ILog Log = LogManager.GetLogger(typeof(StandardExtensionMethods));

        /// <summary>
        /// Extension that calls string IsNullOrEmpty
        /// </summary>
        /// <param name="value">The string to check if it is null or empty</param>
        /// <returns>True if the string is null or empty.</returns>
        public static bool IsNullOrEmpty(this String value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Remove any characters that should not be in a filename, replacing them with a Empty string or if it is white space an _
        /// These include . , : " ? ~ < > * | ' / \ \t \n \r 
        /// </summary>
        /// <param name="value">A filename without the file extension</param>
        /// <returns>A string with all problematic characters removed, including periods.</returns>
        public static string CleanFileName(this String value)
        {
            return value
                .Replace(" ", "_")
                .Replace("\t", "_")
                .Replace(".", string.Empty)
                .Replace(",", string.Empty)
                .Replace(":", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("?", string.Empty)
                .Replace("~", string.Empty)
                .Replace("<", string.Empty)
                .Replace(">", string.Empty)
                .Replace("*", string.Empty)
                .Replace("|", string.Empty)
                .Replace("'", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("/", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Trim()
                ;
        }

        /// <summary>
        /// Returns null if the string is blank or empty.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns> Null or the string.</returns>
        public static string ReturnNullIfEmpty(this String value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }

            return value;
        }

        /// <summary>
        /// Remove all of the extra ,'s from the begining and end of the value.
        /// </summary>
        public static String TrimCommas(this String value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Trim(',');
        }
        
        /// <summary>
        /// Remove all of the extra "'s from the begining and end of the value.
        /// </summary>
        public static String TrimDoubleQuotes(this String value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Trim('"');
        }

        /// <summary>
        /// Remove all of the extra ''s from the begining and end of the value.
        /// </summary>
        public static String TrimSingleQuotes(this String value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Trim('\'');
        }

        /// <summary>
        /// Remove all of the extra "'s from the value.
        /// </summary>
        public static String RemoveDoubleQuotes(this String value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Replace(@"""", String.Empty);
        }

        /// <summary>
        /// Remove all of the extra ''s from the value.
        /// </summary>
        public static String RemoveSingleQuotes(this String value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Replace(@"'", String.Empty);
        }

        /// <summary>
        /// Remove all of the extra ,'s from thevalue.
        /// </summary>
        public static String RemoveCommas(this String value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Replace(@",", String.Empty);
        }

        /// <summary>
        /// Remove double spaces if it should only be single spaces
        /// </summary>
        /// <param name="value">This value to remove doublespaces from</param>
        /// <returns>The string with double spaces removed</returns>
        public static String RemoveDoubleSpaces(this String value)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            value = regex.Replace(value, " ");
            return value;
        }

        /// <summary>
        /// Remove Numbers
        /// </summary>
        /// <param name="value">The string to have numbers removed from</param>
        /// <returns>The string with numbers removed.</returns>
        public static String RemoveNumbers(this String value)
        {
            if (value == null)
            {
                return null;
            }

            char[] BAD_CHARS = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            value = string.Concat(value.Split(BAD_CHARS, StringSplitOptions.RemoveEmptyEntries)).Replace(@"""", "").Replace("\"", "");
            return System.Text.RegularExpressions.Regex.Replace(value, @"\s+", " ");
        }

        /// <summary>
        /// Remove any Dashes from the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The value without the dashes</returns>
        public static String RemoveDashes(this String value)
        {
            if (value == null)
            {
                return null;
            }

            //return System.Text.RegularExpressions.Regex.Replace(value, "-", "");
            value = Regex.Replace(value, @"[^0-9]", "");
            return value;
        }

        /// <summary>
        /// Convert the nullable boolean to 2, 1 or 0 as text (2 is false, 1 is true, 0 is null, 3 is invalid (true+false))
        /// </summary>
        /// <param name="value">A boolean to convert to 2, 1 or 0 text</param>
        /// <returns>2 or 1 or 0 text</returns>
        public static String BoolToTwoOneZero(this bool? value)
        {

            if (value == null)
            {
                return "0";
            }
            else if (value.Value)
            {
                return "1";
            }
            else
            {
                return "2";
            }

        }       
         
        /// <summary>
        /// Convert the nullable boolean to 2, 1 or 0 as text (2 is false, 1 is true, 0 is null, 3 is invalid (true+false))
        /// </summary>
        /// <param name="value">A boolean to convert to 2, 1 or 0 text</param>
        /// <returns>2 or 1 or 0 text</returns>
        public static String BoolToTwoOneZero(this bool value)
        {

            if (value)
            {
                return "1";
            }
            else
            {
                return "2";
            }

        }

        /// <summary>
        /// Convert hte boolean to 1 or 0 as text
        /// </summary>
        /// <param name="value">A boolean to convert to 1 or 0 text</param>
        /// <returns>1 or 0 text</returns>
        public static String BoolToOneZero(this bool value)
        {

            if (value)
            {
                return "1";
            }
            else
            {
                return "0";
            }

        }

    }

}
