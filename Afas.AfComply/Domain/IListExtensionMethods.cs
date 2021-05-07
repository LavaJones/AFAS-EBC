using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain
{
    public static class IListExtensionMethods
    {

        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            Type type = typeof(T);
            DataTable dataTable = new DataTable();
            try
            {
                
                var properties = type.GetProperties();
               
                foreach (PropertyInfo info in properties)
                {
                    dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
                }

                foreach (T entity in list)
                {
                    object[] values = new object[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        values[i] = properties[i].GetValue(entity);
                    }

                    dataTable.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Log.Warn(string.Format("Exception coverting IEnumerable of Type {0} to DataTable: ", type.Name), ex);
            }

            return dataTable;
        }

        internal static ILog Log = LogManager.GetLogger(typeof(DataTableExtensionMethods));
    }
}
