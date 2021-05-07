using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Afas.AfComply.Domain;
using log4net;
using System.Text;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.Domain.POCO;

namespace Afas.AfComply.UI.irs_submission
{
    public partial class irs_submission : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = ((User)Session["CurrentUser"]);
            if (user != null)
            {
                LitUserName.Text = user.User_UserName;
                HfDistrictID.Value = user.User_District_ID.ToString();
                employer emp = ((employer)Session["CurrentDistrict"]);
                LitEmployer.Text = emp.EMPLOYER_NAME;
            }
           }
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }


    }
}