using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registration_reg_confirmation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Registration re = null;
            re = (Registration)Session["RegistrationObject"];

            if (re != null)
            {
                //Confirmation Number
                LitConfirmationNum.Text = re.REG_EMP_ID.ToString();

                State tempdState = StateController.findState(re.REG_STATE_ID);
                State tempbState = StateController.findState(re.REG_BILL_STATE);
                TxtDistName.Text = re.REG_EMP_NAME;
                TxtDistAddress.Text = re.REG_ADDRESS;
                TxtDistCity.Text = re.REG_CITY;
                TxtDistState.Text = tempdState.State_Name;
                TxtDistZip.Text = re.REG_ZIP;

                TxtBillAddress.Text = re.REG_BILL_ADDRESS;
                TxtBillCity.Text = re.REG_BILL_CITY;
                TxtBillState.Text = tempbState.State_Name;
                TxtBillZip.Text = re.REG_BILL_ZIP;

                if (re.REG_BILL_USER != null)
                {
                    TxtBillFName.Text = re.REG_BILL_USER.User_First_Name;
                    TxtBillLName.Text = re.REG_BILL_USER.User_Last_Name;
                    TxtBillEmail.Text = re.REG_BILL_USER.User_Email;
                    TxtBillPhone.Text = re.REG_BILL_USER.User_Phone;
                    TxtBillUsername.Text = re.REG_BILL_USER.User_UserName;
                }
                else
                {
                    TxtBillFName.Text = re.REG_ADMIN_USER.User_First_Name;
                    TxtBillLName.Text = re.REG_ADMIN_USER.User_Last_Name;
                    TxtBillEmail.Text = re.REG_ADMIN_USER.User_Email;
                    TxtBillPhone.Text = re.REG_ADMIN_USER.User_Phone;
                    TxtBillUsername.Text = re.REG_ADMIN_USER.User_UserName;  
                }

                TxtUserFname.Text = re.REG_ADMIN_USER.User_First_Name;
                TxtUserLname.Text = re.REG_ADMIN_USER.User_Last_Name;
                TxtUserEmail.Text = re.REG_ADMIN_USER.User_Email;
                TxtUserPhone.Text = re.REG_ADMIN_USER.User_Phone;
                TxtUserName.Text = re.REG_ADMIN_USER.User_UserName;

                //Set the Plan Renewal Dates
                TxtDistRenewalDescription.Text = re.REG_PLANNAME1;
                DateTime rd = (DateTime)re.REG_RENEWAL_DATE;
                TxtDistRenewalDate.Text = rd.ToShortDateString();

                if (re.REG_TWO_PLANS == true)
                {
                    DateTime rd2 = (DateTime)re.REG_RENEWAL_DATE2;
                    TxtDistRenewalDescription2.Text = re.REG_PLANNAME2;
                    TxtDistRenewalDate2.Text = rd2.ToShortDateString();
                }
                else
                {
                    TxtDistRenewalDescription2.Text = "N/A";
                    TxtDistRenewalDate2.Text = "N/A";
                }

                //Clear out session variables.
                Session["RegistrationObject"] = null;
            }
            else
            {
                Response.Redirect("Default.aspx?error=3", false);
            }

        }
    }
}