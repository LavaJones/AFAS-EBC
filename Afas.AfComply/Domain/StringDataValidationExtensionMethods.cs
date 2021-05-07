using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain
{
    public static class StringDataValidationExtensionMethods
    {
        ////QUICK TEMPLATE
        public static bool IsValidDate(this String value)
        {
            string strRegex = @"^((0?[13578]|10|12)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[01]?))(-|\/)((19)([2-9])(\d{1})|(20)([012])(\d{1})|([8901])(\d{1}))|(0?[2469]|11)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[0]?))(-|\/)((19)([2-9])(\d{1})|(20)([012])(\d{1})|([8901])(\d{1})))$";
            Regex re = new Regex(strRegex);

            return re.IsMatch(value);
        }

        public static bool IsValidPasword(this String value)
        {
            string strRegex = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.{6,15})(?=.*[@#$%^&+=_!*-]).*$";
            Regex re = new Regex(strRegex);

            return re.IsMatch(value);
        }

        public static bool IsValidFedId(this String value)
        {
            string strRegex = @"\d{2}-\d{7}$";
            Regex re = new Regex(strRegex);

            return re.IsMatch(value);
        }

        public static bool IsValidSsn(this String value)
        {
            if (value == "000000000")
            {
                return false;
            }
            else
            {
                string strRegex = @"^\d{9}$";
                Regex re = new Regex(strRegex);
                return re.IsMatch(value.Trim());
            }
        }

        public static string ZeroPadSsn(this String value)
        {
            if (value == null || value.IsNullOrEmpty())
            {
                return value;
            }

            return value.ZeroPad(9);
        }

        public static string ZeroPadZip(this String value)
        {
            return value.ZeroPad(5);
        }

        public static string ZeroPad(this String value, int length)
        {
            if (value != null)
            {
                return value.Trim().PadLeft(length, '0');
            }
            return null;
        }

        public static bool IsValidZipCode(this String value)
        {
            string strRegex = @"\d{5}(-\d{4})?$";
            Regex re = new Regex(strRegex);

            return re.IsMatch(value);
        }

        public static bool IsValidPhoneNumber(this String value)
        {
            string strRegex = @"^(?:\+?1[-. ]?)?\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            Regex re = new Regex(strRegex);

            return re.IsMatch(value);
        }

        public static bool IsValidEmail(this String email)
        {
            email = email.Trim();
            if (email.Contains(' ') || false == email.Contains('@') || false == email.Contains('.'))
            {
                return false;
            }

            string domain = email.Split('@').Last();
            if (null == domain || false == domain.Contains('.') || email.Split('@').Count() > 2
                || domain.Split('.').Last() == null || domain.Split('.').Last().Length < 2)
            {
                return false;
            }
            
            return true;
        }

        public static string TruncateLongString(this string str, int maxLength)
        {
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }


        public static bool? ToBoolean(this string BoolString)
        {

            if (String.IsNullOrEmpty(BoolString))
                return null;
            else if (BoolString == "1")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static int? ToInteger(this string IntString)
        {
            if (String.IsNullOrEmpty(IntString))
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(IntString);
            }
        }

        public static String Masked_SSN(this string value)
        {
            if (value == null)
            {
                value = String.Empty;
            }
            else
            {
                value = "*****" + value.Remove(0, 5);
            }

            return value;
        }

    }
}
