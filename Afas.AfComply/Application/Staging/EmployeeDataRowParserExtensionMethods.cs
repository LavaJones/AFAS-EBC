using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.Application.Staging
{
    /// <summary>
    /// Extension methods for parsing a data row of employee import data
    /// </summary>
    public static class EmployeeDataRowParserExtensionMethods
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static ILog Log = LogManager.GetLogger(typeof(EmployeeDataTableParserExtensionMethods));

        /// <summary>
        /// run checks that return true if the employee in this row has irrecoverable data problems
        /// </summary>
        /// <param name="row">The Row that is being validated.</param>
        /// <returns>True if the data is irrecoverable</returns>
        public static bool IsEmployeeRowInvalid(this DataRow row)
        {
            //An Invalid Employee is one that's data is in such bad shape 
            //that it cannot be safely added into the import table
            //or that certain hard data checks failed

            //if the SSN is null or not valid format then it is invalid
            if (row.EmployeeSsn() == null || row.EmployeeSsn() == string.Empty)
            { return true; }

            //if first name and last name are null or blank it is invalid
            if (row.EmployeeFirstName().IsNullOrEmpty() && row.EmployeeMiddleName().IsNullOrEmpty() && row.EmployeeLastName().IsNullOrEmpty())
            { return true; }
            
            //if Measurement period and employee type are null?

            throw new NotImplementedException();
        }

        /// <summary>
        /// runs checks that return true if the employee in this row has recoverable data problems.
        /// </summary>
        /// <param name="row">The Row that is being validated.</param>
        /// <returns>True if the data has recoverable problems</returns>
        public static bool IsEmployeeRowAlert(this DataRow row)
        {

            throw new NotImplementedException();
        }

        public static string EmployeeFirstName(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeLastName(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeMiddleName(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeSsn(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeAddress(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeCity(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeState(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeZipCode(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeHireDate(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeTermDate(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeEmployeeType(this DataRow row)
        {
            throw new NotImplementedException();
        }
        public static string EmployeeMeasurmentGroup(this DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
