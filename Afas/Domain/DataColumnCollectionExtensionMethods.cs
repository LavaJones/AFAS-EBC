using System;
using System.Collections.Generic;

using System.Data;

namespace Afas.Domain
{
    
    public static class DataColumnCollectionExtensionMethods
    {

        /// <summary>
        /// Adds the column with the request name and the typeof(String) if it does not exist in the collection.
        /// </summary>
        public static void AddColumnIfMissing(this DataColumnCollection dataColumnCollection, String columnName)
        {

            if (dataColumnCollection.Contains(columnName))
            {
                return;
            }

            dataColumnCollection.Add(columnName, typeof(String));
        
        }

        /// <summary>
        /// Determines if the month or year is present in the column names.
        /// </summary>
        public static Boolean CheckColumnForMonthOrYear(this DataColumnCollection dataColumnCollection, String monthOrYear)
        {
            int x = 0;
            foreach (DataColumn dataColumn in dataColumnCollection)
            {

      
                if (dataColumn.ColumnName.Length > 15)
                {
                    if (dataColumn.ColumnName.Substring(0, 7) == monthOrYear) { x = 1; break; }
                    if (dataColumn.ColumnName.Substring(8, 4) == monthOrYear) { x = 1; break; }
                }

            }

            if (x == 1)
            {
                return true;
            }
                
            return false;

        }

        /// <summary>
        /// Determine which year the dataTable is referencing based upon the column names.
        /// Defaults to 2015.
        /// </summary>
        public static String DetermineYearFromColumnNames(this DataColumnCollection dataColumnCollection)
        {

            String year = "2015";

            if (dataColumnCollection.CheckColumnForMonthOrYear("2013"))
            {
                year = "2013";
            }

            if (dataColumnCollection.CheckColumnForMonthOrYear("2014"))
            {
                year = "2014";
            }

            if (dataColumnCollection.CheckColumnForMonthOrYear("2015"))
            {
                year = "2015";
            }

            if (dataColumnCollection.CheckColumnForMonthOrYear("2016"))
            {
                year = "2016";
            }

            return year;

        }

        /// <summary>
        /// Removes the Column specified by columnName if it exists in the collection.
        /// </summary>
        public static void RemoveColumnIfExists(this DataColumnCollection dataColumnCollection, String columnName)
        {

            if (dataColumnCollection.Contains(columnName))
            {
                dataColumnCollection.Remove(columnName);
            }
        
        }

        /// <summary>
        /// Removes any column that is identified as a user defined column.
        /// </summary>
        public static void RemoveUserDefinedColumns(this DataColumnCollection dataColumnCollection)
        {

            IList<DataColumn> dataColumnsToRemove = new List<DataColumn>();
            foreach (DataColumn dataColumn in dataColumnCollection)
            {

                if (dataColumn.ColumnName.StartsWith("`") && dataColumn.ColumnName.EndsWith("`"))
                {
                    dataColumnsToRemove.Add(dataColumn);
                }
            
            }

            foreach (DataColumn column in dataColumnsToRemove)
            {
                dataColumnCollection.Remove(column);
            }

        }

    }

}
