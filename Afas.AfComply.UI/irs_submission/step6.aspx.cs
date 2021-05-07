using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Afas.AfComply.Domain;

public partial class step6 : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(step6));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        if (null == employer || false == employer.IrsEnabled)
        {
            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [step6] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=52", false);

            return;
        }

        HfUserName.Value = user.User_UserName;
        HfDistrictID.Value = user.User_District_ID.ToString();

        loadData();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    protected void Ddl_step6_ash_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;
        tax_year_submission tys = (tax_year_submission)Session["irs"];
        validData = errorChecking.validateDropDownSelection(Ddl_step6_ash, validData);

        if (validData == true)
        {
            bool q6 = bool.Parse(Ddl_step6_ash.SelectedItem.Value);
            tys.IRS_ASH = q6;
            Session["irs"] = tys;
            loadData();
        }
    }

    protected void Btn_Next_Click(object sender, EventArgs e)
    {
        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_step6_ash, validData);
        tax_year_submission tys = (tax_year_submission)Session["irs"];

        if (validData == true)
        {
            bool validSave = employerController.updateInsertIrsSubmissionApproval(tys);

            if(validSave)
            {

                Response.Redirect("complete.aspx", false);

            }

           
        }
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
            errorChecking.setDropDownList(Ddl_step6_ash, tys.IRS_ASH);

            if (tys.IRS_ASH == true)
            {
                lit_step6_message.Text = "";
                Btn_Next.Enabled = true;
                Btn_Next.BackColor = System.Drawing.Color.Red;
            }
            else if (tys.IRS_ASH == false)
            {
                lit_step6_message.Text = "Please review and certify the safe harbor contributions for your plan(s) are correct before proceeding";
                Btn_Next.Enabled = false;
                Btn_Next.BackColor = System.Drawing.Color.Red;
            }
        }
        else
        {
            Response.Redirect("step1.aspx", false);
        }
    }
    protected void Btn_Previous_Click(object sender, EventArgs e)
    {
        Response.Redirect("step5.aspx", false);
    }
}