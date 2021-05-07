using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for NullHelper
/// </summary>
public class NullHelper
{
	public NullHelper()
	{
	}

    public object checkForDBNull(object obj)
    {
        if (obj == null)
        {
            return DBNull.Value;
        }
        else
        {
            if (obj.ToString() == "")
            {
                return DBNull.Value;
            }
            else
            {
                return obj;
            }
        }
    }

    public object checkIntDBNull(object obj)
    {
        if (obj == null)
        {
            return DBNull.Value;
        }
        else if(obj.ToString() == "0")
        {
            return DBNull.Value;
        }
        else
        {
            try
            {
                int value = (int)obj;
                return value;
            }
            catch
            {
                return DBNull.Value;
            }
        }
    }

    public int checkIntNull(object obj)
    {
        if (obj == null)
        {
            return 0;
        }
        else
        {
            try
            {
                int value = int.Parse(obj.ToString());
                return value;
            }
            catch
            {
                return 0;
            }
        }
    }

    public int checkStringToIntNull(string obj)
    {
        if (obj == null)
        {
            return 0;
        }
        else
        {
            try
            {
                int value = int.Parse(obj);
                return value;
            }
            catch
            {
                return 0;
            }
        }
    }

    public double checkDoubleNull(object obj)
    {
        if (obj == null)
        {
            return 0;
        }
        else
        {
            try
            {
                double value = System.Convert.ToDouble(obj);
                return value;
            }
            catch
            {
                return 0;
            }
        }
    }

    public decimal checkDecimalNull(object obj)
    {
        if (obj == null)
        {
            return 0;
        }
        else
        {
            try
            {
                decimal value = System.Convert.ToDecimal(obj);
                return value;
            }
            catch
            {
                return 0;
            }
        }
    }

    public decimal? checkDecimalNull2(object obj)
    {
        if (obj == null)
        {
            return null;
        }
        else
        {
            try
            {
                decimal value = System.Convert.ToDecimal(obj);
                return value;
            }
            catch
            {
                return null;
            }
        }
    }

    public object checkDecimalDBNull(object obj)
    {
        if (obj == null)
        {
            return DBNull.Value;
        }
        else if (obj.ToString() == "0")
        {
            return DBNull.Value;
        }
        else
        {
            try
            {
                decimal value = (decimal)obj;
                return value;
            }
            catch
            {
                return DBNull.Value;
            }
        }
    }

    /// <summary>
    /// Allows 0 to pass through.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public object checkDecimalDBNull2(object obj)
    {
        if (obj == null)
        {
            return DBNull.Value;
        }
        else
        {
            try
            {
                decimal value = (decimal)obj;
                return value;
            }
            catch
            {
                return DBNull.Value;
            }
        }
    }

    public object checkDoubleDBNull(object obj)
    {
        if (obj == null)
        {
            return DBNull.Value;
        }
        else if (obj.ToString() == "0")
        {
            return DBNull.Value;
        }
        else
        {
            try
            {
                double value = (double)obj;
                return value;
            }
            catch
            {
                return DBNull.Value;
            }
        }
    }

    public string checkStringNull(object obj)
    {
        string value = null;

        if (obj == null)
        {
            return value;
        }
        else
        {
            if (obj.ToString() == "" || obj.ToString() == "Select")
            {
                return value;
            }
            else
            {
                value = obj.ToString();
                return value;
            }
        }
    }


    public DateTime? checkDateNull(object obj)
    {
        DateTime? value = null;
        try
        {
            if (obj == null)
            {
                return value;
            }
            else
            {
                if (obj.ToString() == "")
                {
                    return value;
                }
                else
                {
                    value = Convert.ToDateTime(obj);
                    return value;
                }
            }
        }
        catch
        {
            return value;
        }
    }

    public object checkDateDBNull(object obj)
    {
        Object value = null;
        try
        {
            if (obj == null)
            {
                value = DBNull.Value;
            }
            else
            {
                if (obj.ToString() == "")
                {
                    value = DBNull.Value;
                }
                else
                {
                    value = Convert.ToDateTime(obj);
                }
            }
        }
        catch
        {
            value = DBNull.Value;
        }

        return value;
    }

    /// <summary>
    /// Returns true or false
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool checkBoolNull(object obj)
    {
        bool value = false;
        try
        {
            value = (bool)obj;
        }
        catch
        {
            value = false;
        }

        return value;
    }

    /// <summary>
    /// Returns true or false
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool checkBoolStringNull(object obj)
    {
        bool value = false;
        try
        {
            value = (bool)obj;
        }
        catch
        {
            try
            {
                if (String.Compare("1", obj.ToString(), true) == 0)
                {
                    value = true;
                }
                else
                {
                    value = false;
                }
            }
            catch
            {
                value = false;
            }
        }

        return value;
    }

    /// <summary>
    /// Returns true, false or null 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool? checkBoolNull2(object obj)
    {
        bool? value = false;
        try
        {
             if (obj == null)
             {
                 return null;
             }
             else
             {
                 value = (bool)obj;
             }
        }
        catch
        {
            value = null;
        }

        return value;
    }

     /// Returns true, false or null 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public object checkBoolDBNull(object obj)
    {
        object value = null;
        try
        {
             if (obj == null)
             {
                 return DBNull.Value;
             }
             else
             {
                 value = (bool)obj;
             }
        }
        catch
        {
            value = DBNull.Value;
        }

        return value;
    }

    /// <summary>
    /// Check to see if SELECT is the dropdownlist selection. 
    /// </summary>
    /// <param name="_ddl"></param>
    /// <returns></returns>
    public string validateDropDownStringNULL(DropDownList _ddl)
    {
        string data = null;

        try
        {
            if (_ddl.SelectedItem.Text.Contains("Select"))
            {
                data = null;
            }
            else
            {
                data = _ddl.SelectedItem.Value;
            }
        }
        catch
        {
            data = null;
        }
        return data;
    }

    /// <summary>
    /// Check to see if SELECT is the dropdownlist selection. 
    /// </summary>
    /// <param name="_ddl"></param>
    /// <returns></returns>
    public int validateDropDownIntNULL(DropDownList _ddl)
    {
        int data = 0;

        try
        {
            if (_ddl.SelectedItem.Text.Contains("Select"))
            {
                data = 0;
            }
            else
            {
                data = int.Parse(_ddl.SelectedItem.Value);
            }
        }
        catch
        {
            data = 0;
        }
        return data;
    }
}