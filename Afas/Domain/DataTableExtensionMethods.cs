using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Globalization;

using log4net;

using System.Reflection;
using System.IO;
using System.Collections;

namespace Afas.Domain
{
    
    public static class DataTableExtensionMethods
    {
        public static object OrDBNull(this object obj)
        {
            if (null == obj )
            {
                return DBNull.Value;
            }

            return obj;
        }

        public static object OrDBNullEmptyString(this object obj)
        {
            if (null == obj || obj.ToString() == string.Empty)
            {
                return DBNull.Value;
            }

            return obj;
        }

        public static void FormatDate(this DataTable dataTable, String columnName)
        {
            if (dataTable == null) 
            {
                Log.Warn("Extension Method FormatDate called on null object. Params: columnName: " + columnName);
                throw new ArgumentNullException("dataTable");
            }
            if (columnName == null)
            {
                Log.Warn("Called Extension Method FormatDate with null Param columnName.");
                throw new ArgumentNullException("columnName");
            }
            if (false == dataTable.Columns.Contains(columnName) || dataTable.Columns[columnName] == null)
            {                
                Log.Warn(String.Format("Extension Method FormatDate, Datatable does not contian Column Name: [{0}] ", columnName));
                throw new ArgumentException("Datatable does not contian Column Name: " + columnName);
            }

            String replacementColumnName = String.Format("{0}DEL", columnName);

            dataTable.Columns[columnName].ColumnName = replacementColumnName;
            dataTable.Columns.Add(columnName, typeof(String));       

            int errorRowIndex = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {

                if (String.IsNullOrEmpty(dataRow[replacementColumnName].ToString()))
                {
                    errorRowIndex++;
                    
                    continue;

                }

                DateTimeStyles styles = DateTimeStyles.None;
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                String[] formats =
                {
                    "yyyy/MM/dd",
                    "MM/dd/yyyy",
                    "M/d/yyyy",
                    "yyyyMMdd",
                    "yyyy-MM-dd",
                    "MM/dd/yyyy hh:mm:ss tt",
                    "MM/d/yyyy hh:mm:ss tt",
                    "MM/dd/yyyy h:mm:ss tt",
                    "MM/d/yyyy h:mm:ss tt",
                    "M/dd/yyyy hh:mm:ss tt",
                    "M/d/yyyy hh:mm:ss tt"
                };
                DateTime time;

                String value = dataRow[replacementColumnName].ToString().TrimDoubleQuotes();

                if (value == "0000-00-00")
                {
                    value = "1920-01-01";            
                }

                if (DateTime.TryParseExact(value, formats, culture, styles, out time))
                {
                    dataRow[columnName] = time.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new InvalidCastException(String.Format("Tried to cast to a date time but failed at {0}, row {1}, value '{2}'", columnName, errorRowIndex, value));
                }

                errorRowIndex++;

            }

            dataTable.Columns.Remove(replacementColumnName);

        }
        
        public static void RenameColumn(this DataTable dataTable, String oldName, String newName)
        {

            dataTable.Columns.AddColumnIfMissing(newName);
            
            foreach (DataRow dataRow in dataTable.Rows)
            {
                dataRow[newName] = dataRow[oldName];
            }

            dataTable.Columns.RemoveColumnIfExists(oldName);

        }

        public static void RenameConversionColumns(this DataTable dataTable, IDictionary<String, String> mappingDictionary)
        {

            foreach (DataColumn column in dataTable.Columns)
            {

                if (mappingDictionary.ContainsKey(column.ColumnName))
                {
                    column.ColumnName = mappingDictionary[column.ColumnName];
                }

            }

        }


        public static void ReorderColumns(this DataTable dataTable, params String[] columnNames)
        {

            for (int i = 0; i < columnNames.Length; i++)
            {

                if (Log.IsInfoEnabled) { Log.Info(String.Format("Moving {0} to position {1}.", columnNames[i], i)); }

                DataColumn dataColumn = dataTable.Columns[columnNames[i]];

                if (dataColumn == null)
                {
                    throw new Exception(String.Format("Column named {0} was not found in the collection.", columnNames[i]));
                }

                dataTable.Columns[columnNames[i]].SetOrdinal(i);

                if (Log.IsInfoEnabled) { Log.Info(String.Format("Moved {0}.", columnNames[i])); }

            }

        }

        public static Boolean VerifyContainsColumns(this DataTable dataTable, out String missingColumns, params String[] columnNames)
        {

            StringBuilder missingColumnNames = new StringBuilder();

            foreach(String columnName in columnNames)
            {

                if (Log.IsInfoEnabled) { Log.Info(String.Format("Verifying {0}.", columnName)); }

                if (dataTable.Columns.Contains(columnName) == false)
                {
                    missingColumnNames.AppendFormat("{0}, ", columnName);
                }

                if (Log.IsInfoEnabled) { Log.Info(String.Format("Verified {0}.", columnName)); }

            }

            missingColumns = missingColumnNames.ToString().Trim().Trim(',');

            if (Log.IsInfoEnabled) { Log.Info(String.Format("Missing columns: '{0}'.", missingColumns)); }

            return missingColumns.Length == 0;

        }
        
