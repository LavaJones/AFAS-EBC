using System;
using System.Text;

using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Afas.AfComply.Domain
{

    /// <summary>
    /// Collection of helper methods to translate from one format into another.
    /// </summary>
    public static class ConverterHelper
    {

        /// <summary>
        /// Employee numbers are required to import a record. Generate defaults based on the brandingName and employee values passed.
        /// </summary>
        public static String BuildDefaultEmployeeNumber(String brandingName, String firstName, String lastName, String socialSecurityNumber)
        {

            String employerNumber = String.Empty;
            String baseString = String.Format(
                    "{0}-{1}-{2}", 
                    firstName.ToLower(), 
                    lastName.ToLower(), 
                    socialSecurityNumber.ToLower().Replace("-", String.Empty)
                );

            byte[] rawBits;

            rawBits = new SHA256Managed().ComputeHash(new ASCIIEncoding().GetBytes(baseString));

            String hashValue = BitConverter.ToString(rawBits).Replace("-", "");

            employerNumber = String.Format("{0}-{1}", brandingName, hashValue);

            return employerNumber.Substring(0, 50).Trim();

        }

        /// <summary>
        /// Build a suitable HrStatusDescription and HrStatusId.
        /// </summary>
        /// <returns></returns>
        public static String BuildHrStatusDescription(String hrStatusId, String hrStatusDescription)
        {

            String newStatusDescription = String.Empty;

            newStatusDescription = String.Format("{0} - {1}", hrStatusId, hrStatusDescription);

            if (newStatusDescription.Length > 51)
            {
                newStatusDescription = newStatusDescription.Substring(0, 50).Trim();
            }

            return newStatusDescription;

        }

        /// <summary>
        /// Build a suitable HrStatusId based on the Hr Status Description that has a good chance of being unique, without collisions for a specific demographics file.
        /// </summary>
        /// <returns></returns>
        public static String BuildHrStatusId(String hrStatusId, String hrStatusDescription)
        {

            String newStatusId = String.Empty;

            byte[] rawBits;

            rawBits = new SHA256Managed().ComputeHash(new ASCIIEncoding().GetBytes(hrStatusDescription));

            String hashValue = BitConverter.ToString(rawBits).Replace("-", "");

            newStatusId = String.Format("{0}-{1}", hrStatusId, hashValue);

            return newStatusId.Substring(0, 20).Trim();

        }

        /// <summary>
        /// Builds a file system safe filename based on the HR Status Id and HR Status Description. Replaces invalid characters with the '_' symbol.
        /// It is _very_ dependent on the formatting from BuildHrStatusDescription.
        /// </summary>
        /// <returns></returns>
        public static String BuildSafeFilename(String hrStatusDescription)
        {

            String regexSearch = new String(Path.GetInvalidFileNameChars()) + new String(Path.GetInvalidPathChars());

            Regex regex = new Regex(String.Format("[{0}]", Regex.Escape(regexSearch)));

            return regex.Replace(hrStatusDescription, "_");

        }

        /// <summary>
        /// Make the best effort to parse the passed String into a DateTime.
        /// If the return value is False the parsedDate is set to January 1st, 1920.
        /// </summary>
        public static Boolean ParseDate(String dateTime, out DateTime parsedDate)
        {

            DateTimeStyles styles = DateTimeStyles.None;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            String[] formats =
            {
                "yyyy/MM/dd",
                "MM/dd/yyyy",
                "M/d/yyyy",
                "yyyyMMdd",
                "yyyy-MM-dd",
                "MM/dd/yyyy hh:mm:ss tt",
                "MM/d/yyyy hh:mm:ss tt",
                "MM/dd/yyyy h:mm:ss tt",
                "MM/d/yyyy h:mm:ss tt",
                "M/dd/yyyy hh:mm:ss tt",
                "M/d/yyyy hh:mm:ss tt",
                "MM/dd/yyyy hh:mm",
            };

            parsedDate = DateTime.Parse("1920-01-01");

            DateTime time;

            String possibleDateTime = dateTime;

            if (dateTime == "0000-00-00")
            {
                return false;
            }

            if (DateTime.TryParseExact(possibleDateTime, formats, culture, styles, out time))
            {
                
                parsedDate = time;

                return true;

            }

            return false;

        }

    }

}
