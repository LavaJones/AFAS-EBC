using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using log4net;

public partial class admin_payroll_duplicate_checker : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_payroll_duplicate_checker));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;
        loadEmployers();
    }

    /*********************************************************************************************
     GROUP 1: All functions that load data into dropdown lists & gridviews. ****************** 
    *********************************************************************************************/
    /// <summary>
    /// 1-1) Load all existing employers into a dropdown list. 
    /// </summary>
    private void loadEmployers()
    {
        DdlEmployer.DataSource = employerController.getAllEmployers();
        DdlEmployer.DataTextField = "EMPLOYER_NAME";
        DdlEmployer.DataValueField = "EMPLOYER_ID";
        DdlEmployer.DataBind();

        DdlEmployer.Items.Add("Select");
        DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
    }

    private void loadPayRollData(int _employerID)
    {
        List<Payroll> dupPayroll = null;
        int _prevEmployerID = 0;

        try
        {
            _prevEmployerID = System.Convert.ToInt32(Session["employerPayroll"]);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            _prevEmployerID = 0;
        }

        if (Session["dupPayroll"] == null || _prevEmployerID != _employerID)
        {
            dupPayroll = Payroll_Controller.getEmployerDuplicatePayroll(_employerID);
            Session["dupPayroll"] = dupPayroll;
        }
        else
        {
            dupPayroll = (List<Payroll>)Session["dupPayroll"];
        }

        LitCount.Text = dupPayroll.Count.ToString();
        
        GvPayrollData.DataSource = dupPayroll;
        GvPayrollData.DataBind();

    }

    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;
        employer _employer = null;
        bool validData = true;
        GvPayrollData.PageIndex = 0;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            _employer = employerController.getEmployer(_employerID);

            loadPayRollData(_employerID);

            Session["employerPayroll"] = _employerID;
        }

    }


    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }


    protected void GvPayrollData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (DdlEmployer.SelectedItem.Text != "Select")
        {
            int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            GvPayrollData.PageIndex = e.NewPageIndex;
            loadPayRollData(_employerID);
        }
    }


    protected void GvPayrollData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortBy = e.SortExpression;
        string lastSortExpression = "";
        string lastSortDirection = "ASC";
        int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

        List<Payroll> tempList = (List<Payroll>)Session["dupPayroll"];

        if (Session["sortExp"] != null)
        {
            lastSortExpression = Session["sortExp"].ToString();
        }
        if (Session["sortDir"] != null)
        {
            lastSortDirection = Session["sortDir"].ToString();
        }

        try
        {
            if (e.SortExpression != lastSortExpression)
            {
                lastSortExpression = e.SortExpression;
                lastSortDirection = "ASC";
            }
            else
            {
                if (lastSortDirection == "ASC")
                {
                    lastSortExpression = e.SortExpression;
                    lastSortDirection = "DESC";
                }
                else
                {
                    lastSortExpression = e.SortExpression;
                    lastSortDirection = "ASC";
                }
            }

            switch (lastSortDirection)
            {
                case "ASC":
                    switch (e.SortExpression)
                    {
                        case "PAY_BATCH_ID":
                            tempList.Sort(delegate(Payroll x, Payroll y)
                            {
                                return x.PAY_BATCH_ID.CompareTo(y.PAY_BATCH_ID);
                            });
                            break;
                        case "PAY_HOURS":
                            tempList.Sort(delegate(Payroll x, Payroll y)
                            {
                                return x.PAY_HOURS.CompareTo(y.PAY_HOURS);
                            });
                            break;
                        case "PAY_SDATE":
                            tempList = tempList.OrderBy(o => o.PAY_SDATE).ToList();
                            break;
                        case "PAY_EDATE":
                            tempList = tempList.OrderBy(o => o.PAY_EDATE).ToList();
                            break;
                        case "PAY_CDATE":
                            tempList = tempList.OrderBy(o => o.PAY_CDATE).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                case "DESC":
                    tempList.Reverse();
                    break;
                default:
                    break;
            }

            Session["dupPayroll"] = tempList;
            Session["sortDir"] = lastSortDirection;
            Session["sortExp"] = lastSortExpression;
            loadPayRollData(_employerID);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

    }

    protected void BtnValidateRecords_Click(object sender, EventArgs e)
    {
        int _employerID = 0;
        int count = 0;
        List<Payroll> tempList = (List<Payroll>)Session["dupPayroll"];
        bool validation = true;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;

        validation = errorChecking.validateDropDownSelection(DdlEmployer, validation);

        if (validation == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            foreach (Payroll p in tempList)
            {
                bool valid = false;
                valid = Payroll_Controller.deletePayroll(p.ROW_ID, _modBy, _modOn);
                if (valid == true)
                {
                    count += 1;
                }
            }

            employerController.insertEmployerCalculation(_employerID);

            Session["dupPayroll"] = null;
            LitMessage.Text = count.ToString() + " duplicate payroll records have been removed from the software.";
            loadPayRollData(_employerID);
        }
    }
}