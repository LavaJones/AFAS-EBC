using Afas.Application.CSV;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Afas.Domain;

namespace Afas.AfComply.Domain
{
    public static class DbNullExtensionMethods
    {
        private static ILog Log = LogManager.GetLogger(typeof(DbNullExtensionMethods));

        /// <summary>
        /// Converts Null Objects to DBNull Objects.
        /// </summary>
        /// <param name="obj">The Object to check</param>
        /// <returns>The Object or DBNull if the object is null or tostring == empty string</returns>
        public static object checkForDBNull(this object obj)
        {
            if (obj == null)
            {    
                return DBNull.Value;
            }
            else if (obj.ToString() == string.Empty)
            {
                return DBNull.Value;
            }
            return obj;
        }

        /*We added this method( convertYesAndNoToBool) specially for the offer file import, because our business team needs the values in Column H(OFFERED) 
         and Column I(Accepted) should accept "Yes" and "No" strings */
        /// <summary>
        /// converts the strings(Yes,No,TRUE,FALSE) to boolean values 
        /// </summary>
        /// <param name="obj"> the string which wants to convert to bool</param>
        /// <returns>true or false</returns>
        public static bool? convertYesAndNoToBool(this object obj)
        {
            if (null == obj || obj == string.Empty)
            {           
                return null;
            }

            if (obj is bool || obj is Boolean)
            {
                return (bool)obj;
            }
            string ConvertedObject = obj.ToString().ToLower().Trim();
            if (ConvertedObject.Equals("yes"))
            {
                return true;
            }
            if (ConvertedObject.Equals("no"))
            {
                return false;
            }
            bool value;
            if (bool.TryParse(ConvertedObject, out value))
            {   
                return value;
            }
            return null;
        }


        /// <summary>
        /// Converts an Object to an int. Warning: 0 will become Null
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A (non-zero) int or DBull</returns>
        public static object checkIntDBNull(this object obj)
        {
            if (null == obj)
            {    
                return DBNull.Value;
            }
            else if (obj.ToString() == "0")
            {
                return DBNull.Value;
            }
            else if (obj is int)
            {           
                return (int)obj;
            }
            else
            {             
                int value = 0;
                if (int.TryParse(obj.ToString(), out value))
                {
                    if (0 == value)
                    {      
                        return DBNull.Value;
                    }

                    return value;
                }
                else
                {       
                    return DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Converts an object to a double or DB Null. Warning: 0 will become DBNull
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A (non zero) decimal or DBNull</returns>
        public static object checkDecimalDBNull(this object obj)
        {
            if (obj == null)
            {    
                return DBNull.Value;
            }
            else if (obj.ToString() == "0")
            {
                return DBNull.Value;
            }
            else if (obj is decimal || obj is Decimal)
            {           
                return (decimal)obj;
            }
            else
            {             
                decimal value;
                if (decimal.TryParse(obj.ToString(), out value))
                {
                    if (0 == value)
                    {
                        return DBNull.Value;
                    }
                    return value;
                }
                else
                {       
                    return DBNull.Value;
                }
            }
        }


        /// <summary>
        /// Converts an object to a Decimal or DB Null. Allows 0 to pass through.
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A Decimal or DBNull</returns>
        public static object checkDecimalDBNull2(this object obj)
        {
            if (obj == null)
            {    
                return DBNull.Value;
            }
            else if (obj is decimal || obj is Decimal)
            {           
                return (decimal)obj;
            }
            else
            {             
                decimal value;
                if (decimal.TryParse(obj.ToString(), out value))
                {
                    return value;
                }
                else
                {       
                    return DBNull.Value;
                }
            }
        }


        /// <summary>
        /// Converts an object to a double or DB Null. Warning: 0 will become DBNull
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A (non zero) Double or DBNull</returns>
        public static object checkDoubleDBNull(this object obj)
        {
            if (obj == null)
            {    
                return DBNull.Value;
            }
            else if (obj.ToString() == "0")
            {          
                return DBNull.Value;
            }
            else if (obj is double || obj is Double)
            {           
                return (double)obj;
            }
            else
            {             
                decimal value;
                if (decimal.TryParse(obj.ToString(), out value))
                {
                    if (0 == value)
                    {
                        return DBNull.Value;
                    }
                    return value;
                }
                else
                {       
                    return DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Converts an object to a DataeTime or DBNull
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A DateTime or DBNull</returns>
        public static object checkDateDBNull(this object obj)
        {
            if (obj == null || obj.ToString() == "")
            {          
                return DBNull.Value;
            }
            else
            {    
                try
                {
                    return Convert.ToDateTime(obj);
                }
                catch
                {      
                    return DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Try Parse that returns null if the parse failes
        /// </summary>
        /// <param name="textDate">The text to parse to a date</param>
        /// <returns>The text as a date or Null</returns>
        public static DateTime? TryParseNullableDateTime(this string textDate)
        {
            DateTime? results = null;
            DateTime parse;
            if (textDate != null && DateTime.TryParse(textDate, out parse))
            {
                results = parse;
            }

            return results;
        }

        /// <summary>
        /// Converts the Object to a bool or DBNull
        /// </summary>
        /// <param name="obj">The Object to convert to a bool</param>
        /// <returns>true, false, or DBnull if the parse failed</returns>
        public static object checkBoolDBNull(this object obj)
        {
            if (null == obj || obj.ToString() == string.Empty)
            {           
                return DBNull.Value;
            }
            else if (obj is bool || obj is Boolean)
            {           
                return (bool)obj;
            }
            else
            {     
                bool value;
                if (bool.TryParse(obj.ToString(), out value))
                {   
                    return value;
                }
                else
                {     
                    return DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Parses the object to a date or DbNull if the date is null and throws an exception if the date is not parseable
        /// </summary>
        /// <param name="obj">The object to parse</param>
        /// <returns>The date string or DbNull</returns>
        public static object parseDateToShortStringWithDbNull(this object obj)
        {
            if (null == obj || obj.ToString().IsNullOrEmpty())
            {
                return DBNull.Value;
            }

            if (obj is DateTime || obj is DateTime?)
            {
                return ((DateTime)obj).ToString("yyyyMMdd");
            }

            string test = obj.ToString().Trim();
            DateTime results = new DateTime();
            if (
                DateTime.TryParseExact(test, "yyyyMMdd", CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out results)
                || DateTime.TryParse(test, out results))
            {
                return results.ToString("yyyyMMdd");
            }


            throw new FormatException("Failed to parse String to Date.");
        }

        /// <summary>
        /// Used to cut off extra text that exceeds the limits of the table
        /// </summary>
        /// <param name="text">THe string to possibly truncate</param>
        /// <param name="maxLength">The max length for the string</param>
        /// <returns>The shortened string, or the origonal if it is not too long.</returns>
        public static string TruncateLength(this string text, int maxLength)
        {

            if (null == text)
            {
                return null;
            }
            else if (text.Length <= maxLength)
            {
                return text;
            }
            else
            {
                return text.Substring(0, maxLength);
            }

        }

    }

}