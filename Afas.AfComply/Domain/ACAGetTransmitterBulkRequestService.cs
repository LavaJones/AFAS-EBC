using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Afas.AfComply.Domain
{
    public static class ACAGetTransmitterBulkRequestService
    {

        public static String TimestampDigestValue
        {
            get
            {
                return ConfigurationManager.AppSettings["TimestampDigestValue"];
            }
        }

        public static String ACABusinessHeaderDigestValue
        {
            get
            {
                return ConfigurationManager.AppSettings["ACABusinessHeaderDigestValue"];
            }
        }

        public static String ACABulkRequestTransmitterStatusDetailRequestDigestValue
        {
            get
            {
                return ConfigurationManager.AppSettings["ACABulkRequestTransmitterStatusDetailRequestDigestValue"];
            }
        }

        public static String SoftwareId
        {
            get
            {
                return ConfigurationManager.AppSettings["SoftwareId"];
            }
        }

        public static String KeyIdentifier
        {
            get
            {
                return ConfigurationManager.AppSettings["KeyIdentifier"];
            }
        }

        public static String TestFileCd
        {
            get
            {
                return ConfigurationManager.AppSettings["TestFileCd"];
            }
        }


        public static String TransmitterControlCode
        {
            get
            {
                return ConfigurationManager.AppSettings["TransmitterControlCode"];
            }
        }

    }
}
