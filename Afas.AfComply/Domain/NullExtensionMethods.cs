using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain
{
    public static class NullExtensionMethods
    {
        private static ILog Log = LogManager.GetLogger(typeof(NullExtensionMethods));

        /// <summary>
        /// Converts an object to an int. 0 will Pass through  
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>An int (0 if cant parse)</returns>
        public static int checkIntNull(this object obj)
        {
            if (obj == null)
            {          
                return 0;
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
                    return value;
                }
                else
                {           
                    return 0;
                }
            }
        }

        /// <summary>
        /// Converts an object to an int. 0 will Pass through  
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>An int (0 if cant parse)</returns>
        public static int? checkNullableInt(this object obj)
        {
            if (obj == null)
            {          
                return null;
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
                    return value;
                }
                else
                {           
                    return null;
                }
            }
        }

        /// <summary>
        /// Converts a string to an int. 
        /// </summary>
        /// <param name="obj">The string to be converted</param>
        /// <returns>An int (0 if cant parse)</returns>
        public static int checkStringToIntNull(this string obj)
        {
            if (obj == null)
            {          
                return 0;
            }
            else
            {           
                int value = 0;
                if (int.TryParse(obj, out value))
                {
                    return value;
                }
                else
                {           
                    return 0;
                }
            }
        }

        /// <summary>
        /// Converts an object to a decimal. Null or parse failures become 0.
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A double</returns>
        public static double checkDoubleNull(this object obj)
        {
            if (obj == null)
            {          
                return 0.0;
            }
            else if (obj is double || obj is Double)
            {           
                return (double)obj;
            }
            else
            {           
                double value = 0.0;
                if (double.TryParse(obj.ToString(), out value))
                {
                    return value;
                }
                else
                {       
                    return 0.0;
                }
            }
        }

        /// <summary>
        /// Converts an object to a decimal. Null or parse failures become 0.
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A decimal</returns>
        public static decimal checkDecimalNull(this object obj)
        {
            if (obj == null)
            {          
                return 0;
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
                    return 0;
                }
            }
        }

        /// <summary>
        /// Converts an object to a nullable Decimal or Null. Allows 0 to pass through.
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>A Decimal or Null</returns>
        public static decimal? checkDecimalNull2(this object obj)
        {
            if (obj == null)
            {      
                return null;
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
                    return null;
                }
            }
        }

        /// <summary>
        /// Converts and Object to a string or null
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>string or null</returns>
        public static string checkStringNull(this object obj)
        {
            if (null == obj || obj.ToString() == string.Empty || obj.ToString() == "Select")
            {           
                return null;
            }
            else
            {      
                return obj.ToString();
            }
        }

        /// <summary>
        /// Converts an object to a nullable DateTime
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>The Date Time or Null</returns>
        public static DateTime? checkDateNull(this object obj)
        {
            if (null == obj || obj.ToString() == "")
            {         
                return null;
            }
            else
            {    
                try
                {
                    DateTime result = new DateTime();
                    if (obj is string && ConverterHelper.ParseDate((string)obj, out result))
                    {
                        return result;
                    }

                    return Convert.ToDateTime(obj);
                }
                catch
                {      
                    return null;
                }
            }
        }

        /// <summary>
        /// Converts the Object to a boolean
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>true or false (null/parse failure returns false)</returns>
        public static bool checkBoolNull(this object obj)
        {
            bool value = false;
            if (null != obj && bool.TryParse(obj.ToString(), out value))
            {     
                return value;
            }
            else
            {           
                return false;
            }
        }

        /// <summary>
        /// Converts the Object to a boolean, with additional logic where 1 == true
        /// </summary>
        /// <param name="obj">The object to convert</param>
        /// <returns>True or false (false on parse failure)</returns>
        public static bool checkBoolStringNull(this object obj)
        {
            if (null == obj || obj.ToString() == string.Empty)
            {           
                return false;
            }
            else if (obj is bool || obj is Boolean)
            {           
                return (bool)obj;
            }
            else
            {     
                if (String.Compare("1", obj.ToString(), true) == 0)
                {              
                    return true;
                }

                bool value;
                if (bool.TryParse(obj.ToString(), out value))
                {   
                    return value;
                }
                else
                {     
                    return false;
                }
            }
        }

        /// <summary>
        /// Converts The object to a nullable boolean
        /// </summary>
        /// <param name="obj">The Object to convert to a bool</param>
        /// <returns>true, false, or null if the parse failed</returns>
        public static bool? checkBoolNull2(this object obj)
        {
            if (null == obj || obj.ToString() == string.Empty)
            {           
                return null;
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
                else if (obj.ToString() == "0") { return false; }
                else if (obj.ToString() == "1") { return true; }
                else
                {     
                    return null;
                }
            }
        }

        /// <summary>
        /// Converts The object to a non nullable Guid
        /// </summary>
        /// <param name="obj">The Object to convert to a Guid</param>
        /// <returns>The Guid or the default Guid (00.....00)</returns>
        public static Guid checkGuidNull(this object obj)
        {

            if (null == obj || string.Empty == obj.ToString())
            {
                Log.Warn("GUID parsing failed, returning blank GUID");
                return new Guid();
            }
            else if (obj is Guid)
            {
                return (Guid)obj;
            }
            else
            {
                Guid value = new Guid();
                if (Guid.TryParse(obj.ToString(), out value))
                {
                    return value;
                }
                else
                {
                    Log.Warn("GUID parsing failed, returning blank GUID");
                    return new Guid();
                }
            }

            Log.Warn("GUID parsing failed, returning blank GUID");
            return new Guid();
        }

        /// <summary>
        /// This will output the string in ISO time or blank if the object is null
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String ToIsoTime(this DateTime? data) 
        {
            if (null == data)
            {
                return "";
            }
            else 
            {
                DateTime time = (DateTime)data;
                return time.ToString("yyyy-MM-ddTHH:mm:ss");
            }        
        }

        /// <summary>
        /// This will output the string in short date or blank if the object is null
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static String ToShortDate(this DateTime? data)
        {
            if (null == data)
            {
                return "";
            }
            else
            {
                DateTime time = (DateTime)data;
                return time.ToShortDateString();
            }
        }
    }
}
