using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class step1 : Afas.AfComply.UI.securepages.SecurePageBase
{
    private ILog Log = LogManager.GetLogger(typeof(step1));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {

        if (null == employer || false == employer.IrsEnabled)
        {
        
            Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [step1] which is is not yet enabled for them.");

            Response.Redirect("~/default.aspx?error=48", false);

            return;
        
        }

        HfUserName.Value = user.User_UserName;
        HfDistrictID.Value = user.User_District_ID.ToString();
            
        loadStates();
        loadData();

    }

    private void loadData()
    {

        tax_year_submission tys = null;
        
        int _taxYear = Feature.CurrentReportingYear;
        int _employerID = int.Parse(HfDistrictID.Value);

        if (Session["irs"] != null)
        {
             tys = (tax_year_submission)Session["irs"];
        }
        else
        {

            tys = employerController.manufactureTaxYearSubmission(_employerID, _taxYear);
            if (tys == null)
            {
                tys = new tax_year_submission();
                tys.IRS_EMPLOYER_ID = _employerID;
                tys.IRS_TAX_YEAR = _taxYear;
                tys.IRS_EDIT = true;
                
            }
            Session["irs"] = tys;
        }

        if (tys != null)
        {
            errorChecking.setDropDownList(Ddl_step1_DGE, tys.IRS_DGE);

            if (tys.IRS_DGE == true)
            {
                Txt_step1_Addresss.Text = tys.IRS_DGE_ADDRESS;
                Txt_step1_City.Text = tys.IRS_DGE_CITY;
                Txt_step1_Fname.Text = tys.IRS_DGE_CONTACT_FNAME;
                Txt_step1_Lname.Text = tys.IRS_DGE_CONTACT_LNAME;
                Txt_step1_DGEName.Text = tys.IRS_DGE_NAME;
                Txt_step1_EIN.Text = tys.IRS_DGE_EIN;
                Txt_step1_Phone.Text = tys.IRS_DGE_PHONE;
                Txt_step1_Zip.Text = tys.IRS_DGE_ZIP;
                errorChecking.setDropDownList(Ddl_step1_State, tys.IRS_DGE_STATE_ID);

                PnlDGE.Visible = true;
                Btn_Next.BackColor = System.Drawing.Color.Red;
                Btn_Next.Enabled = true;
            }
            else if (tys.IRS_DGE == false)
            {
                PnlDGE.Visible = false;
                clearForm();
                Btn_Next.BackColor = System.Drawing.Color.Red;
                Btn_Next.Enabled = true;
            }
        }
        else
        {
            tys = new tax_year_submission();
            Session["irs"] = tys;
        }
    }

    protected void Ddl_step1_DGE_SelectedIndexChanged(object sender, EventArgs e)
    {
        bool validData = true;
        tax_year_submission tys = null;
        validData = errorChecking.validateDropDownSelection(Ddl_step1_DGE, validData);

        if (validData == true)
        {
            bool deg = bool.Parse(Ddl_step1_DGE.SelectedItem.Value);
            tys = (tax_year_submission)Session["irs"];

            tys.IRS_DGE = deg;
            Session["irs"] = tys;
            loadData();
        }
    }

    private void clearForm()
    {
        Txt_step1_Addresss.Text = null;
        Txt_step1_City.Text = null;
        Txt_step1_Fname.Text = null;
        Txt_step1_Lname.Text = null;
        Txt_step1_DGEName.Text = null;
        Txt_step1_EIN.Text = null;
        Txt_step1_Phone.Text = null;
        Txt_step1_Zip.Text = null;
        Ddl_step1_State.SelectedIndex = Ddl_step1_State.Items.Count - 1;
    }

    protected void Btn_Next_Click(object sender, EventArgs e)
    {

        bool validData = true;

        validData = errorChecking.validateDropDownSelection(Ddl_step1_DGE, validData);

        tax_year_submission tys = (tax_year_submission) Session["irs"];

        if (validData == true)
        {

            bool _dge = bool.Parse(Ddl_step1_DGE.SelectedItem.Value);

            if (_dge == true)
            {

                validData = errorChecking.validateTextBoxNull(Txt_step1_DGEName, validData);
                validData = errorChecking.validateTextBoxNull(Txt_step1_EIN, validData);
                validData = errorChecking.validateTextBoxNull(Txt_step1_Addresss, validData);
                validData = errorChecking.validateTextBoxNull(Txt_step1_City, validData);
                validData = errorChecking.validateDropDownSelection(Ddl_step1_State, validData);
                validData = errorChecking.validateTextBoxZipCode(Txt_step1_Zip, validData);
                validData = errorChecking.validateTextBoxPhone(Txt_step1_Phone, validData);
                validData = errorChecking.validateTextBoxNull(Txt_step1_Fname, validData);
                validData = errorChecking.validateTextBoxNull(Txt_step1_Lname, validData);

                if (validData == true)
                {

                    tys.IRS_DGE_NAME = Txt_step1_DGEName.Text;
                    tys.IRS_DGE_EIN = Txt_step1_EIN.Text;
                    tys.IRS_DGE_ADDRESS = Txt_step1_Addresss.Text;
                    tys.IRS_DGE_CITY = Txt_step1_City.Text;
                    tys.IRS_DGE_STATE_ID = int.Parse(Ddl_step1_State.SelectedItem.Value);
                    tys.IRS_DGE_ZIP = Txt_step1_Zip.Text;
                    tys.IRS_DGE_CONTACT_FNAME = Txt_step1_Fname.Text;
                    tys.IRS_DGE_CONTACT_LNAME = Txt_step1_Lname.Text;
                    tys.IRS_DGE_PHONE = Txt_step1_Phone.Text;

                    bool validSave = employerController.updateInsertIrsSubmissionApproval(tys);

                    if (validSave)
                    {
                    
                        Session["irs"] = tys;
                        Response.Redirect("step2.aspx", false);
                    
                    }
                    
                }
                else
                { 
                    this.Log.Warn(String.Format("Invalid data for IRS questionaire for DGE Employer {0}!", int.Parse(HfDistrictID.Value)));
                }

            }
            else
            {

                tys.IRS_DGE_NAME = null;
                tys.IRS_DGE_EIN = null;
                tys.IRS_DGE_ADDRESS = null;
                tys.IRS_DGE_CITY = null;
                tys.IRS_DGE_STATE_ID = 0;
                tys.IRS_DGE_ZIP = null;
                tys.IRS_DGE_CONTACT_FNAME = null;
                tys.IRS_DGE_CONTACT_LNAME = null;
                tys.IRS_DGE_PHONE = null;

                tys.IRS_TOBACCO = false;

                Boolean validSave = employerController.updateInsertIrsSubmissionApproval(tys);

                if (validSave)
                {
                    Session["irs"] = tys;
                    Response.Redirect("./step2.aspx", false);
                }
                else
                {
                    this.Log.Warn(String.Format("Invalid data for IRS questionaire for nonDGE Employer {0}!", int.Parse(HfDistrictID.Value)));
                }

            }

        }
        else
        {
            this.Log.Warn(String.Format("No answer selected for Employer {0}!", int.Parse(HfDistrictID.Value)));
        }

    }

    private void loadStates()
    {
        Ddl_step1_State.DataSource = StateController.getStates();
        Ddl_step1_State.DataTextField = "State_Name";
        Ddl_step1_State.DataValueField = "State_ID";
        Ddl_step1_State.DataBind();

        Ddl_step1_State.Items.Add("Select");
        Ddl_step1_State.SelectedIndex = Ddl_step1_State.Items.Count - 1;
    }
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }
}