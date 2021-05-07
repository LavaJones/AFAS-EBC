using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class step3 : Afas.AfComply.UI.securepages.SecurePageBase
{

    private ILog Log = LogManager.GetLogger(typeof(step3));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {

        if (null == employer || false == employer.IrsEnabled)
        {

            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [step3] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=50", false);

            return;
        
        }

                LitUserName.Text = user.User_UserName;
        HfDistrictID.Value = user.User_District_ID.ToString();

        loadData();

    }

    private void loadData()
    {

        tax_year_submission tys = null;

        if (Session["irs"] != null)
        {
            tys = (tax_year_submission) Session["irs"];
            errorChecking.setDropDownList(Ddl_step3_quest_1, tys.IRS_TR_Q1);

            if (tys.IRS_TR_Q1 == true)
            {
                Pnl_tr_final.Visible = false;
                Ddl_step3_quest_2.SelectedIndex = Ddl_step3_quest_2.Items.Count - 1;
                EnableBtnNext(true);
            }
            else
            {
                Pnl_tr_final.Visible = true;
                errorChecking.setDropDownList(Ddl_step3_quest_2, tys.IRS_TR_Q2);
                ValidateDdlQuestion2();

            }
        }
        
    }

        /// <summary>
        /// Question 1:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ddl_step3_quest_1_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool validData = true;
            tax_year_submission tys = (tax_year_submission)Session["irs"];
            validData = errorChecking.validateDropDownSelection(Ddl_step3_quest_1, validData);

            if (validData == true)
            {
                
                bool q1 = bool.Parse(Ddl_step3_quest_1.SelectedItem.Value);
                tys.IRS_TR_Q1 = q1;
                tys.IRS_TR_Q2 = null;
                tys.IRS_TR_Q3 = null;
                tys.IRS_TR_Q4 = null;
                tys.IRS_TR_Q5 = null;
                tys.IRS_TR = getQualificationValue(tys.IRS_TR_Q1, tys.IRS_TR_Q2);

                Session["irs"] = tys;
                loadData();

            }
            else
            {
                EnableBtnNext(false);
            }
        
        }
        
        protected void Ddl_step3_quest_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool validData = true;
            tax_year_submission tys = (tax_year_submission)Session["irs"];
            validData = errorChecking.validateDropDownSelection(Ddl_step3_quest_2, validData);

            if (validData == true)
            {
                bool tr = bool.Parse(Ddl_step3_quest_2.SelectedItem.Value);
                tys.IRS_TR_Q2 = tr;
                tys.IRS_TR = getQualificationValue(tys.IRS_TR_Q1, tys.IRS_TR_Q2);
                Session["irs"] = tys;
                loadData();
            }
            else
            {
                EnableBtnNext(false);
            }
        }

        private void EnableBtnNext(bool enabled)
        {
            Btn_Next.Enabled = enabled;
            Btn_Next.BackColor = (enabled) ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightGray;
        }

        private void ValidateDdlQuestion2()
        {
            if (Ddl_step3_quest_2 == null || Ddl_step3_quest_2.SelectedItem == null || Ddl_step3_quest_2.SelectedItem.Text.Contains("Select"))
            {
                Ddl_step3_quest_2.BackColor = System.Drawing.Color.Red;
                EnableBtnNext(false);
            }
            else
            {
                Ddl_step3_quest_2.BackColor = System.Drawing.Color.White;
                EnableBtnNext(true);
            }
        }

        private bool getQualificationValue(bool? _q1, bool? _q2)
        {
            
            bool q1 = false;
            if (_q1.HasValue)
            {
                q1 = _q1.Value;
            }

            bool q2 = false;
            if (_q2.HasValue)
            {
                q2 = _q2.Value;
            }

            return (q1 || q2);

        }
        protected void Btn_Next_Click(object sender, EventArgs e)
        {

            bool validData = true;

            validData = errorChecking.validateDropDownSelection(Ddl_step3_quest_1, validData);
            tax_year_submission tys = (tax_year_submission)Session["irs"];

            if (validData == true)
            {

                bool validSave = employerController.updateInsertIrsSubmissionApproval(tys);
                Response.Redirect("step5.aspx", false);

            }

        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }

}