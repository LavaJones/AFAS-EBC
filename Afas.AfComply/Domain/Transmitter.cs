using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Afas.AfComply.Domain
{
    public static class Transmitter
    {

        public static Int32 Timeout
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Transmitter.Timeout"]);
            }
        }

        public static String TimestampDigestValue
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.TimestampDigestValue"];
            }
        }

        public static String ACABusinessHeaderDigestValue
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.ACABusinessHeaderDigestValue"];
            }
        }

        public static String ACABulkRequestTransmitterStatusDetailRequestDigestValue
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.ACABulkRequestTransmitterStatusDetailRequestDigestValue"];
            }
        }

        public static String SoftwareId
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.SoftwareId"];
            }
        }

        public static String KeyIdentifier
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.KeyIdentifier"];
            }
        }

        public static String TestFileCd
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.TestFileCd"];
            }
        }


        public static String TransmitterControlCode
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.TransmitterControlCode"];
            }
        }

        public static String EIN
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.EIN"];
            }
        }

        public static String BusinessNameLine1
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.BusinessNameLine1"];
            }
        }

        public static String City
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.City"];
            }
        }

        public static String State
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.State"];
            }
        }

        public static String Zip
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.Zip"];
            }
        }

        public static String PersonFirstNm
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.PersonFirstNm"];
            }
        }

        public static String PersonLastNm
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.PersonLastNm"];
            }
        }

        public static String ContactPhoneNum
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.ContactPhoneNum"];
            }
        }

        public static String VendorCd
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.VendorCd"];
            }
        }

        public static String VendorPersonFirstNm
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.VendorPersonFirstNm"];
            }
        }

        public static String VendorPersonLastNm
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.VendorPersonLastNm"];
            }
        }

        public static String VendorContactPhoneNum
        {
            get
            {
                return ConfigurationManager.AppSettings["Transmitter.VendorContactPhoneNum"];
            }
        }

        public static string UnescapeXMLValue(string xmlString)
        {
            if (xmlString == null)
                throw new ArgumentNullException("xmlString");

            return xmlString.Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&");
        }

        public static string EscapeXMLValue(string xmlString)
        {

            if (xmlString == null)
                throw new ArgumentNullException("xmlString");

            return xmlString.Replace("'","&apos;").Replace( "\"", "&quot;").Replace(">","&gt;").Replace( "<","&lt;").Replace( "&","&amp;");
        }



    }
}
