using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.Mapping
{

    /// <summary>
    /// Handles the column mappings/deleting/renames for the Payroll Files in the Legacy Format.
    /// </summary>
    public static class LegacyFormatPayrollDictionary
    {

        public static IDictionary<String, String> Map
        {

            get
            {

                if (LegacyFormatPayrollDictionary.Mapping == null)
                {

                    lock (LegacyFormatPayrollDictionary.LockingObject)
                    {

                        if (LegacyFormatPayrollDictionary.Mapping != null)
                        {
                            return LegacyFormatPayrollDictionary.Map;
                        }

                        LegacyFormatPayrollDictionary.Mapping = new Dictionary<String, String>();

                        LegacyFormatPayrollDictionary.Mapping.Add("CoFEIN", "DELETE1");
                        LegacyFormatPayrollDictionary.Mapping.Add("EIN", "SSN");
                        LegacyFormatPayrollDictionary.Mapping.Add("LastName", "Last_Name");
                        LegacyFormatPayrollDictionary.Mapping.Add("FirstName", "First_Name");
                        LegacyFormatPayrollDictionary.Mapping.Add("PayPeriodEndDate", "End Date");
                        LegacyFormatPayrollDictionary.Mapping.Add("HoursWorked", "ACA Hours");
                        LegacyFormatPayrollDictionary.Mapping.Add("loc", "DELETE2");
                        LegacyFormatPayrollDictionary.Mapping.Add("pcode", "DELETE3");
                        LegacyFormatPayrollDictionary.Mapping.Add("ppearn", "DELETE4");
                        LegacyFormatPayrollDictionary.Mapping.Add("hourly", "DELETE5");
                        LegacyFormatPayrollDictionary.Mapping.Add("annual", "DELETE6");

                    }

                }

                return LegacyFormatPayrollDictionary.Mapping;

            }

        }

        private static Object LockingObject = new Object();

        private static IDictionary<String, String> Mapping;

    }

}
