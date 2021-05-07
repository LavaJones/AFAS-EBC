using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using log4net;

public partial class admin_grosspay_tool : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_grosspay_tool));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;
        loadEmployers();
        loadGrossPayDescriptions(0);               
    }

    /*********************************************************************************************
     GROUP 1: All functions that load data into dropdown lists & gridviews. ****************** 
    *********************************************************************************************/
    /// <summary>
    /// 1-1) Load all existing employers into a dropdown list. 
    /// </summary>
    private void loadEmployers()
    {
        List<employer> tempList = employerController.getAllEmployers();

        DdlEmployer.DataSource = tempList;
        DdlEmployer.DataTextField = "EMPLOYER_NAME";
        DdlEmployer.DataValueField = "EMPLOYER_ID";
        DdlEmployer.DataBind();

        DdlEmployer.Items.Add("Select");
        DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
    }

    /// <summary>
    /// When a user selects an Employer, the system will call a function to load the Gross Pay Descriptions for that Employer.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            loadGrossPayDescriptions(_employerID);
            loadGridView();
        }
        else
        {
            DdlGp1.Items.Clear();
            DdlGp2.Items.Clear();
            GvGrossPayDescriptions.DataSource = null;
            GvGrossPayDescriptions.DataBind();
        }
    }

    /// <summary>
    /// Load all existing Gross Pay Descriptions for a specific Employer into both dropdownlists.
    /// </summary>
    /// <param name="_employerID"></param>
    private void loadGrossPayDescriptions(int _employerID)
    {
        List<gpType> tempList = gpType_Controller.getEmployeeTypes(_employerID);

        DdlGp1.DataSource = tempList;
        DdlGp1.DataTextField = "GROSS_PAY_DESCRIPTION_EXTERNAL_ID";
        DdlGp1.DataValueField = "GROSS_PAY_ID";
        DdlGp1.DataBind();
        DdlGp1.Items.Add("Select");
        DdlGp1.SelectedIndex = DdlGp1.Items.Count - 1;

        DdlGp2.DataSource = tempList;
        DdlGp2.DataTextField = "GROSS_PAY_DESCRIPTION_EXTERNAL_ID";
        DdlGp2.DataValueField = "GROSS_PAY_ID";
        DdlGp2.DataBind();
        DdlGp2.Items.Add("Select");
        DdlGp2.SelectedIndex = DdlGp2.Items.Count - 1;
    }

    private void loadGridView()
    {
        int employerID = 0;
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            List<gpType> tempList = gpType_Controller.getEmployeeTypes(employerID);

            GvGrossPayDescriptions.DataSource = tempList;
            GvGrossPayDescriptions.DataBind();

            LitShowing.Text = GvGrossPayDescriptions.Rows.Count.ToString();
            LitToal.Text = tempList.Count.ToString();
        }
        else
        {
        
        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }


    /// <summary>
    /// Change the Page Index of the Gridview and Re-Bind the data. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GvGrossPayDescriptions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvGrossPayDescriptions.PageIndex = e.NewPageIndex;
        loadGridView();
    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        bool validData = true;
        bool validTransaction = false;
        int _gp1 = 0;
        int _gp2 = 0;
        int _employerID = 0;

        validData = errorChecking.validateDropDownSelection(DdlGp1, validData);
        validData = errorChecking.validateDropDownSelection(DdlGp2, validData);

        if (validData == true)
        {
            _gp1 = int.Parse(DdlGp1.SelectedItem.Value);
            _gp2 = int.Parse(DdlGp2.SelectedItem.Value);
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            if (_gp1 != _gp2)
            {
                validTransaction = gpType_Controller.mergeGrossPayType(_gp1, _gp2);
                if (validTransaction == true)
                {
                    loadGrossPayDescriptions(_employerID);
                    loadGridView();
                    MpeWebMessage.Show();
                    LitMessage.Text = "The two Gross Pay Descriptions were SUCCESSFULLY merged.";
                }
                else
                {
                    MpeWebMessage.Show();
                    LitMessage.Text = "An ERROR occurred while trying to MERGE the two Gross Pay Descriptions, please try again.";
                }
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "Please do NOT select the same two Gross Pay Descriptions.";
            }
        }
        else
        {
            MpeWebMessage.Show();
            LitMessage.Text = "Please correct any field highlighted in RED.";
        }
    }
}