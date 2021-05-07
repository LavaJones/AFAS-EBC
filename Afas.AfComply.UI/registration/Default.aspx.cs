using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

using Afas.AfComply.Domain;

public partial class registration_Default : System.Web.UI.Page
{
    private ILog Log = LogManager.GetLogger(typeof(registration_Default));

    protected void Page_Load(object sender, EventArgs e)
    {
        if (false == Feature.SelfRegistrationEnabled)
        {
            Response.Redirect("../default.aspx?error=53", false);
            return;
        }

        if (!Page.IsPostBack)
        {
            int errorCode = 0;

            loadStates();
            loadEmployerTypes();
            loadMonths();

            RbtnYes.Attributes.Add("onClick", "Javascript:show2ndDate()");
            RbtnNo.Attributes.Add("onClick", "Javascript:hide2ndDate()");
            RbtnBillNo.Attributes.Add("onClick", "Javascript:showBillingUser()");
            RbtnBillYes.Attributes.Add("onClick", "Javascript:hideBillingUser()");

            if (Request.QueryString["error"] != null)
            {
                errorCode = int.Parse(Request.QueryString["error"]);
                loadRegistrationData();

                switch (errorCode)
                { 
                    case 1:
                        break;
                    case 2:
                        MpeError2.Show();
                        break;
                    case 3:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void loadStates()
    {
        DdlDistState.DataSource = StateController.getStates();
        DdlDistState.DataTextField = "State_Name";
        DdlDistState.DataValueField = "State_ID";
        DdlDistState.DataBind();
        DdlBillState.DataSource = StateController.getStates();
        DdlBillState.DataTextField = "State_Name";
        DdlBillState.DataValueField = "State_ID";
        DdlBillState.DataBind();
    }

    private void loadEmployerTypes()
    {
        DdlEmployerType.DataSource = employer_typeController.getEmployerTypes();
        DdlEmployerType.DataTextField = "EMPLOYER_TYPE_NAME";
        DdlEmployerType.DataValueField = "EMPLOYER_TYPE_ID";
        DdlEmployerType.DataBind();
    
    }

    private void loadMonths()
    {
        DdlRenewalDate1.DataSource = MonthController.getMonths();
        DdlRenewalDate1.DataTextField = "MONTH_NAME";
        DdlRenewalDate1.DataValueField = "MONTH_ID";
        DdlRenewalDate1.DataBind();
        DdlRenewalDate1.Items.Add("Select");
        DdlRenewalDate1.SelectedIndex = DdlRenewalDate1.Items.Count - 1;

        DdlRenewalDate2.DataSource = MonthController.getMonths();
        DdlRenewalDate2.DataTextField = "MONTH_NAME";
        DdlRenewalDate2.DataValueField = "MONTH_ID";
        DdlRenewalDate2.DataBind();
        DdlRenewalDate2.Items.Add("Select");
        DdlRenewalDate2.SelectedIndex = DdlRenewalDate1.Items.Count - 1;
    }


    protected void ImgBtnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        bool validData = true;
        Registration re = null;
     
        validData = validateRegistrationData();

        re = (Registration)Session["RegistrationObject"];

        if (validData == true)
        {
            int _employerID = 0;

             _employerID = employerController.newRegistration(re);

            if (_employerID > 0)
            {
                re.REG_EMP_ID = _employerID;
                Session["RegistrationObject"] = re;
                System.Threading.Thread.Sleep(3000);      

                Email notification = new Email();
                string _emailBody = buildEmailBody();

                notification.SendEmail(TxtUserEmail.Text, "Registration Confirmation", _emailBody, true);

                Response.Redirect("reg_confirmation.aspx", false);
            }
            else
            {
                Response.Redirect("Default.aspx?error=2", false);
            }
        }
        else
        {
            Response.Redirect("Default.aspx?error=1", false);
        }
    }

    private bool validateRegistrationData()
    {
        bool validData = true;

        int _employerTypeID = 0;                                              
        try
        {
            _employerTypeID = int.Parse(DdlEmployerType.SelectedItem.Value);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            _employerTypeID = 0;
        }
        string _employerName = TxtDistName.Text;                              
        string _employerAdd = TxtDistAddress.Text;                            
        string _employerCity = TxtDistCity.Text;                              
        int _employerState = 0;                                               
        try
        {
            _employerState = int.Parse(DdlDistState.SelectedItem.Value);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            _employerState = 0;
        }        
        string _employerZip = TxtDistZip.Text;                                
        string _employerEIN = TxtEmployerEIN.Text;                            

        string _renewalDesc = TxtDistRenewalDescription.Text;                   
        int _renewalDate = 0;                                                   
        try
        {
            _renewalDate = int.Parse(DdlRenewalDate1.SelectedItem.Value);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            _renewalDate = 0;
        }
        bool _multiplePlans = RbtnYes.Checked;                                    
        string _renewalDesc2 = TxtDistRenewalDescription2.Text;                 
        int _renewalDate2 = 0;                                                  
        try
        {
            _renewalDate2 = int.Parse(DdlRenewalDate2.SelectedItem.Value);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            _renewalDate2 = 0;
        }

        User adminUser = null;
        User billUser = null;
        string _userfname = TxtUserFname.Text;                                  
        string _userlname = TxtUserLname.Text;                                  
        string _useremail = TxtUserEmail.Text;                                  
        string _userphone = TxtUserPhone.Text;                                  
        string _userName = TxtUserName.Text;                                   
        string _password = TxtUserPass.Text;                                   

        _userName = _userName.ToLower();

        string _billAddress = TxtBillAddress.Text;                            
        string _billCity = TxtBillCity.Text;                                  
        int _billStateID = 0;                                                 
        try
        {
            _billStateID = int.Parse(DdlBillState.SelectedItem.Value);
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            _billStateID = 0;
        }
        string _billZip = TxtBillZip.Text;                                     

                                   
        if (RbtnBillYes.Checked == true)
        {
            adminUser = new User(0, _userfname, _userlname, _useremail, _userphone, _userName, _password, 0, true, true, DateTime.Now, "registration", false, true, false, false);
        }
        else
        {
            string _billEmail = TxtBillEmail.Text;
            string _billPhone = TxtBillPhone.Text;
            string _billUsername = TxtBillUsername.Text;
            string _billPassword = TxtBillPassword.Text;
            string _billFName = TxtBillFName.Text;
            string _billLName = TxtBillLName.Text;

            _billUsername = _billUsername.ToLower();

            billUser = new User(0, _billFName, _billLName, _billEmail, _billPhone, _billUsername, _billPassword, 0, false, true, DateTime.Now, "registration", false, true, false,false);
            adminUser = new User(0, _userfname, _userlname, _useremail, _userphone, _userName, _password, 0, true, true, DateTime.Now, "registration", false, false, false,false);
        }

        Registration roe = new Registration(_employerTypeID, _employerName, _employerEIN, _employerAdd, _employerCity, _employerState, _employerZip, _renewalDesc, _renewalDate, _multiplePlans, _renewalDesc2, _renewalDate2, _billAddress, _billCity, _billStateID, _billZip, adminUser, billUser, _employerName);


        Session["RegistrationObject"] = roe;

        try
        {
            validData = errorChecking.validateTextBoxNull(TxtDistName, validData);
            validData = errorChecking.validateTextBoxNull(TxtDistAddress, validData);
            validData = errorChecking.validateTextBoxNull(TxtDistCity, validData);
            validData = errorChecking.validateTextBoxNull(TxtDistZip, validData);
            validData = errorChecking.validateTextBoxEIN(TxtEmployerEIN, validData);

            validData = errorChecking.validateTextBoxNull(TxtUserFname, validData);
            validData = errorChecking.validateTextBoxNull(TxtUserLname, validData);
            validData = errorChecking.validateTextBoxPhone(TxtUserPhone, validData);
            validData = errorChecking.validateTextBoxEmail(TxtUserEmail, validData);
            validData = errorChecking.validateTextBox6Length(TxtUserName, validData);
            validData = errorChecking.validateTextBoxPassword(TxtUserPass, TxtUserPass2, validData);

            if (RbtnBillNo.Checked == true)
            {
                validData = errorChecking.validateTextBoxNull(TxtBillFName, validData);
                validData = errorChecking.validateTextBoxNull(TxtBillLName, validData);
                validData = errorChecking.validateTextBoxNull(TxtBillAddress, validData);
                validData = errorChecking.validateTextBoxNull(TxtBillCity, validData);
                validData = errorChecking.validateTextBoxNull(TxtBillZip, validData);
                validData = errorChecking.validateTextBoxEmail(TxtBillEmail, validData);
                validData = errorChecking.validateTextBoxPhone(TxtBillPhone, validData);
            }

            if (RbtnYes.Checked == true)
            {
                validData = errorChecking.validateTextBoxNull(TxtDistRenewalDescription2, validData);
                validData = errorChecking.validateDropDownSelection(DdlRenewalDate2, validData);
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            validData = false;
        }

        return validData;
    }

    private string buildEmailBody()
    {
        string _emailBody = null;

        _emailBody += "<h3>Thank you for registering with "+Branding.CompanyName+" Software</h3>";
        _emailBody += "<hr />";
        _emailBody += TxtDistName.Text + " <br />";
        _emailBody += TxtDistAddress.Text + " <br />";
        _emailBody += TxtDistCity.Text + ", " + DdlDistState.SelectedItem.Text + " " + TxtDistZip.Text;
        _emailBody += "<h3>Billing Information</h3>";
        _emailBody += "<hr />";
        _emailBody += TxtBillFName.Text + " " + TxtBillLName.Text + "<br />";
        _emailBody += TxtBillAddress.Text + "<br />";
        _emailBody += TxtBillCity.Text + ", " + DdlBillState.SelectedItem.Text + " " + TxtBillZip.Text + "<br />";
        _emailBody += "Phone: " + TxtBillPhone.Text + "<br />";
        _emailBody += "Email: " + TxtBillEmail.Text + "<br />";
        _emailBody += "<h3>Primary User</h3>";
        _emailBody += "<hr />";
        _emailBody += TxtUserFname.Text + " " + TxtUserLname.Text;
        _emailBody += "<h3>Renewal Date</h3>";
        _emailBody += "<p>We have multiple renewal dates:</p>";
        if (RbtnYes.Checked == true)
        {
            _emailBody += "Yes <br />";
            _emailBody += "Renewal Date:" + DdlRenewalDate1.SelectedItem.Text;
            _emailBody += "<br />Renewal Date 2:" + DdlRenewalDate2.SelectedItem.Text;
        }
        else
        { 
            _emailBody += "No <br />";
            _emailBody += "Renewal Date:" + DdlRenewalDate1.SelectedItem.Text;
        }
        _emailBody += "<h3>Payroll Company</h3>";
        _emailBody += TxtEmployerPayrollSoftware.Text;
        

        return _emailBody;
    }



    private void loadRegistrationData()
    {
        Registration re = null;
        re = (Registration)Session["RegistrationObject"];

        DdlEmployerType.SelectedIndex = -1;
        DdlEmployerType.Items.FindByValue(re.REG_EMP_ID.ToString()).Selected = true;
        TxtDistName.Text = re.REG_EMP_NAME;
        TxtEmployerEIN.Text = re.REG_EIN;
        TxtDistAddress.Text = re.REG_ADDRESS;
        TxtDistCity.Text = re.REG_CITY;
        DdlDistState.Items.FindByValue(re.REG_STATE_ID.ToString()).Selected = true;
        TxtDistZip.Text = re.REG_ZIP;
        TxtDistRenewalDescription.Text = re.REG_PLANNAME1;
        DdlRenewalDate1.SelectedIndex = -1;
        DdlRenewalDate1.Items.FindByValue(re.REG_RENEWAL_MONTH.ToString()).Selected = true;
        if (re.REG_TWO_PLANS == true)
        {
            RbtnYes.Checked = true;
            RbtnNo.Checked = false;
            TxtDistRenewalDescription2.Text = re.REG_PLANNAME2;
            DdlRenewalDate2.SelectedIndex = -1;
            DdlRenewalDate2.Items.FindByValue(re.REG_RENEWAL_MONTH2.ToString()).Selected = true;
            ClientScript.RegisterStartupScript(GetType(), "Program", "<script>show2ndDate();</script>");
        }
        else
        {
            RbtnYes.Checked = false;
            RbtnNo.Checked = true;
        }

        TxtUserFname.Text = re.REG_ADMIN_USER.User_First_Name;
        TxtUserLname.Text = re.REG_ADMIN_USER.User_Last_Name;
        TxtUserEmail.Text = re.REG_ADMIN_USER.User_Email;
        TxtUserPhone.Text = re.REG_ADMIN_USER.User_Phone;
        TxtUserName.Text = re.REG_ADMIN_USER.User_UserName;
        
        TxtBillAddress.Text = re.REG_BILL_ADDRESS;
        TxtBillCity.Text = re.REG_BILL_CITY;
        DdlBillState.SelectedIndex = -1;
        DdlBillState.Items.FindByValue(re.REG_BILL_STATE.ToString()).Selected = true;
        TxtBillZip.Text = re.REG_BILL_ZIP;

        if (re.REG_BILL_USER != null)
        {
            RbtnBillNo.Checked = true;
            TxtBillFName.Text = re.REG_BILL_USER.User_First_Name;
            TxtBillLName.Text = re.REG_BILL_USER.User_Last_Name;
            TxtBillEmail.Text = re.REG_BILL_USER.User_Email;
            TxtBillPhone.Text = re.REG_BILL_USER.User_Phone;
            TxtBillUsername.Text = re.REG_BILL_USER.User_UserName; 
        }
        else
        {
            RbtnBillYes.Checked = true;
        }
    }


    protected void CbSameUser_CheckedChanged(object sender, EventArgs e)
    {

    }
}