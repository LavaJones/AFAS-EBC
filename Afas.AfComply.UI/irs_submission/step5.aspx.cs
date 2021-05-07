using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class step5 : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(step5));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        if (null == employer || false == employer.IrsEnabled)
        {
            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [step5] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=51", false);

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
            errorChecking.setDropDownList(Ddl_step6_unpaid, tys.IRS_UNPAID_LEAVE);

            if (tys.IRS_UNPAID_LEAVE == true)
            {
                lit_step5_message.Text = "";
                Btn_Next.Enabled = true;
                Btn_Next.BackColor = System.Drawing.Color.Red;
            }
            else if (tys.IRS_UNPAID_LEAVE == false)
            {
                lit_step5_message.Text = "Please upload any missing data before proceeding ";
                Btn_Next.Enabled = true;
                Btn_Next.BackColor = System.Drawing.Color.Red;
            }
        }
        else
        {
            Response.Redirect("step1.aspx", false);
        }
    }

    protected void Ddl_step6_unpaid_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;
        tax_year_submission tys = (tax_year_submission)Session["irs"];
        validData = errorChecking.validateDropDownSelection(Ddl_step6_unpaid, validData);

        if (validData == true)
        {
            bool q6 = bool.Parse(Ddl_step6_unpaid.SelectedItem.Value);
            tys.IRS_UNPAID_LEAVE = q6;
            Session["irs"] = tys;
            loadData();
        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void Btn_Next_Click(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_step6_unpaid, validData);
        tax_year_submission tys = (tax_year_submission)Session["irs"];

        if (validData == true)
        {
            bool validSave = employerController.updateInsertIrsSubmissionApproval(tys);
            Response.Redirect("step6.aspx", false);
        }
    }
    protected void Btn_Previous_Click(object sender, EventArgs e)
    {
        Response.Redirect("step2.aspx", false);
    }


}