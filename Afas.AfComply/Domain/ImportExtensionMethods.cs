using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain
{

    public static class ImportExtensionMethods
    {
        /// <summary>
        /// Checks if this string is a header row of the import file.
        /// </summary>
        public static Boolean IsHeaderRow(this String value)
        {

            String matchingValue = value.ToUpper();

            return
                (
                    matchingValue.Contains("ACCEPTED")
                    || matchingValue.Contains("ACCEPTED/DECLINED ON")
                    || matchingValue.Contains("ADDRESS1")
                    || matchingValue.Contains("ADDRESS_1")
                    || matchingValue.Contains("ADDRESS2")
                    || matchingValue.Contains("ADDRESS_2")
                    || matchingValue.Contains("AVG HOURS")
                    || matchingValue.Contains("CLASS ID")
                    || matchingValue.Contains("COFEIN")
                    || matchingValue.Contains("CO_FEIN")
                    || matchingValue.Contains("CONTRIBUTION ID")
                    || matchingValue.Contains("DEPEIN")
                    || matchingValue.Contains("EFFECTIVE DATE")
                    || matchingValue.Contains("EMPEIN")
                    || matchingValue.Contains("EMPLOYEE ID")
                    || matchingValue.Contains("EMPLOYEE_NUMBER")
                    || matchingValue.Contains("EMPLOYEE_TYPE")
                    || matchingValue.Contains("EMPLOYER ID")
                    || matchingValue.Contains("FOD ID")
                    || matchingValue.Contains("FIRSTNAME")
                    || matchingValue.Contains("FIRST_NAME")
                    || matchingValue.Contains("FIRST NAME")
                    || matchingValue.Contains("HOURSWORKED")
                    || matchingValue.Contains("HIRE_DATE")
                    || matchingValue.Contains("HRA-FLEX")
                    || matchingValue.Contains("INSURANCE ID")
                    || matchingValue.Contains("LASTNAME")
                    || matchingValue.Contains("LAST_NAME")
                    || matchingValue.Contains("LAST NAME")
                    || matchingValue.Contains("MEASUREMENT_GROUP")
                    || matchingValue.Contains("MEMBER")
                    || matchingValue.Contains("MIDDLENAME")
                    || matchingValue.Contains("MIDDLE_NAME")
                    || matchingValue.Contains("NAME")
                    || matchingValue.Contains("OFFERED")
                    || matchingValue.Contains("OFFERED ON")
                    || matchingValue.Contains("PAYPERIODENDDATE")
                    || matchingValue.Contains("PAYROLL ID")
                    || matchingValue.Contains("PLAN YEAR ID")
                    || matchingValue.Contains("POSTAL_CODE")
                    || matchingValue.Contains("ROW ID")
                    || matchingValue.Contains("SOCIAL")
                    || matchingValue.Contains("SUFFIX")
                    || matchingValue.Contains("SUBID")
                    || matchingValue.Contains("TERM_DATE")
                    || matchingValue.Contains("TOTAL")
                );
        
        }

        public static Boolean IsNotHeaderRow(this String value)
        {
            return (value.IsHeaderRow() == false);
        }

    }

}
