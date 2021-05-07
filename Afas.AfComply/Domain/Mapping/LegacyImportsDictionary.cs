using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.Mapping
{

    public static class LegacyImportsDictionary
    {

        public static IDictionary<String, String> Map
        {
        
            get
            {

                if (LegacyImportsDictionary.Mapping == null)
                {

                    lock(LegacyImportsDictionary.LockingObject)
                    {

                        if (LegacyImportsDictionary.Mapping != null)
                        {
                            return LegacyImportsDictionary.Map;
                        }

                        LegacyImportsDictionary.Mapping = new Dictionary<String, String>();

                        LegacyImportsDictionary.Mapping.Add("CoFEIN", "DELETE");
                        LegacyImportsDictionary.Mapping.Add("Social", "SSN");
                        LegacyImportsDictionary.Mapping.Add("PayPeriodEndDate", "End Date");
                        LegacyImportsDictionary.Mapping.Add("HoursWorked", "ACA Hour");
                        LegacyImportsDictionary.Mapping.Add("Relationship", "DELETE1");
                        LegacyImportsDictionary.Mapping.Add("EnrollStatus", "CoverageStatus");
                        LegacyImportsDictionary.Mapping.Add("OfferDate", "DELETE3");
                        LegacyImportsDictionary.Mapping.Add("Dependent_EIN", "DepEIN");
                        LegacyImportsDictionary.Mapping.Add("EmpEIN", "SSN");
                        LegacyImportsDictionary.Mapping.Add("End Date", "EndDate");
                        LegacyImportsDictionary.Mapping.Add("Start Date", "StartDate");
                        LegacyImportsDictionary.Mapping.Add("Co_FEIN", "DELETE5");
                        LegacyImportsDictionary.Mapping.Add("Employee_Number", "Employee #");
                        LegacyImportsDictionary.Mapping.Add("Address_1", "Street Address");
                        LegacyImportsDictionary.Mapping.Add("State", "State Code");
                        LegacyImportsDictionary.Mapping.Add("Postal_Code", "Zip Code");
                        LegacyImportsDictionary.Mapping.Add("Hire_Date", "Hire Date");
                        LegacyImportsDictionary.Mapping.Add("Measurement_Group", "HR Status Description");
                        LegacyImportsDictionary.Mapping.Add("Employee_Type", "HR Status Code");
                        LegacyImportsDictionary.Mapping.Add("Term_Date", "Termination Date");
                        LegacyImportsDictionary.Mapping.Add("Salary", "DELETE7");
                        LegacyImportsDictionary.Mapping.Add("Pay_Rate", "DELETE8");
                        LegacyImportsDictionary.Mapping.Add("Employee_Group", "EEType");
                        LegacyImportsDictionary.Mapping.Add("BenefitElig", "DELETE10");
                        LegacyImportsDictionary.Mapping.Add("Pay_Period", "DELETE11");
                        LegacyImportsDictionary.Mapping.Add("Suffix", "DELETE12");
                        LegacyImportsDictionary.Mapping.Add("Country", "DELETE13");
                        LegacyImportsDictionary.Mapping.Add("'CRF1'", "DELETE14");
                        LegacyImportsDictionary.Mapping.Add("'CRF2'", "DELETE15");
                        LegacyImportsDictionary.Mapping.Add("'CRF3'", "DELETE16");
                        LegacyImportsDictionary.Mapping.Add("'CRF4'", "DELETE17");
                        LegacyImportsDictionary.Mapping.Add("'CRF5'", "DELETE18");
                        LegacyImportsDictionary.Mapping.Add("'CRF6'", "DELETE19");
                        LegacyImportsDictionary.Mapping.Add("'CRF7'", "DELETE20");
                        LegacyImportsDictionary.Mapping.Add("'CRF8'", "DELETE21");
                        LegacyImportsDictionary.Mapping.Add("'CRF9'", "DELETE22");
                        LegacyImportsDictionary.Mapping.Add("'CRF10'", "DELETE23");
                        LegacyImportsDictionary.Mapping.Add("EIN", "SSN");
                        LegacyImportsDictionary.Mapping.Add("Client_EEId", "Employee #");
                        LegacyImportsDictionary.Mapping.Add("HireDt", "Hire Date");
                        LegacyImportsDictionary.Mapping.Add("LastName", "Last_Name");
                        LegacyImportsDictionary.Mapping.Add("FirstName", "First_Name");
                        LegacyImportsDictionary.Mapping.Add("MiddleName", "Middle_Name");
                        LegacyImportsDictionary.Mapping.Add("PostalCode", "Zip Code");
                        LegacyImportsDictionary.Mapping.Add("Address2","DELETE25");
                        LegacyImportsDictionary.Mapping.Add("MeasurementGroup", "HR Status Description");
                        LegacyImportsDictionary.Mapping.Add("PayRate", "DELETE26");
                        LegacyImportsDictionary.Mapping.Add("PayPeriodName", "DELETE27");
                        LegacyImportsDictionary.Mapping.Add("Address1", "Street Address");
                        LegacyImportsDictionary.Mapping.Add("TermDt", "Termination Date");
                        LegacyImportsDictionary.Mapping.Add("EmployeeGroupName", "HR Status Code");

                        LegacyImportsDictionary.Mapping.Add("Employee_EIN", "SSN");

                    }

                }

                return LegacyImportsDictionary.Mapping;

            }
        
        }

        private static Object LockingObject = new Object();

        private static IDictionary<String, String> Mapping;

    }

}
