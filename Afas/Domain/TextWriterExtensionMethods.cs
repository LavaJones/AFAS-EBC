using System;

using System.IO;

namespace Afas.Domain
{
    
    /// <summary>
    /// Helper extension methods for the TextWriter derived classes.
    /// </summary>
    public static class TextWriterExtensionMethods
    {

        /// <summary>
        /// Ensure the value is devoid of double quotes and is wrapped in double quotes if it contains the , value.
        /// To write the column seperators use the bare .Write() method.
        /// </summary>
        public static void WriteCsvEscaped(this TextWriter textWriter, String value)
        {

            String escapedValue = String.Empty;

            String sanitizedValue = value.TrimDoubleQuotes();

            if (value.Contains(","))
            {
                escapedValue = String.Format("\"{0}\"", sanitizedValue);
            }
            else
            {
                escapedValue = sanitizedValue;
            }

            textWriter.Write(escapedValue);

        }

        /// <summary>
        /// Ensure the value is devoid of double quotes and is wrapped in double quotes if it contains the , value.
        /// To write the column seperators use the bare .Write() method.
        /// </summary>
        public static string GetCsvEscaped(this String value)
        {
            String escapedValue = String.Empty;

            if (String.IsNullOrEmpty(value))
            {
                return escapedValue;
            }

            String sanitizedValue = value.TrimDoubleQuotes();

            if (value.Contains(","))
            {
                escapedValue = String.Format("\"{0}\"", sanitizedValue);
            }
            else
            {
                escapedValue = sanitizedValue;
            }

            return escapedValue;
        }

    }

}
