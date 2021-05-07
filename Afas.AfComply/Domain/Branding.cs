using System;
using System.Configuration;

namespace Afas.AfComply.Domain
{

    /// <summary>
    /// Helper class to handle all of the Branding deployment variables.
    /// </summary>
    public static class Branding
    {

        /// <summary>
        /// Address Attention To entry.
        /// </summary>
        public static String AddressAttentionLine
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.AddressAttn"];
            }
        }

        /// <summary>
        /// Street address in the complete form.
        /// </summary>
        public static String AddressStreet
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.AddressStreet"];
            }
        }

        /// <summary>
        /// City, State Zipcode line.
        /// </summary>
        public static String AddressCityState
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.AddressCityState"];
            }
        }

        /// <summary>
        /// This is the message that will be displayed to the user when they are automatically logged out
        /// </summary>
        public static String AutoLogoutMessage
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.AutoLogoutMessage"];
            }
        }

        /// <summary>
        /// Represents the Company's Legal name.
        /// </summary>
        public static String CompanyName
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.CompanyName"];
            }
        }

        public static String CompanyShortName
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.CompanyShortName"];
            }
        }

        public static String CompanyWebSite
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.CompanyWebSite"];
            }
        }

        public static String CopyrightYears
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.CopyrightYears"];
            }
        }

        /// <summary>
        /// Public email address for support questions.
        /// </summary>
        public static String EmailAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.EmailAddress"];
            }
        }

        public static String IrsDeadlineCertify
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.IrsDeadlineCertify"];
            }
        }

        public static String IrsDeadlineSetup
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.IrsDeadlineSetup"];
            }
        }

        public static String IrsDeadlineTransmit
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.IrsDeadlineTransmit"];
            }
        }

        public static Boolean IrsReprintFeeEnabled
        {
            get
            {
                return Boolean.Parse(ConfigurationManager.AppSettings["Branding.IrsReprintFeeEnabled"].ToString());
            }
        }

        public static Boolean Irs1094Enabled
        {
            get
            {
                return Boolean.Parse(ConfigurationManager.AppSettings["Branding.Irs1094Enabled"].ToString());
            }
        }

        public static String LogoUri
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.LogoUri"];
            }
        }

        public static String PhoneNumber
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.PhoneNumber"];
            }
        }

        public static String ProductName
        {
            get
            {
                return ConfigurationManager.AppSettings["Branding.ProductName"];
            }
        }

    }

}