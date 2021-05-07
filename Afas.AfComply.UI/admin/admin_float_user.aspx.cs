using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class admin_admin_float_user : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_admin_float_user));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;
        HfDistrictID.Value = user.User_District_ID.ToString();
        loadEmployers();
        loadUsers();
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

    private void loadUsers()
    {
        int _employerID = int.Parse(HfDistrictID.Value);
        List<User> userList = UserController.getAllUsers();
        List<User> floatingUser = new List<User>();

        foreach (User u in userList)
        {
            if (u.User_Floater == true)
            {
                floatingUser.Add(u);
            }
        }

        DdlUser.DataSource = floatingUser;
        DdlUser.DataTextField = "User_UserName";
        DdlUser.DataValueField = "User_ID";
        DdlUser.DataBind();

        DdlUser.Items.Add("Select");

        var user = floatingUser.FirstOrDefault(u => u.User_UserName.Contains(LitUserName.Text));
        if (user == null)
            DdlUser.SelectedIndex = DdlUser.Items.Count - 1;
        else
            DdlUser.SelectedValue = user.User_ID.ToString();

    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        bool validData = true;
        bool validTransaction = false;
        int _employerID = 0;
        int _userID = 0;
        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
        validData = errorChecking.validateDropDownSelection(DdlUser, validData);

        if (validData == true)
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
            _userID = int.Parse(DdlUser.SelectedItem.Value);
            validTransaction = UserController.updateFloatingUser(_employerID, _userID);
            if (validTransaction == true)
            {
                MpeRolloverMessage.Show();
                LblRolloverMessage.Text = "The floating user account has been moved.";
            }
            else
            {
                MpeRolloverMessage.Show();
                LblRolloverMessage.Text = "An ERROR occurred while trying to move the Floating User.";
            }
        }
        else
        {
            MpeRolloverMessage.Show();
            LblRolloverMessage.Text = "Please correct all red highlighted fields.";
        }

    }


    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }
}