        public static void CleanDataTable(this DataTable dataTable)
        {
            dataTable.RemoveDoubleQuotes();
            dataTable.RemoveBackTicColumns();
        }

        public static void RemoveDoubleQuotes(this DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    row[column] = row[column]?.ToString()?.RemoveDoubleQuotes();
                }
            }
        }

        public static void RemoveBackTicColumns(this DataTable dataTable)
        {
            List<int> list = new List<int>();
            List<DataColumn> col = new List<DataColumn>();
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.ColumnName.StartsWith("`") && column.ColumnName.EndsWith("`"))
                {
                    col.Add(column);
                }
            }

            foreach (DataColumn column in col)
            {
                dataTable.Columns.Remove(column);
            }
        }

        public static void WriteOutCsv(this DataTable dataTable, string fileName, bool includeHeader = true)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                if (includeHeader)
                {
                    dataTable.WriteOutCsvHeaders(streamWriter);
                }

                dataTable.WriteOutCsvData(streamWriter);

            }

        }

        public static void WriteOutCsvHeaders(this DataTable dataTable, StreamWriter streamWriter) 
        {

            int columnCount = 0;

            foreach (DataColumn dataColumn in dataTable.Columns)
            {

                streamWriter.WriteCsvEscaped(dataColumn.ColumnName);

                columnCount++;

                if (columnCount < dataTable.Columns.Count)
                {
                    streamWriter.Write(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                }

            }

            streamWriter.Write(streamWriter.NewLine);

        }

        public static void WriteOutCsvData(this DataTable dataTable, StreamWriter streamWriter) 
        {
            int iColCount = dataTable.Columns.Count;
            foreach (DataRow dataRow in dataTable.Rows)
            {

                for (int i = 0; i < iColCount; i++)
                {

                    if (!Convert.IsDBNull(dataRow[i]))
                    {

                        streamWriter.WriteCsvEscaped(dataRow[i].ToString());

                    }

                    if (i < iColCount - 1)
                    {
                        streamWriter.Write(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                    }

                }

                streamWriter.Write(streamWriter.NewLine);

            }
        }

        public static string GetAsCsv(this DataTable dataTable, string Preheaders = null)
        {
            StringBuilder builder = new StringBuilder();

            if (null != Preheaders)
            {
                builder.AppendLine(Preheaders);
            }

            int columnCount = 0;
            foreach (DataColumn dataColumn in dataTable.Columns)
            {

                builder.Append(dataColumn.ColumnName.GetCsvEscaped());

                columnCount++;

                if (columnCount < dataTable.Columns.Count)
                {
                    builder.Append(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                }

            }

            builder.Append("\n");

            int iColCount = dataTable.Columns.Count;
            foreach (DataRow dataRow in dataTable.Rows)
            {

                for (int i = 0; i < iColCount; i++)
                {

                    if (!Convert.IsDBNull(dataRow[i]))
                    {

                        builder.Append(dataRow[i].ToString().GetCsvEscaped());

                    }

                    if (i < iColCount - 1)
                    {
                        builder.Append(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                    }

                }

                builder.Append("\n");

            }

            return builder.ToString();
        }
        
        public static Boolean RowIsNotBlank(this DataRow dataRow)
        {
            return dataRow.IsRowBlank() == false;
        }

        public static List<T> DataTableToList<T>(this DataTable table, IEnumerable<String> skipProperties = null) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    list.Add(row.DataRowToObject<T>(skipProperties));
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static T DataTableToObject<T>(this DataTable table, IEnumerable<String> skipProperties = null) where T : class, new()
        {
            try
            {
                var row = table.AsEnumerable().FirstOrDefault();
                return (row == null) ? null : row.DataRowToObject<T>(skipProperties);
            }
            catch
            {
                return null;
            }
        }

        public static T DataRowToObject<T>(this DataRow row, IEnumerable<String> skipProperties = null) where T : class, new()
        {
            T obj = new T();
            var currentType = typeof(T);

            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    if (skipProperties != null && skipProperties.Contains(prop.Name)) 
                    {
                        continue;
                    }

                    var value = row[prop.Name];
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);

                    if (propertyInfo.CanWrite)
                    {
                        if (value is DBNull) 
                        {
                            propertyInfo.SetValue(obj, null);
                        }
                        else if (propertyInfo.PropertyType.IsEnum)
                        {
                            propertyInfo.SetValue(obj, Enum.ToObject(propertyInfo.PropertyType, value));
                        }
                        else
                        {
                            propertyInfo.SetValue(obj, ChangeType(value, propertyInfo.PropertyType), null);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Log.Warn(string.Format("Exception during conversion from DataTable to List: {0} for Type {1} ", ex.Message, currentType.Name), ex);
                    continue;
                }
            }

            return obj;
        }

        private static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (Convert.IsDBNull(value))
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        internal static ILog Log = LogManager.GetLogger(typeof(DataTableExtensionMethods));

    }

}
