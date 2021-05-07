using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using log4net;
using Afas.AfComply.Domain;
using Afas.Domain;

/// <summary>
/// Summary description for errorChecking
/// </summary>
public static class errorChecking
{
   





    public static ILog Log = LogManager.GetLogger(typeof(errorChecking));

    public static bool validateTextBoxEmail(TextBox _txt, bool _validData)
    {
        if (false == _txt.Text.IsValidEmail())
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            _txt.BackColor = System.Drawing.Color.White;
        }

        return _validData;
    }

    public static bool validateEmailOld(string _txt)
    {        
        string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        Regex re = new Regex(strRegex);

        if (false == re.IsMatch(_txt))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_txt"></param>
    /// <param name="_validData"></param>
    /// <returns></returns>
    public static bool validateTextBoxPhone(TextBox _txt, bool _validData)
    {       
        if (false == _txt.Text.IsValidPhoneNumber())
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            _txt.BackColor = System.Drawing.Color.White;
        }

        return _validData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_txt"></param>
    /// <param name="_validData"></param>
    /// <returns></returns>
    public static bool validateTextBoxZipCode(TextBox _txt, bool _validData)
    {
        if (false == _txt.Text.IsValidZipCode())
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            _txt.BackColor = System.Drawing.Color.White;
        }

        return _validData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_txt"></param>
    /// <param name="_validData"></param>
    /// <returns></returns>
    public static bool validateStringZipCode(string _txt, bool _validData)
    {
        if (false == _txt.IsValidZipCode())
        {
            _validData = false;
        }

        return _validData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_txt"></param>
    /// <param name="_validData"></param>
    /// <returns></returns>
    public static bool validateTextBoxSSN(TextBox _txt, bool _validData)
    {
        string strRegex = @"^\d{9}$";
        Regex re = new Regex(strRegex);

        if (false == re.IsMatch(_txt.Text) || _txt.Text=="000000000")
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            _txt.BackColor = System.Drawing.Color.White;
        }

        return _validData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_txt"></param>
    /// <param name="_validData"></param>
    /// <returns></returns>
    public static bool validateTextBoxEIN(TextBox _txt, bool _validData)
    {
        if (false == _txt.Text.IsValidFedId())
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            _txt.BackColor = System.Drawing.Color.White;
        }

        return _validData;
    }

    /// <summary>
    /// A textbox object and bool object are passed into this function. It will validate that the 
    /// count of characters is greater than 0. It will turn the textbox background color to red if 
    /// there is no data.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateTextBoxNull(TextBox _txt, bool _validData)
    {
        if (_txt.Text.IsNullOrEmpty())
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            _txt.BackColor = System.Drawing.Color.White;
        }

        return _validData;
    }

    /// <summary>
    /// A textbox object and bool object are passed into this function. It will validate that the 
    /// count of characters is greater than 0. It will turn the textbox background color to red if 
    /// there is no data.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateStringNull(string _txt, bool _validData)
    {
        if (_txt.Length < 1)
        {
            _validData = false;
        }

        return _validData;
    }

    /// <summary>
    /// A textbox object and bool object are passed into this function. It will validate that the 
    /// count of characters is greater than 0. It will turn the textbox background color to red if 
    /// there is no data.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateTextBoxDecimal(TextBox _txt, bool _validData)
    {
        try
        {
            System.Convert.ToDecimal(_txt.Text);
            _txt.BackColor = System.Drawing.Color.White;
        }
        catch (Exception exception)
        {
            Log.Warn("validateTextBoxDecimal caught an exception, suppressing.", exception);

            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        
        }

        return _validData;
    }

    /// <summary>
    /// A textbox object and bool object are passed into this function. It will validate that the 
    /// count of characters is greater than 0. It will turn the textbox background color to red if 
    /// there is no data.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateTextBoxDecimalACAHours(TextBox _txt, bool _validData)
    {
        decimal hours = 0;
        if (decimal.TryParse(_txt.Text, out hours) && hours > 0)
        {
            _txt.BackColor = System.Drawing.Color.White;
        }
        else
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }

        return _validData;
    }

    /// <summary>
    /// A textbox object and bool object are passed into this function. It will validate that the 
    /// count of characters is greater than 0. It will turn the textbox background color to red if 
    /// there is no data.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateTextBoxInteger(TextBox _txt, bool _validData)
    {
        try
        {
            System.Convert.ToInt32(_txt.Text);
            _txt.BackColor = System.Drawing.Color.White;
        }
        catch (Exception exception)
        {

            Log.Warn("validateTextBoxInteger caught an exception, suppressing.", exception);

            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;

        }

        return _validData;
    }


    /// <summary>
    /// Verify that a Textbox has atleast 6 characters.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateTextBox6Length(TextBox _txt, bool _validData)
    {
        if (false == _txt.Text.IsPasswordLengthValid())
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            _txt.BackColor = System.Drawing.Color.White;
        }

        return _validData;
    }



    /// <summary>
    /// Verify that a Textbox has min Length.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateTextBoxLength(TextBox _txt, bool _validData, int _minLength)
    {
        if (_txt.Text.Length < _minLength)
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            _txt.BackColor = System.Drawing.Color.White;
        }

        return _validData;
    }

    public static bool IsPasswordLengthValid(this String value)
    {
        return value.Length >= Authorization.MinPasswordLength;
    }

    /// <summary>
    /// Verify that a users password.
    /// 1) 6 to 15 characters.
    /// 2) Atleast 1 UPPERCASE letter.
    /// 3) Atleast 1 lowercase letter.
    /// 4) Atleast 1 numeric digit.
    /// 5) Atleast 2 special character.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateTextBoxPassword(TextBox _txt, TextBox _txt2, bool _validData)
    {
        if (false == _txt.Text.IsPasswordLengthValid() || false == _txt.Text.IsValidPasword())
        {
            _txt.BackColor = System.Drawing.Color.Red;
            _validData = false;
        }
        else
        {
            if (_txt.Text == _txt2.Text)
            {
                _txt.BackColor = System.Drawing.Color.White;
                _txt2.BackColor = System.Drawing.Color.White;
            }
            else
            {
                _validData = false;
                _txt.BackColor = System.Drawing.Color.Red;
                _txt2.BackColor = System.Drawing.Color.Red;
            }
        }

        return _validData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_txt"></param>
    /// <param name="_validData"></param>
    /// <returns></returns>
    public static bool validateTextBoxDate(TextBox _txt, bool _validData)
    {
        if (false == _txt.Text.IsValidDate())
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            _txt.BackColor = System.Drawing.Color.White;
        }

        return _validData;
    }

    /// <summary>
    /// Compare to make sure the 1st date is less than the 2nd date. 
    /// </summary>
    /// <param name="_txt"></param>
    /// <param name="_txt2"></param>
    /// <param name="_validData"></param>
    /// <returns></returns>
    public static bool validateTextBoxDateCompare(TextBox _txt, TextBox _txt2, bool _validData)
    {
        DateTime date1 = DateTime.Parse(_txt.Text);
        DateTime date2 = DateTime.Parse(_txt2.Text);

        if (date1 > date2)
        {
            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;
            _txt2.BackColor = System.Drawing.Color.Red;
        }

        return _validData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_txt"></param>
    /// <param name="_validData"></param>
    /// <returns></returns>
    public static bool validateStringDate(string _txt, bool _validData)
    {
        if (false == _txt.IsValidDate())
        {
            _validData = false;
        }

        return _validData;
    }

    /// <summary>
    /// A textbox object and bool object are passed into this function. It will validate that the 
    /// count of characters is greater than 0. It will turn the textbox background color to red if 
    /// there is no data.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateDropDownSelection(DropDownList _ddl, bool _validData)
    {
        try
        {
            if (_ddl == null || _ddl.SelectedItem == null || _ddl.SelectedItem.Text.Equals("Select"))
            {
                _ddl.BackColor = System.Drawing.Color.Red;
                _validData = false;
            }
            else
            {
                _ddl.BackColor = System.Drawing.Color.White;
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            _validData = false;
            _ddl.BackColor = System.Drawing.Color.Red;
        }
        return _validData;
    }

    /// <summary>
    /// A textbox object and bool object are passed into this function. It will validate that the 
    /// count of characters is greater than 0. It will turn the textbox background color to red if 
    /// there is no data.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateDropDownSelectionMonthlyStatus(DropDownList _ddl, bool _validData)
    {
        try
        {
            if (_ddl.SelectedItem.Text.Contains("Select"))
            {
                _ddl.BackColor = System.Drawing.Color.Red;
                _validData = false;
            }
            else
            {
                int statusID = int.Parse(_ddl.SelectedItem.Value);
                if (statusID == 7)
                {
                    _ddl.BackColor = System.Drawing.Color.Red;
                    _validData = false;
                }
                else
                {
                    _ddl.BackColor = System.Drawing.Color.White;
                }
            }
        }
        catch (Exception exception)
        {

            Log.Warn("validateDropDownSelectionMonthlyStatus caught an exception, suppressing.", exception);

            _validData = false;
            _ddl.BackColor = System.Drawing.Color.Red;

        }
        return _validData;
    }

    /// <summary>
    /// A textbox object and bool object are passed into this function. It will validate that the 
    /// count of characters is greater than 0. It will turn the textbox background color to red if 
    /// there is no data.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateDropDownSelectionLine14_1H_Line15_noValue(DropDownList _ddl, TextBox _txt, bool _validData)
    {
        try
        {
            string ashCode = _ddl.SelectedItem.Text;            

            if (ashCode.Contains("1H"))
            {
                double lcmp = double.Parse(_txt.Text);
                if(lcmp >= 0)
                {
                    _txt.BackColor = System.Drawing.Color.Red;
                    _validData = false;
                }
            }
            else
            {
                _txt.BackColor = System.Drawing.Color.White;
            }
        }
        catch (Exception exception)
        {

            Log.Warn("validateDropDownSelectionLine14_1H_Line15_noValue caught an exception, suppressing.", exception);

            _txt.BackColor = System.Drawing.Color.White;

        }

        return _validData;
    }

    /// <summary>
    /// If Line 14 contains (1B, 1C, 1D, 1E), than a dollar value must be entered in Line 15. 
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateDropDownSelectionLine15_Value(DropDownList _ddl, TextBox _txt, bool _validData)
    {
        string oneB = "1B";
        string oneC = "1C";
        string oneD = "1D";
        string oneE = "1E";

        try
        {
            string _ooc = _ddl.SelectedItem.Text;

            if (String.Compare(_ooc, oneB, true) == 0 || String.Compare(_ooc, oneC, true) == 0 || String.Compare(_ooc, oneD, true) == 0 || String.Compare(_ooc, oneE, true) == 0)
            {
                double lcmp = double.Parse(_txt.Text);

                if (lcmp >= 0)
                {
                    _txt.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    _txt.BackColor = System.Drawing.Color.Red;
                    _validData = false;
                }
            }
            else
            {
                _txt.BackColor = System.Drawing.Color.White;
            }
        }
        catch (Exception exception)
        {

            Log.Warn("validateDropDownSelectionLine15_Value caught an exception, suppressing.", exception);

            _validData = false;
            _txt.BackColor = System.Drawing.Color.Red;

        }

        return _validData;
    }

    /// <summary>
    /// A textbox object and bool object are passed into this function. It will validate that the 
    /// count of characters is greater than 0. It will turn the textbox background color to red if 
    /// there is no data.
    /// </summary>
    /// <param name="_txt"></param>
    /// <returns></returns>
    public static bool validateDropDownSelectionNoRed(DropDownList _ddl, bool _validData)
    {
        try
        {
            if (_ddl.SelectedItem.Text.Contains("Select"))
            {
                _validData = false;
            }
        }
        catch (Exception exception)
        {

            Log.Warn("validateDropDownSelectionNoRed caught an exception, suppressing.", exception);

            _validData = false;

        }

        return _validData;

    }



    /// <summary>
    /// This is meant to conver the imported datetime strings that come from the payroll/HR software. 
    /// </summary>
    /// <param name="_data"></param>
    /// <returns></returns>
    public static DateTime? convertDateTime(string _data)
    {
        string year = null;
        string day = null;
        string month = null;
        DateTime? data = null;

        if (_data.IsNullOrEmpty() || _data.Length < 8)
        {
            return null;
        }

        try
        {
            year = _data.Substring(0, 4);
            day = _data.Substring(6, 2);
            month = _data.Substring(4, 2);
            data = System.Convert.ToDateTime(month + "/" + day + "/" + year);
        }
        catch (Exception exception)
        {

            Log.Warn("convertDateTime caught an exception, suppressing.", exception);

            data = null;

        }

        return data;
    }

    public static decimal convertHours(string _hours)
    {
        string hours = null;
        string minutes = null;
        decimal ACTHours = 0;

        if (_hours == null)
        {
            return 0;
        }

        if ((_hours.Contains(".") || _hours.Length != 6) && decimal.TryParse(_hours, out ACTHours))
        {
            return ACTHours;
        }

        try
        {
            hours = _hours.Substring(0, 4);
            minutes = _hours.Substring(4, 2);

            ACTHours = System.Convert.ToDecimal(hours + "." + minutes);
        }
        catch (Exception exception)
        {

            Log.Warn("ConvertHours was unable to parse: " + _hours, exception);

            return 0;

        }

        return ACTHours;
    }

    public static string convertShortDate(string _date)
    {

        string convertedDate = null;
        try
        {
            string[] cdate = _date.Split('/');
            string month = cdate[0];
            string day = cdate[1];
            string year = cdate[2];
            bool validData = true;



            if (month.Length < 2)
            {
                month = "0" + month;
            }
            if (day.Length < 2)
            {
                day = "0" + day;
            }

            if (month.Length != 2)
            {
                validData = false;
            }

            if (day.Length != 2)
            {
                validData = false;
            }

            if (year.Length != 4)
            {
                validData = false;
            }

            if (validData == true)
            {
                convertedDate = year + month + day;
            }
            else
            {
                convertedDate = null;
            }
        }
        catch (Exception exception)
        {

            Log.Warn("convertShortDate caught an exception, suppressing.", exception);

            convertedDate = null;

        }


        return convertedDate;
    }

    public static string formatHours(string _value)
    {
        string convertedHours = null;
        string _hours = null; ;
        string _minutes = null;

        try
        {
            string[] hours = _value.Split('.');

            if (hours.Count() == 2)
            {
                if (hours[0].Length == 0)
                {
                    _hours = "0000";
                }
                else if (hours[0].Length == 1)
                {
                    _hours = "000" + hours[0].Trim();
                }
                else if (hours[0].Length == 2)
                { 
                    _hours = "00" + hours[0].Trim();
                }
                else if (hours[0].Length == 3)
                {
                    _hours = "0" + hours[0].Trim();
                }
                else if (hours[0].Length == 4)
                {
                    _hours = hours[0].Trim();
                }
                else 
                {
                    _hours = "0000";
                }

                if (hours[1].Length == 0)
                {
                    _minutes = "00";
                }
                else if (hours[1].Length == 1)
                {
                    _minutes = hours[1].Trim() + "0";
                }
                else if (hours[1].Length == 2)
                {
                    _minutes = hours[1].Trim();
                }
                else
                {
                    _minutes = "00";
                }
            }
            else if (hours.Count() == 1)
            {
                _minutes = "00";              
                if (hours[0].Length == 0)
                {
                    _hours = "0000";
                }
                else if (hours[0].Length == 1)
                {
                    _hours = "000" + hours[0].Trim();
                }
                else if (hours[0].Length == 2)
                {
                    _hours = "00" + hours[0].Trim();
                }
                else if (hours[0].Length == 3)
                {
                    _hours = "0" + hours[0].Trim();
                }
                else if (hours[0].Length == 4)
                {
                    _hours = hours[0].Trim();
                }
                else
                {
                    _hours = "0000";
                }
            }
            else
            {
                _hours = "0000";
                _minutes = "00";
            }

            convertedHours = _hours + _minutes;

            
        }
        catch (Exception exception)
        {

            Log.Warn("formatHours caught an exception, suppressing.", exception);

            convertedHours = "000000";

        }

        return convertedHours;
        

    }

    public static string formatSmartHrHours(string _value)
    {
        string convertedHours = null;

        _value = _value.Trim(' ');

        try
        {
            if (_value.Length == 0)
            {
                convertedHours = "000000";
            }
            else if (_value.Length == 1)
            {
                convertedHours = "00000" + _value;
            }
            else if (_value.Length == 2)
            {
                convertedHours = "0000" + _value;
            }
            else if (_value.Length == 3)
            {
                convertedHours = "000" + _value;
            }
            else if (_value.Length == 4)
            {
                convertedHours = "00" + _value;
            }
            else if (_value.Length == 5)
            {
                convertedHours = "0" + _value;
            }
            else
            {
                convertedHours = _value;
            }
        }
        catch (Exception exception)
        {

            Log.Warn("formatSmartHrHours caught an exception, suppressing.", exception);

            convertedHours = "000000";
        }

        return convertedHours;


    }

    public static bool validateDateTime(DateTime? _data)
    {
        bool valid = true;

        try
        {
            DateTime temp = (DateTime)_data;
            valid = false;
        }
        catch (Exception exception)
        {

            Log.Warn("validateDateTime caught an exception, suppressing.", exception);

            valid = true;

        }

        return valid;
    }



    public static void setDropDownList(DropDownList _ddl, int _id)
    {
        if (_id == 0)
        {
            _ddl.SelectedIndex = _ddl.Items.Count - 1;
        }
        else
        {
            ListItem li = _ddl.Items.FindByValue(_id.ToString());
            if (li != null)
            {
                _ddl.ClearSelection();
                li.Selected = true;
            }
        }
    }

    public static void setDropDownList(DropDownList _ddl, int? _id)
    {
        if (_id == null)
        {
            _ddl.SelectedIndex = _ddl.Items.Count - 1;
        }
        else
        {
            ListItem li = _ddl.Items.FindByValue(_id.ToString());
            if (li != null)
            {
                _ddl.ClearSelection();
                li.Selected = true;
            }
        }
    }

    public static void setDropDownList(DropDownList _ddl, bool _value)
    {
        if (_value == null)
        {
            _ddl.SelectedIndex = _ddl.Items.Count - 1;
        }
        else
        {
            string value = _value.ToString().ToLower();
            ListItem li = _ddl.Items.FindByValue(value);
            if (li != null)
            {
                _ddl.ClearSelection();
                li.Selected = true;
            }
        }
    }


    public static bool compareIntValueDropDownListNotEqual(DropDownList _ddl1, DropDownList _ddl2, bool _valid)
    {
        try
        {
            int py1 = int.Parse(_ddl1.SelectedItem.Value);
            int py2 = int.Parse(_ddl2.SelectedItem.Value);

            if (py1 == py2)
            {
                _ddl1.BackColor = System.Drawing.Color.Red;
                _ddl2.BackColor = System.Drawing.Color.Red;
                _valid = false;
            }
            else
            {
                _ddl1.BackColor = System.Drawing.Color.White;
                _ddl2.BackColor = System.Drawing.Color.White;
            }
        }
        catch (Exception exception)
        {

            Log.Warn("compareIntValueDropDownListNotEqual caught an exception, suppressing.", exception);

            _valid = false;
            _ddl1.BackColor = System.Drawing.Color.Red;
            _ddl2.BackColor = System.Drawing.Color.Red;

        }

        return _valid;
    }

    public static void setDropDownList(DropDownList _ddl, bool? _value)
    {
        if (_value == null)
        {
            _ddl.SelectedIndex = _ddl.Items.Count - 1;
        }
        else
        {
            string value = _value.ToString().ToLower();
            ListItem li = _ddl.Items.FindByValue(value);
            if (li != null)
            {
                _ddl.ClearSelection();
                li.Selected = true;
            }
        }
    }


    public static void setDropDownList(DropDownList _ddl, string _value)
    {
        if (_value == null)
        {
            _ddl.SelectedIndex = _ddl.Items.Count - 1;
        }
        else
        {
            ListItem myListItem = null;

            foreach (ListItem li in _ddl.Items)
            {
                if (String.Compare(li.Value, _value, true) == 0)
                {
                    myListItem = li;
                    break;
                }
            }

            if (myListItem != null)
            {
                _ddl.ClearSelection();
                myListItem.Selected = true;
            }
        }
    }

    public static void setDropDownListByText(DropDownList _ddl, string _value)
    {
        if (_value == null)
        {
            _ddl.SelectedIndex = _ddl.Items.Count - 1;
        }
        else
        {
            foreach (ListItem li in _ddl.Items)
            {
                if (String.Compare(li.Text, _value, true) == 0)
                {
                    _ddl.ClearSelection();
                    li.Selected = true;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// This will check to see if the OLD Plan Year is actually older than the New Plan Year ID.
    /// </summary>
    /// <param name="_oldPlanYearID"></param>
    /// <param name="_newPlanYearID"></param>
    /// <returns></returns>
    public static bool comparePlanYearSelectionOldNew(DropDownList ddl1, DropDownList ddl2, int _employerID, bool _validData)
    {
        try
        {
            int py1_id = int.Parse(ddl1.SelectedItem.Value);
            int py2_id = int.Parse(ddl2.SelectedItem.Value);

            PlanYear py1 = PlanYear_Controller.findPlanYear(py1_id, _employerID);
            PlanYear py2 = PlanYear_Controller.findPlanYear(py2_id, _employerID);

            if (py1.PLAN_YEAR_START < py2.PLAN_YEAR_START)
            {
                _validData = false;
                ddl1.BackColor = System.Drawing.Color.Red;
                ddl2.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                ddl1.BackColor = System.Drawing.Color.White;
                ddl2.BackColor = System.Drawing.Color.White;
            }
        }
        catch (Exception exception)
        {

            Log.Warn("comparePlanYearSelectionOldNew caught an exception, suppressing.", exception);

            _validData = false;
            ddl1.BackColor = System.Drawing.Color.Red;
            ddl2.BackColor = System.Drawing.Color.Red;

        }

        return _validData;
    }

    /// <summary>
    /// Validate the following business rules: 
    /// 5) _effectiveOn date must be greater than or equal to the _acceptedOn date. 
    /// 6) _effectiveOn date must be greater than or equal to the Measurement Period Stability Start Date. 
    /// 7) _effectiveOn date must be less than the Measurement Period Stability End Date. 
    /// </summary>
    /// <param name="_offeredOn">Depricated on 11/3/2017 TLW.</param>
    /// <param name="_mp">Measurement Object in question</param>
    /// <param name="_acceptedOn">Depricated on 11/3/2017 TLW</param>
    /// <param name="_effectiveDate">Date insurance goes into effect.</param>
    /// <param name="_validData">Return paramater for valid data.</param>
    /// <returns></returns>
    public static bool validateInsuranceOfferDates(DateTime _offeredOn, Measurement _mp, DateTime _acceptedOn, DateTime _effectiveOn, bool _validData, Label _LblMessage, DateTime _hireDate, TextBox _txtOfferedOn, TextBox _txtAcceptedOn, TextBox _txtEffectiveOn)
    {
        string message = "Incorrect Dates <br />";

        try
        {
            //************************************************************************************************************************************************************************************
            //************************************************************************************************************************************************************************************
            //************************************************************************************************************************************************************************************
            //************************************************************************************************************************************************************************************
            //************************************************************************************************************************************************************************************
            //************************************************************************************************************************************************************************************
            if (_effectiveOn < _hireDate)
            {
                _validData = false;
                _txtEffectiveOn.BackColor = System.Drawing.Color.Red;
                message = "The effective date of (" + _effectiveOn.ToShortDateString() + ") must be greater than or equal to the hire date of: " + _hireDate.ToShortDateString();
            }
            else if (false == (_effectiveOn >= _mp.MEASUREMENT_STAB_START))
            {
                _validData = false;
                _txtEffectiveOn.BackColor = System.Drawing.Color.Red;
                message += "The effective on date of (" + _effectiveOn.ToShortDateString() + ") should be greater than the Measurement Period Stability Start: " + _mp.MEASUREMENT_STAB_START.ToShortDateString();
            }
            else
            {
                if (false == (_effectiveOn < _mp.MEASUREMENT_STAB_END))
                {
                    _validData = false;
                    _txtEffectiveOn.BackColor = System.Drawing.Color.Red;
                    message += "The effective on date  of (" + _effectiveOn.ToShortDateString() + ") should be less than the Measurement Period Stability End: " + _mp.MEASUREMENT_STAB_END.ToShortDateString();
                }
                else
                {
                    _txtEffectiveOn.BackColor = System.Drawing.Color.White;
                }
            }
        }
        catch (Exception exception)
        {

            Log.Warn("validateInsuranceOfferDates caught an exception, suppressing.", exception);

            _validData = false;

        }

        _LblMessage.Text = message;

        return _validData;
    }

    /// <summary>
    /// Check to see if SELECT is the dropdownlist selection. 
    /// </summary>
    /// <param name="_ddl"></param>
    /// <returns></returns>
    public static string validateDropDownStringNULL(DropDownList _ddl)
    {

        String data = null;

        if (_ddl == null || _ddl.SelectedItem == null || _ddl.SelectedItem.Text.Contains("Select"))
        {
            return null;
        }

        data = _ddl.SelectedItem.Value.ToString();

        return data;

    }

    /// <summary>
    /// Check to see if SELECT is the dropdownlist selection. 
    /// </summary>
    /// <param name="_ddl"></param>
    /// <returns></returns>
    public static int validateDropDownIntNULL(DropDownList _ddl)
    {

        int data = 0;

        if (_ddl.SelectedItem.Text.Contains("Select"))
        {
            return 0;
        }

        int.TryParse(_ddl.SelectedItem.Value, out data);

        return data;

    }

    public static bool hasOriginalTaxYearSubmissionBeenSubmittedYet(int taxyear, int employerid)
    {
        bool originalSubmissionComplete = false;
        List<taxYearEmployerTransmission> tempList = airController.manufactureEmployerTransmissions(employerid, taxyear);

        foreach (taxYearEmployerTransmission tyet in tempList)
        {
            if (tyet.TransmissionType == "O" && tyet.ReceiptID != null)
            {
                originalSubmissionComplete = true;
                break;
            }
        }

        return originalSubmissionComplete;
    }

}