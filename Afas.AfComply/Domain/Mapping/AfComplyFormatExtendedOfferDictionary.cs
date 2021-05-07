using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.Mapping
{

    public static class AfComplyFormatExtendedOfferDictionary
    {

        public static IDictionary<String, String> Map
        {

            get
            {

                if (AfComplyFormatExtendedOfferDictionary.Mapping == null)
                {

                    lock (AfComplyFormatExtendedOfferDictionary.LockingObject)
                    {

                        if (AfComplyFormatExtendedOfferDictionary.Mapping != null)
                        {
                            return AfComplyFormatExtendedOfferDictionary.Map;
                        }

                        AfComplyFormatExtendedOfferDictionary.Mapping = new Dictionary<String, String>();

                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("Hire Date", "DELETE3");
                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("Change Date", "DELETE4");
                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("Rehire Date", "DELETE5");
                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("Termination Date", "DELETE6");
                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("Employee #", "DELETE7");
                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("HR Status Code", "DELETE8");
                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("HR Status Description", "DELETE9");
                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("ACA Status", "DELETE10");
                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("Employee Class", "DELETE11");
                        AfComplyFormatExtendedOfferDictionary.Mapping.Add("Employee Type", "DELETE12");

                    }

                }

                return AfComplyFormatExtendedOfferDictionary.Mapping;

            }

        }

        private static Object LockingObject = new Object();

        private static IDictionary<String, String> Mapping;

    }

}
