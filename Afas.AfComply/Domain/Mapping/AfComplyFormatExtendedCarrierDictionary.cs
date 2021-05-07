using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.Mapping
{

    public static class AfComplyFormatExtendedCarrierDictionary
    {

        public static IDictionary<String, String> Map
        {

            get
            {

                if (AfComplyFormatExtendedCarrierDictionary.Mapping == null)
                {

                    lock (AfComplyFormatExtendedCarrierDictionary.LockingObject)
                    {

                        if (AfComplyFormatExtendedCarrierDictionary.Mapping != null)
                        {
                            return AfComplyFormatExtendedCarrierDictionary.Map;
                        }

                        AfComplyFormatExtendedCarrierDictionary.Mapping = new Dictionary<String, String>();

                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Middle Name", "DELETE1");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Suffix", "DELETE2");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Member", "InsuredMember");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Subscriber SSN", "SUBID");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Member SSN", "SSN");

                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Hire Date", "DELETE3");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Change Date", "DELETE4");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Rehire Date", "DELETE5");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Termination Date", "DELETE6");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Employee #", "DELETE7");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("HR Status Code", "DELETE8");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("HR Status Description", "DELETE9");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("ACA Status", "DELETE10");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Employee Class", "DELETE11");
                        AfComplyFormatExtendedCarrierDictionary.Mapping.Add("Employee Type", "DELETE12");

                    }

                }

                return AfComplyFormatExtendedCarrierDictionary.Mapping;

            }

        }

        private static Object LockingObject = new Object();

        private static IDictionary<String, String> Mapping;

    }

}
