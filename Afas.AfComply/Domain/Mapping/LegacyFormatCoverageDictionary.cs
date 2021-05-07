using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.Mapping
{

    /// <summary>
    /// Handles the column mappings/deleting/renames for the Coverage Files in the Legacy Format.
    /// </summary>
    public static class LegacyFormatCoverageDictionary
    {

        public static IDictionary<String, String> Map
        {

            get
            {

                if (LegacyFormatCoverageDictionary.Mapping == null)
                {

                    lock (LegacyFormatCoverageDictionary.LockingObject)
                    {

                        if (LegacyFormatCoverageDictionary.Mapping != null)
                        {
                            return LegacyFormatCoverageDictionary.Map;
                        }

                        LegacyFormatCoverageDictionary.Mapping = new Dictionary<String, String>();

                        LegacyFormatCoverageDictionary.Mapping.Add("ACA Status", "DELETE4");
                        LegacyFormatCoverageDictionary.Mapping.Add("CoFEIN", "DELETE1");
                        LegacyFormatCoverageDictionary.Mapping.Add("Employee Class", "DELETE5");
                        LegacyFormatCoverageDictionary.Mapping.Add("Employee Type", "DELETE6");
                        LegacyFormatCoverageDictionary.Mapping.Add("HR Status Code", "DELETE2");
                        LegacyFormatCoverageDictionary.Mapping.Add("HR Status Description", "DELETE3");
                        LegacyFormatCoverageDictionary.Mapping.Add("Member Name", "CoveredName");
                        LegacyFormatCoverageDictionary.Mapping.Add("Name", "CoveredName");
                        LegacyFormatCoverageDictionary.Mapping.Add("Relationship", "InsuredMember");
                        LegacyFormatCoverageDictionary.Mapping.Add("EmpEIN", "Subscriber SSN");
                        LegacyFormatCoverageDictionary.Mapping.Add("DepEIN", "Dependent SSN");
                        LegacyFormatCoverageDictionary.Mapping.Add("OfferDate", "Offered On");
                        LegacyFormatCoverageDictionary.Mapping.Add("StartDate", "Coverage Date Start");
                        LegacyFormatCoverageDictionary.Mapping.Add("EndDate", "Coverage Date End");

                    }

                }

                return LegacyFormatCoverageDictionary.Mapping;

            }

        }

        private static Object LockingObject = new Object();

        private static IDictionary<String, String> Mapping;

    }

}
