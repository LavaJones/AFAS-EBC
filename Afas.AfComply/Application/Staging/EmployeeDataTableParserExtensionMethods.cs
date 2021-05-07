using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Afas.AfComply.Application.Staging
{
    /// <summary>
    /// Extension methods for parsing a data table of employee import data
    /// </summary>
    public static class EmployeeDataTableParserExtensionMethods
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static ILog Log = LogManager.GetLogger(typeof(EmployeeDataTableParserExtensionMethods));

        /// <summary>
        /// run internal checks to make sure that the code will function properly with the specified data
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>True if the validation passed, false if there was a failure</returns>
        public static bool ValidateEmloyeeValidation(this DataTable data)
        {
            //if the cumulative count of all three groups doesn't equal the inital count, then a row got droped or double counted
            int count = 0;
            count += data.SelectValidEmployeeRows().Rows.Count;
            count += data.SelectAlertEmployeeRows().Rows.Count;
            count += data.SelectInvalidEmployeeRows().Rows.Count;
            if (data.Rows.Count != count)
            {
                Log.Error(String.Format("Inital Data Count [{0}] does not match total count [{1}];\n Valid Count: [{2}], Alert Count: [{3}], Invalid Count: [{4}]",
                    data.Rows.Count, 
                    count, 
                    data.SelectValidEmployeeRows().Rows.Count, 
                    data.SelectAlertEmployeeRows().Rows.Count, 
                    data.SelectInvalidEmployeeRows().Rows.Count));

                return false;
            }
            
            

            throw new NotImplementedException();
            //return true;
        }

        /// <summary>
        /// Filters the Data Table to only the Employees that are Valid 
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>A Data Table of just the rows that are valid</returns>
        public static DataTable SelectValidEmployeeRows(this DataTable data) 
        {
            //A valid employee row is one that passes all checks and is ready to be upserted into the main Emplopyee table
            //Checks all rows in this data table and returns any that are not Alerts or Invalid

            return data.SelectEmployeeRowsByCriteria((row) => false == row.IsEmployeeRowAlert() && false == row.IsEmployeeRowInvalid());
        }

        /// <summary>
        /// Filters the Data Table to only the Employees that are Alert 
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>A Data Table of just the rows that are Alerts</returns>
        public static DataTable SelectAlertEmployeeRows(this DataTable data)
        {
            //Checks all rows in this data table and returns any that can be fixed

            return data.SelectEmployeeRowsByCriteria((row) => row.IsEmployeeRowAlert() && false == row.IsEmployeeRowInvalid());
        }

        /// <summary>
        /// Filters the Data Table to only the Employees that are Invalid 
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>A Data Table of just the rows that are invalid</returns>
        public static DataTable SelectInvalidEmployeeRows(this DataTable data)
        {
            //Checks all rows in this data table and returns any that are invalid

            return data.SelectEmployeeRowsByCriteria((row) => row.IsEmployeeRowInvalid());
        }

        /// <summary>
        /// Filters the Data Table to only the Employees that match the provided criteria
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <param name="Criteria">A Delegate that decides if the Row matches the critera or not</param>
        /// <returns>A Data Table of just the rows that match the Criteria (Func returned true)</returns>
        public static DataTable SelectEmployeeRowsByCriteria(this DataTable data, Func<DataRow, bool> Criteria)
        {
            //Checks all rows in this data table and returns any that are invalid
            DataTable NewTable = new DataTable();

            foreach (DataRow row in data.Rows)
            {
                if (Criteria(row))
                {
                    NewTable.Rows.Add(row);
                }
            }

            return NewTable;
        }

        #region These methods don't really add value
        /// <summary>
        /// Gets an XML string of just the Invalid employees from the Data Table
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>A string holding the XML representation of the DataTable of Invalid Employees</returns>
        public static string GetInvalidEmployeeXml(this DataTable data) 
        {
            // Write out the Invalid Employee Rows as XML 
            return data.SelectInvalidEmployeeRows().GetXmlString();
        }

        /// <summary>
        /// Gets an XML string of just the Alert employees from the Data Table
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>A string holding the XML representation of the DataTable of Alert Employees</returns>
        public static string GetAlertEmployeeXml(this DataTable data)
        {
            // Write out the Alert Employee Rows as XML 
            return data.SelectAlertEmployeeRows().GetXmlString();
        }

        /// <summary>
        /// Gets an XML string of just the Valid employees from the Data Table
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>A string holding the XML representation of the DataTable of Valid Employees</returns>
        public static string GetValidEmployeeXml(this DataTable data)
        {
            // Write out the Valid Employee Rows as XML 
            return data.SelectValidEmployeeRows().GetXmlString();
        }

        /// <summary>
        /// Gets an Sql XML object of just the Invalid employees from the Data Table
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>A string holding the XML representation of the DataTable of Invalid Employees</returns>
        public static SqlXml GetInvalidEmployeeSqlXml(this DataTable data)
        {
            // Write out the Invalid Employee Rows as XML 
            return data.SelectInvalidEmployeeRows().GetSqlXml();
        }

        /// <summary>
        /// Gets an Sql XML object of just the Alert employees from the Data Table
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>A string holding the XML representation of the DataTable of Alert Employees</returns>
        public static SqlXml GetAlertEmployeeSqlXml(this DataTable data)
        {
            // Write out the Alert Employee Rows as XML 
            return data.SelectAlertEmployeeRows().GetSqlXml();
        }

        /// <summary>
        /// Gets an Sql XML object of just the Valid employees from the Data Table
        /// </summary>
        /// <param name="data">The Table that this is being called on.</param>
        /// <returns>A string holding the XML representation of the DataTable of Valid Employees</returns>
        public static SqlXml GetValidEmployeeSqlXml(this DataTable data)
        {
            // Write out the Valid Employee Rows as XML 
            return data.SelectValidEmployeeRows().GetSqlXml();
        }
        #endregion

        /// <summary>
        /// Wrapper for WriteXml that returns it as a string including the schema
        /// </summary>
        /// <param name="data">The Table to write as XML</param>
        /// <returns>The table data and schema as XML</returns>
        public static string GetXmlString(this DataTable data) 
        {
            // Write out the table out as XML 
            using (StringWriter writer = new StringWriter())
            {
                data.WriteXml(writer, XmlWriteMode.WriteSchema);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Wrapper for WriteXml that returns it as a Sql Xml Object, including the schema
        /// </summary>
        /// <param name="data">The Table to write as XML</param>
        /// <returns>The table data and schema as SQL XML Object</returns>
        public static SqlXml GetSqlXml(this DataTable data)
        {
            // Write out the table out as XML 
            using (Stream stream = new MemoryStream())
            {
                data.WriteXml(stream, XmlWriteMode.WriteSchema);
                return new SqlXml(stream);
            }
        }

        //public static List<ImportEmployee> GetAlertEmployees(this DataTable data)
        //{
        //    List<ImportEmployee> results = List<ImportEmployee>();

        //    foreach (DataRow row in data.SelectAlertEmployeeRows())
        //    {
        //        results.Add(row.ConvertRowToImportEmployee());
        //    }

        //    return results;
        //}

        //public static List<Employee> GetValidEmployees(this DataTable data)
        //{
        //    List<Employee> results = List<Employee>();

        //    foreach (DataRow row in data.SelectValidEmployeeRows())
        //    {
        //        results.Add(row.ConvertRowToEmployee());
        //    }

        //    return results;
        //}

        //private static Employee ConvertRowToEmployee(this DataRow row) 
        //{
        //    //Take the data in the row and convert it to a full blown employee
        //    throw new NotImplementedException();
        //}

        //private static ImportEmployee ConvertRowToImportEmployee(this DataRow row)
        //{
        //    //Take the data in the row and convert it to an import employee
        //    throw new NotImplementedException();
        //}
    }
}
