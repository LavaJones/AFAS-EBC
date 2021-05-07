using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.Mapping
{

    /// <summary>
    /// Handles the column mappings/deleting/renames for the Demographics Files in the Legacy Format.
    /// </summary>
    public static class LegacyFormatDemographicsDictionary
    {

        public static IDictionary<String, String> Map
        {

            get
            {

                if (LegacyFormatDemographicsDictionary.Mapping == null)
                {

                    lock (LegacyFormatDemographicsDictionary.LockingObject)
                    {

                        if (LegacyFormatDemographicsDictionary.Mapping != null)
                        {
                            return LegacyFormatDemographicsDictionary.Map;
                        }

                        LegacyFormatDemographicsDictionary.Mapping = new Dictionary<String, String>();

                        LegacyFormatDemographicsDictionary.Mapping.Add("CoFEIN", "DELETE1");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Client_EEId", "Employee #");
                        LegacyFormatDemographicsDictionary.Mapping.Add("EIN", "SSN");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Employee_Number", "Employee #");
                        LegacyFormatDemographicsDictionary.Mapping.Add("LastName", "Last_Name");
                        LegacyFormatDemographicsDictionary.Mapping.Add("FirstName", "First_Name");
                        LegacyFormatDemographicsDictionary.Mapping.Add("MiddleName", "Middle_Name");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Suffix", "DELETE2");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Address1", "Street Address");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Address_1", "Street Address");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Address2", "DELETE3");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Address_2", "DELETE9");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Social", "SSN");
                        LegacyFormatDemographicsDictionary.Mapping.Add("State", "State Code");
                        LegacyFormatDemographicsDictionary.Mapping.Add("PostalCode", "Zip Code");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Postal_Code", "Zip Code");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Country", "DELETE4");
                        LegacyFormatDemographicsDictionary.Mapping.Add("HireDt", "Hire Date");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Hire_Date", "Hire Date");
                        LegacyFormatDemographicsDictionary.Mapping.Add("TermDt", "Termination Date");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Term_Date", "Termination Date");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Employee_Type", "HR Status Code");
                        LegacyFormatDemographicsDictionary.Mapping.Add("EmpType", "HR Status Code");
                        LegacyFormatDemographicsDictionary.Mapping.Add("MeasurementGroup", "HR Status Description");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Measurement_Group", "HR Status Description");
                        LegacyFormatDemographicsDictionary.Mapping.Add("Salary", "DELETE5");
                        LegacyFormatDemographicsDictionary.Mapping.Add("PayRate", "DELETE6");
                        LegacyFormatDemographicsDictionary.Mapping.Add("PayPeriodName", "DELETE7");
                        LegacyFormatDemographicsDictionary.Mapping.Add("EmployeeGroupName", "DELETE8");

                    }

                }

                return LegacyFormatDemographicsDictionary.Mapping;

            }

        }

        private static Object LockingObject = new Object();

        private static IDictionary<String, String> Mapping;

    }

}
