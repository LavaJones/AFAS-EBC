using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;

public partial class step2 : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(step2));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        if (null == employer || false == employer.IrsEnabled)
        {
            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [step2] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=49", false);

            return;
        }

        HfUserName.Value = user.User_UserName;
        HfDistrictID.Value = user.User_District_ID.ToString();

        loadData();
    }

    private void loadData()
    {
        tax_year_submission tys = null;

        if (Session["irs"] != null)
        {
            tys = (tax_year_submission)Session["irs"];
        }

        if (tys != null)
        {
            errorChecking.setDropDownList(Ddl_step2_ALE, tys.IRS_ALE);

            if (tys.IRS_ALE == true)
            {
                PnlALE.Visible = true;
                Btn_Next.Enabled = true;
                Btn_Next.BackColor = System.Drawing.Color.Red;
                loadGV();
            }
            else if (tys.IRS_ALE == false)
            {
                PnlALE.Visible = false;
                tys.IRS_ALE_MEMBERS = new List<ale>();
                clearText();
                Btn_Next.Enabled = true;
                Btn_Next.BackColor = System.Drawing.Color.Red;
            }
        }
        else
        {
            Response.Redirect("step1.aspx", false);
        }
    }

    private void loadGV()
    {

        GvALE.DataSource = AleMemberFactory.GetAllAleMembersForEmployerId(int.Parse(HfDistrictID.Value));
        GvALE.DataBind();

        if (GvALE.Rows.Count > 0)
        {
            Btn_Next.Enabled = true;
            Btn_Next.BackColor = System.Drawing.Color.Red;
        }
        else
        {
            Btn_Next.Enabled = false;
            Btn_Next.BackColor = System.Drawing.Color.Red;
        }
    }

    private void clearText()
    {
        Txt_step2_DGEName.Text = null;
        Txt_step2_EIN.Text = null;
    }

    protected void Ddl_step2_ALE_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;
        tax_year_submission tys = (tax_year_submission)Session["irs"];
        validData = errorChecking.validateDropDownSelection(Ddl_step2_ALE, validData);

        if (validData == true)
        {
            bool deg = bool.Parse(Ddl_step2_ALE.SelectedItem.Value);
            tys.IRS_ALE = deg;
            Session["irs"] = tys;
            loadData();
        }
    }

    protected void ImgBtn_add_Click(object sender, ImageClickEventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateTextBoxNull(Txt_step2_DGEName, validData);
        validData = errorChecking.validateTextBoxEIN(Txt_step2_EIN, validData);

        if (validData == true)
        {
            try
            {
                int _id = 0;
                int _employerID = int.Parse(HfDistrictID.Value);
                string _name = Txt_step2_DGEName.Text;
                string _ein = Txt_step2_EIN.Text;

                ale tempALE = new ale(_id, _employerID, _name, _ein);

                AleMemberFactory.UpsertAleMember(tempALE, HfUserName.Value);
            }
            catch (Exception ex) 
            {
                Log.Warn("Failed to Insert ale Due to exception: ",ex);
            }

            loadGV();
        }
    }

    protected void GvALE_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)GvALE.Rows[e.RowIndex];

            if (row != null)
            {
                HiddenField gv_ale_Id = (HiddenField)row.FindControl("Hf_gv_step2_id");
                int aleId = int.Parse(gv_ale_Id.Value);
                AleMemberFactory.DeleteAleMember(aleId, HfUserName.Value);
            }
        }
        catch (Exception ex) 
        {
            Log.Warn("Failed to delete ale Due to exception: ",ex);
        }

        loadGV();

    }

   
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void Btn_Next_Click(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_step2_ALE, validData);
        tax_year_submission tys = (tax_year_submission)Session["irs"];

        if (validData == true)
        {
            bool validSave = employerController.updateInsertIrsSubmissionApproval(tys);
            Response.Redirect("step5.aspx", false);
        }
    }

    protected void Btn_Previous_Click(object sender, EventArgs e)
    {
        Response.Redirect("step1.aspx", false);
    }
}
