using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

using log4net;

using Afas.AfComply.Application;

using Afas.AfComply.Domain;
using System.Text;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class UserManagement : AdminPageBase
    {
    
        private ILog Log = LogManager.GetLogger(typeof(UserManagement));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.UserManagementEnabled)
            {
            
                Log.Info("A user tried to access the UserManagement page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=45", false);
            
            }
            else
            {
            
                HfDistrictID.Value = employer.EMPLOYER_ID.ToString();
                HfUserName.Value = user.User_UserName;
                loadEmployers();
                loadDistrictUsers();
            
            }

        }

        private void loadEmployers()
        {

            DdlEmployer.DataSource = employerController.getAllEmployers();
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();

            DdlEmployer.Items.Add("Select");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
        
        }

        private void loadDistrictUsers()
        {

            int _distID = System.Convert.ToInt32(HfDistrictID.Value);

            Session["UserList"] = UserController.getDistrictUsers(_distID);

            List<User> userList = (List<User>)Session["UserList"];

            GvDistrictUsers.DataSource = userList;
            GvDistrictUsers.DataBind();

            LitUserShow.Text = GvDistrictUsers.Rows.Count.ToString();
            LitUserTotal.Text = userList.Count.ToString();

        }

        protected void ImgBtnExportCSV_Click(object sender, EventArgs e)
        {

            DataTable export = new DataTable();

            export.Columns.Add("Employer Name", typeof(string));
            export.Columns.Add("FEIN", typeof(string));
            export.Columns.Add("First Name", typeof(string));
            export.Columns.Add("Last Name", typeof(string));
            export.Columns.Add("Email", typeof(string));
            export.Columns.Add("Power User", typeof(string));
            export.Columns.Add("Billing User", typeof(string));
            export.Columns.Add("IRS Contact", typeof(string));

            // loop throgh every employer and every employee and igonore ones with 
            foreach (employer employer in employerController.getAllEmployers())
            {

                foreach (User user in UserController.getDistrictUsers(employer.EMPLOYER_ID))
                {

                    if (false == user.User_Floater && true == user.USER_ACTIVE)
                    {
                        DataRow row = export.NewRow();
                        row["Employer Name"] = employer.EMPLOYER_NAME;
                        row["FEIN"] = employer.EMPLOYER_EIN;
                        row["First Name"] = user.User_First_Name;
                        row["Last Name"] = user.User_Last_Name;
                        row["Email"] = user.User_Email;
                        row["Power User"] = user.User_Power;
                        row["Billing User"] = user.User_Billing;
                        row["IRS Contact"] = user.User_IRS_CONTACT;

                        export.Rows.Add(row);
                    }
                }
            }

            // Next 4 lines of Code from internet : http://stackoverflow.com/questions/1746701/export-datatable-to-excel-file
            string filename = "Users"; 
            string attachment = "attachment; filename="+ filename.CleanFileName() + ".csv";
            Response.ClearContent();
            Response.BufferOutput = false;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(export.GetAsCsv());

            // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
            Response.Flush(); // Sends all currently buffered output to the client.
            Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            Response.End();
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            HfDistrictID.Value = DdlEmployer.SelectedValue;

            loadDistrictUsers();
        }

        protected void BtnSaveNewUser_Click(object sender, EventArgs e)
        {
            string _fname = null;
            string _lname = null;
            string _email = null;
            string _phone = null;
            string _userName = null;
            string _password = null;
            string _modBy = HfUserName.Value;
            DateTime _modDate = System.DateTime.Now;
            int _distID = 0;
            bool _power = false;
            bool _billing = false;
            bool _irsContact = false;
            User tempUser = null;
            List<User> tempList = (List<User>)Session["UserList"];


            if (validateNewUser() == true)
            {
                _fname = TxtNewFName.Text.Trim();
                _lname = TxtNewLName.Text.Trim();
                _email = TxtNewEmail.Text.Trim();
                _phone = TxtNewPhone.Text.Trim();
                _userName = TxtNewUserName.Text.Trim();
                _password = TxtNewPassword.Text.Trim();
                _distID = System.Convert.ToInt32(HfDistrictID.Value);
                _power = false;
                _billing = false;
                _irsContact = false;

                _userName = _userName.ToLower();

                tempUser = UserController.orderUser(_fname, _lname, _email, _phone, _userName, _password, _distID, _power, _modDate, _modBy, true, _billing, _irsContact);

                if (tempUser != null)
                {
                    tempList.Add(tempUser);
                    clearNewUserData();
                    Session["UserList"] = tempList;
                    loadDistrictUsers();
                }
                else
                {
                    ModalPopupExtender2.Show();
                    LblNewUserMessage.Text = "Username is invalid";
                }
            }
            else
            {
                ModalPopupExtender2.Show();
                LblNewUserMessage.Text = "Please correct all red fields.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool validateNewUser()
        {
            bool validData = true;

            //Validate the User's First Name
            validData = errorChecking.validateTextBoxNull(TxtNewFName, validData);
            //Validate the User's Last Name
            validData = errorChecking.validateTextBoxNull(TxtNewLName, validData);
            //Validate the User's Email. 
            validData = errorChecking.validateTextBoxEmail(TxtNewEmail, validData);
            //Validate the User's Phone. 
            validData = errorChecking.validateTextBoxPhone(TxtNewPhone, validData);
            //Validate the User's UserName
            validData = errorChecking.validateTextBox6Length(TxtNewUserName, validData);
            //Validate the User's Password.
            validData = errorChecking.validateTextBoxPassword(TxtNewPassword, TxtNewPassword2, validData);

            return validData;
        }

        /// <summary>
        /// 
        /// </summary>
        private void clearNewUserData()
        {
            TxtNewFName.Text = null;
            TxtNewLName.Text = null;
            TxtNewEmail.Text = null;
            TxtNewUserName.Text = null;
            TxtNewPassword.Text = null;
            TxtNewPassword2.Text = null;
            LblNewUserMessage.Text = null;
            TxtNewPhone.Text = null;
        }


        protected void GvDistrictUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = GvDistrictUsers.Rows[e.RowIndex];
            HiddenField hfID = (HiddenField)row.FindControl("HfUserID");
            int _userID = System.Convert.ToInt32(hfID.Value);
            bool updateConfirmed = false;

            updateConfirmed = UserController.deleteUser(_userID);

            if (updateConfirmed == true)
            {
                //Get the current list of users. 
                List<User> tempUsers = (List<User>)Session["UserList"];

                foreach (User u in tempUsers)
                {
                    if (u.User_ID == _userID)
                    {
                        tempUsers.Remove(u);
                        break;
                    }
                }

                Session["UserList"] = tempUsers;

                loadDistrictUsers();
            }
            else
            {

            }
        }


        protected void GvDistrictUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GvDistrictUsers.Rows[e.RowIndex];

            string _fname = null;
            string _lname = null;
            string _email = null;
            string _username = null;
            string _phone = null;
            bool _power = false;
            int _userID = 0;
            string _modBy = HfUserName.Value;
            DateTime _modOn = System.DateTime.Now;
            bool _billing = false;
            bool _irsContact = false;

            HiddenField hfID = (HiddenField)row.FindControl("HfUserID");
            HiddenField hfUserName = (HiddenField)row.FindControl("HfUserName");
            TextBox txtfname = (TextBox)row.FindControl("TxtmpFName");
            TextBox txtlname = (TextBox)row.FindControl("TxtmpLName");
            TextBox txtemail = (TextBox)row.FindControl("TxtmpEmail");
            TextBox txtphone = (TextBox)row.FindControl("TxtmpPhone");
            ModalPopupExtender mpe = (ModalPopupExtender)row.FindControl("ModalPopupExtender3");
            CheckBox cb = (CheckBox)row.FindControl("CbtmpPowerUser");
            CheckBox cb2 = (CheckBox)row.FindControl("CbtmpBillingUser");
            CheckBox cb3 = (CheckBox)row.FindControl("CbtmpIRSContact");
            CheckBox cb4 = (CheckBox)row.FindControl("CbtmpFloating");

            bool validData = true;

            validData = errorChecking.validateTextBoxNull(txtfname, validData);
            validData = errorChecking.validateTextBoxNull(txtlname, validData);
            validData = errorChecking.validateTextBoxEmail(txtemail, validData);
            validData = errorChecking.validateTextBoxPhone(txtphone, validData);

            //If data is valid, update user data.
            if (validData == true)
            {
                bool updateConfirmed = false;
                _userID = System.Convert.ToInt32(hfID.Value);
                _username = hfUserName.Value;
                _fname = txtfname.Text;
                _lname = txtlname.Text;
                _email = txtemail.Text;
                _power = cb.Checked;
                _phone = txtphone.Text;
                _billing = cb2.Checked;
                _irsContact = cb3.Checked;

                updateConfirmed = UserController.updateUser(_userID, _fname, _lname, _email, _phone, _power, _modBy, _modOn, _billing, _irsContact);
                UserController.updateUserFloatingFlag(_userID, cb4.Checked);
                //Update the object in memory.
                if (updateConfirmed == true)
                {
                    User currUser = (User)Session["CurrentUser"];

                    if (currUser.User_ID == _userID)
                    {
                        currUser.User_First_Name = _fname;
                        currUser.User_Last_Name = _lname;
                        currUser.User_Email = _email;
                        currUser.User_UserName = _username;
                        currUser.User_Power = _power;
                        currUser.User_Phone = _phone;
                        currUser.LAST_MOD = _modOn;
                        currUser.LAST_MOD_BY = _modBy;
                        currUser.User_Billing = _billing;
                        currUser.User_IRS_CONTACT = _irsContact;

                        Session["CurrentUser"] = currUser;

                        loadCurrentUser();
                    }

                    List<User> tempUsers = (List<User>)Session["UserList"];
                    foreach (User u in tempUsers)
                    {
                        if (u.User_ID == _userID)
                        {
                            u.User_First_Name = _fname;
                            u.User_Last_Name = _lname;
                            u.User_Email = _email;
                            u.User_UserName = _username;
                            u.User_Phone = _phone;
                            u.User_Power = _power;
                            u.LAST_MOD = _modOn;
                            u.LAST_MOD_BY = _modBy;
                            u.User_Billing = _billing;
                            u.User_IRS_CONTACT = _irsContact;
                            break;
                        }
                    }
                    Session["UserList"] = tempUsers;
                    loadDistrictUsers();
                }
            }
            else
            {
                //Display error message.
                mpe.Show();
            }
        }

        private bool loadCurrentUser()
        {
            User currUser = (User)Session["CurrentUser"];

            if (currUser != null)
            {
                //Disable conrtrols if user is not power user.
                if (currUser.User_Power != true)
                {
                    //GvDistrictUsers.Enabled = false;
                    //BtnNewUser.Enabled = false;
                    PnlUsers.Enabled = false;
                }

                HfUserName.Value = currUser.User_UserName;

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void GvDistrictUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvDistrictUsers.PageIndex = e.NewPageIndex;
            loadDistrictUsers();
        }
    }
}