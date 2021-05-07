using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

public partial class securepages_contact : Afas.AfComply.UI.securepages.SecurePageBase
{

    private ILog Log = LogManager.GetLogger(typeof(securepages_contact));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;
        //// }

        HfDistrictID.Value = user.User_District_ID.ToString();
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }
}