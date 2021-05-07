using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class admin_batch_management : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_batch_management));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;

        DdlEmployer.DataSource = employerController.getAllEmployers();
        DdlEmployer.DataTextField = "EMPLOYER_NAME";
        DdlEmployer.DataValueField = "EMPLOYER_ID";
        DdlEmployer.DataBind();

        DdlEmployer.Items.Add("Select");
        DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    private void loadBatchData(int _employerID)
    {
        List<batch> batchList = employerController.manufactureBatchList(_employerID);
        GvBatchFiles.DataSource = batchList;
        GvBatchFiles.DataBind();
    }

    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            loadBatchData(_employerID);
        }
        else
        { 
        
        
        }
    }

    protected void GvBatchFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = GvBatchFiles.Rows[e.RowIndex];
        Label lblBatchID = (Label)row.FindControl("LblBatchID");
        string _modBy = LitUserName.Text;
        DateTime _modOn = DateTime.Now;
        int _batchID = int.Parse(lblBatchID.Text);
        int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
        try
        {
            Payroll_Controller.deleteImportedPayrollBatch(_batchID, _modBy, _modOn);
            EmployeeController.DeleteFailedBatch(_batchID);

            LitMessage.Text = "All files related to Batch ID " + _batchID.ToString() + " have been deleted.";
            MpeWebMessage.Show();
            loadBatchData(_employerID);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }
    }
}