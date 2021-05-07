using System;

using System.Data;

using log4net;

namespace Afas.AfComply.Domain
{
    
    public static class IDataReaderExtensionMethods
    {

        public static Object GetFieldForNamedColumn(this IDataReader dataReader, String tableName, String columnName)
        {

            Boolean columnFound = false;
            int fieldCount = dataReader.FieldCount;

            for (int index = 0; index < fieldCount; index++)
            {

                if (dataReader.GetName(index).Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
                {
                    
                    columnFound = true;

                    break;
                
                }
            
            }

            if (columnFound == false)
            {
                if (Log.IsInfoEnabled) Log.Info(String.Format("Column {0} does not exist for data table derived from {1}.", columnName, tableName));
            }

            return dataReader[columnName] as Object ?? default(object);

        }

        private static ILog Log = LogManager.GetLogger(typeof(IDataReaderExtensionMethods));

    }

}
