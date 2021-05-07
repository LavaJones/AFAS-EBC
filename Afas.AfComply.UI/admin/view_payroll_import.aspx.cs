using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using log4net;

using Afas.AfComply.Domain;

public partial class admin_view_payroll_import : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_view_payroll_import));

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
        int tempID = 0;

        if (Session["tempID"] == null)
        {
            Session["tempID"] = int.Parse(DdlEmployer.SelectedItem.Value);
            Session["tempPayroll"] = Payroll_Controller.manufactureEmployerPayrollImportList(_employerID);
        }
        else
        {
            tempID = (int)Session["tempID"];
            if (_employerID != tempID)
            {
                Session["tempID"] = _employerID;
                Session["tempPayroll"] = Payroll_Controller.manufactureEmployerPayrollImportList(_employerID);
            }
        }
        GvPayrollData.DataSource = Session["tempPayroll"];
        GvPayrollData.DataBind();
    }

    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;
        employer _employer = null;

        GvPayrollData.PageIndex = 0;

        if (DdlEmployer.SelectedItem.Text != "Select")
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            _employer = employerController.getEmployer(_employerID);

            loadPayRollData(_employerID);
            loadFTPFiles(_employer);
        }
    }


    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvCurrentFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        GridViewRow row = GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hfFilePath = (HiddenField)row.FindControl("HfFilePath");
        employer _employer = employerController.getEmployer(_employerID);

        string filePath = hfFilePath.Value;

        if (System.IO.File.Exists(filePath))
        {
            try
            {
                new FileArchiverWrapper().ArchiveFile(filePath, _employer.ResourceId, "User Delete Payroll", _employer.EMPLOYER_ID);
                loadFTPFiles(_employer);
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
            }
        }
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

        List<Payroll_I> tempList = (List<Payroll_I>)Session["tempPayroll"];

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
                            tempList.Sort(delegate (Payroll_I x, Payroll_I y)
                            {
                                return x.PAY_BATCH_ID.CompareTo(y.PAY_BATCH_ID);
                            });
                            break;
                        case "PAY_F_NAME":
                            tempList.Sort(delegate (Payroll_I x, Payroll_I y)
                           {
                               return x.PAY_F_NAME.CompareTo(y.PAY_F_NAME);
                           });
                            break;
                        case "PAY_M_NAME":
                            tempList.Sort(delegate (Payroll_I x, Payroll_I y)
                           {
                               return x.PAY_M_NAME.CompareTo(y.PAY_M_NAME);
                           });
                            break;
                        case "PAY_L_NAME":
                            tempList.Sort(delegate (Payroll_I x, Payroll_I y)
                           {
                               return x.PAY_L_NAME.CompareTo(y.PAY_L_NAME);
                           });
                            break;
                        case "PAY_I_HOURS":
                            tempList.Sort(delegate (Payroll_I x, Payroll_I y)
                           {
                               return x.PAY_I_HOURS.CompareTo(y.PAY_I_HOURS);
                           });
                            break;
                        case "PAY_SDATE":
                            tempList = tempList.OrderBy(o => o.PAY_SDATE).ToList();
                            break;
                        case "PAY_EDATE":
                            tempList = tempList.OrderBy(o => o.PAY_EDATE).ToList();
                            break;
                        case "PAY_GP_DESC":
                            tempList.Sort(delegate (Payroll_I x, Payroll_I y)
                           {
                               return x.PAY_GP_DESC.CompareTo(y.PAY_GP_DESC);
                           });
                            break;
                        case "PAY_GP_EXT_ID":
                            tempList.Sort(delegate (Payroll_I x, Payroll_I y)
                           {
                               return x.PAY_GP_EXT_ID.CompareTo(y.PAY_GP_EXT_ID);
                           });
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

            Session["tempPayroll"] = tempList;
            Session["sortDir"] = lastSortDirection;
            Session["sortExp"] = lastSortExpression;
            loadPayRollData(_employerID);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        List<Payroll_I> tempList = (List<Payroll_I>)Session["tempPayroll"];
        generateTextFile(tempList);
        employer _emp = employerController.getEmployer(_employerID);
        loadFTPFiles(_emp);
    }


    private bool generateTextFile(List<Payroll_I> _tempList)
    {
        bool validData = true;
        string fileName = null;
        string currDate = errorChecking.convertShortDate(DateTime.Now.ToShortDateString());
        int count = GvCurrentFiles.Rows.Count + 1;
        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        bool success = false;
        string fullFilePath = null;
        employer emp = null;

        if (validData == true)
        {
            int _employerID2 = int.Parse(DdlEmployer.SelectedItem.Value);
            emp = employerController.getEmployer(_employerID2);

            fileName = Branding.ProductName.ToUpper() + "_EXPORT_" + emp.EMPLOYER_IMPORT_PAYROLL + "_" + count + "_" + currDate + ".txt";
            fullFilePath = Server.MapPath("..\\ftps\\export\\") + fileName;
            try
            {
                using (StreamWriter sw = File.CreateText(fullFilePath))
                {
                    string allEmpIds = string.Empty;

                    foreach (Payroll_I p in _tempList)
                    {
                        allEmpIds += p.PAY_EMPLOYEE_ID + ", ";

                        string line = p.PAY_F_NAME + ",";
                        line += p.PAY_M_NAME + ",";
                        line += p.PAY_L_NAME + ",";
                        line += p.PAY_I_HOURS + ",";
                        line += p.PAY_I_SDATE + ",";
                        line += p.PAY_I_EDATE + ",";
                        line += p.PAY_SSN + ",";
                        line += p.PAY_GP_DESC + ",";
                        line += p.PAY_GP_EXT_ID + ",";
                        line += p.PAY_I_CDATE + ",";
                        line += p.EMPLOYEE_EXT_ID;
                        sw.WriteLine(line);
                    }

                    PIILogger.LogPII(String.Format("Building Payroll Export File for User ID:[{0}] at IP: [{1}] including employees: [{2}]", ((User)Session["CurrentUser"]).User_ID, Request.UserHostAddress, allEmpIds));
                }
                success = true;
            }
            catch (Exception exception)
            {
                Log.Warn("Suppressing errors.", exception);
                success = false;
            }
        }

        return success;
    }


    /// <summary>
    /// 1-5) Loads a specific employer's EMPLOYEE DEMOGRAPHIC file import.
    /// </summary>
    /// <param name="_employer"></param>
    private void loadFTPFiles(employer _employer)
    {
        List<FileInfo> tempList = getGrossPayFiles();
        List<FileInfo> tempList2 = new List<FileInfo>();

        if (_employer.EMPLOYER_IMPORT_EMPLOYEE != null)
        {
            string fname = _employer.EMPLOYER_IMPORT_PAYROLL.ToLower();
            foreach (FileInfo fi in tempList)
            {
                string fname2 = fi.Name.ToLower();

                if (fname2.Contains(fname))
                {
                    tempList2.Add(fi);
                }
            }
        }

        GvCurrentFiles.DataSource = tempList2;
        GvCurrentFiles.DataBind();
    }

    /// <summary>
    /// 1-6) Creates a LIST of all files in the FTPS folder. 
    /// </summary>
    /// <returns></returns>
    private List<FileInfo> getGrossPayFiles()
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/export"));
        return directory.GetFiles().ToList<FileInfo>();
    }

    protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        GridViewRow row = GvCurrentFiles.Rows[e.RowIndex];
        HiddenField hfFilePath = (HiddenField)row.FindControl("HfFilePath");
        Label lblFileName = (Label)row.FindControl("LblFileName");

        PIILogger.LogPII(String.Format("Downloading payroll Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", lblFileName.Text, Request.UserHostAddress, LitUserName.Text));

        string appendText = "attachment; filename=" + lblFileName.Text;
        Response.ContentType = "file/text";
        Response.AppendHeader("Content-Disposition", appendText);
        Response.TransmitFile(hfFilePath.Value);
        Response.Flush();         
        Response.SuppressContent = true;                
        HttpContext.Current.ApplicationInstance.CompleteRequest();                      
        Response.End();
    }


    /// <summary>
    /// This function is used DELETE all current Payroll Alerts. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnDeletePayrollAlerts_Click(object sender, EventArgs e)
    {
        int _employerID = 0;
        bool transactionComplete = false;
        bool validData = true;
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            transactionComplete = alert_controller.deleteEmployerPayrollAlerts(_employerID, _modBy, _modOn);

            if (transactionComplete == true)
            {
                Session["tempID"] = null;
                loadPayRollData(_employerID);
                LitMessage.Text = "All payroll alerts have been deleted for this employer.";
                MpeWebMessage.Show();
            }
            else
            {
                LitMessage.Text = "An error occurred while DELETING the payroll alerts. Please try again.";
                MpeWebMessage.Show();
            }
        }
        else
        {
            LitMessage.Text = "Please select an employer.";
            MpeWebMessage.Show();
        }

    }
}