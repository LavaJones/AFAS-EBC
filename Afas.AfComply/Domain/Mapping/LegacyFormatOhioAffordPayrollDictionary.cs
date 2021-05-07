using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.Mapping
{

    /// <summary>
    /// Handles the column mappings/deleting/renames for the Ohio Afford Payroll Files in the Legacy Format.
    /// </summary>
    public static class LegacyFormatOhioAffordPayrollDictionary
    {

        public static IDictionary<String, String> Map
        {

            get
            {

                if (LegacyFormatOhioAffordPayrollDictionary.Mapping == null)
                {

                    lock (LegacyFormatOhioAffordPayrollDictionary.LockingObject)
                    {

                        if (LegacyFormatOhioAffordPayrollDictionary.Mapping != null)
                        {
                            return LegacyFormatOhioAffordPayrollDictionary.Map;
                        }

                        LegacyFormatOhioAffordPayrollDictionary.Mapping = new Dictionary<String, String>();

                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("EMPLOYEE_ID", "Employee #");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("TRUE_SSN", "SSN");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("EIN", "SSN");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("LastName", "Last_Name");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("LAST", "Last_Name");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("FirstName", "First_Name");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("FIRST", "First_Name");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("MI", "Middle_Name");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("START_DT", "Start Date");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("STOP_DT", "End Date");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("PayPeriodEndDate", "End Date");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("CoFEIN", "FEIN");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("TOTAL_HOURS", "ACA Hours");
                        LegacyFormatOhioAffordPayrollDictionary.Mapping.Add("HoursWorked", "ACA Hours");

                    }

                }

                return LegacyFormatOhioAffordPayrollDictionary.Mapping;

            }

        }

        private static Object LockingObject = new Object();

        private static IDictionary<String, String> Mapping;

    }

}
