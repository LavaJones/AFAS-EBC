using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Reflection;

public static class GenericExtensionClass
{

    /// <summary>
    /// The below methotd takes datatabe as parameter  and create collection of <T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static List<T> ToCollection<T>(this DataTable dt)
    {
        var lst = new List<T>();
        Type tClass = typeof(T);
        PropertyInfo[] pClass = tClass.GetProperties();
        List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
        T cn;
        foreach (DataRow item in dt.Rows)
        {
            cn = (T)Activator.CreateInstance(tClass);
            foreach (PropertyInfo pc in pClass)
            {

                try
                {
                    DataColumn d = dc.Find(c => c.ColumnName.ToUpper() == pc.Name.ToUpper());
                    if (d != null)
                    {
                        if (item[pc.Name] == DBNull.Value)
                        {
                            pc.SetValue(cn, null);
                        }
                        else
                        {
                            pc.SetValue(cn, item[pc.Name]);
                        }
                    }
                }
                catch(Exception ex)
                {                  
                    throw;
                }
            }
            lst.Add(cn);
        }
        return lst;
    }

    /// <summary>
    ///  The below methotd takes collection as parameter and create table.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static DataTable ConvertTo<T>(IList<T> list)
    {
        DataTable table = CreateTable<T>();
        Type entityType = typeof(T);
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

        foreach (T item in list)
        {
            DataRow row = table.NewRow();

            foreach (PropertyDescriptor prop in properties)
            {
                row[prop.Name] = prop.GetValue(item);
            }

            table.Rows.Add(row);
        }

        return table;
    }

    public static IList<T> ConvertTo<T>(IList<DataRow> rows)
    {
        IList<T> list = null;

        if (rows != null)
        {
            list = new List<T>();

            foreach (DataRow row in rows)
            {
                T item = CreateItem<T>(row);
                list.Add(item);
            }
        }

        return list;
    }

    public static IList<T> ConvertTo<T>(this DataTable table)
    {
        if (table == null)
        {
            return null;
        }

        List<DataRow> rows = new List<DataRow>();

        foreach (DataRow row in table.Rows)
        {
            rows.Add(row);
        }

        return ConvertTo<T>(rows);
    }

    public static T CreateItem<T>(DataRow row)
    {
        T obj = default(T);
        if (row != null)
        {
            obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns)
            {
                PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName.ToUpper());
                try
                {
                    object value = row[column.ColumnName];
                    prop.SetValue
                        (obj, value, null);
                }
                catch
                {
                    throw;
                }
            }
        }

        return obj;
    }

    public static DataTable CreateTable<T>()
    {
        Type entityType = typeof(T);
        DataTable table = new DataTable(entityType.Name);
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

        foreach (PropertyDescriptor prop in properties)
        {
            table.Columns.Add(prop.Name, prop.PropertyType);
        }

        return table;
    }
}




