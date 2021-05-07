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

namespace Afas.AfComply.UI.securepages
{
    public partial class SecurePages : System.Web.UI.MasterPage
    {
        private ILog Log = LogManager.GetLogger(typeof(SecurePages));

        protected void Page_Load(object sender, EventArgs e)
        {
            User user = ((User)Session["CurrentUser"]);
            if (user != null)
            {
                if ((employer)Session["CurrentDistrict"] == null)
                {
                    Log.Warn("Current employer not set in session.");
                    Response.Redirect("~/Logout.aspx", false);
                    return;
                }

                LitUserName.Text = user.User_UserName;

                HfDistrictID.Value = user.User_District_ID.ToString();

                employer emp = ((employer)Session["CurrentDistrict"]);

                LitEmployer.Text = emp.EMPLOYER_NAME;

                List<alert> alertItem = alert_controller.manufactureEmployerAlertListAll(emp.EMPLOYER_ID);

                int Sum = 0;

                for (int i = 0; i < alertItem.Count; ++i)

                {
                    Sum += (alertItem[i].ALERT_COUNT);
                }
                if (Sum < 99)
                {
                    CountAlert.Text = Sum.ToString();
                }
                else
                {
                    CountAlert.Text = "99+";
                }
            }
            else
            {
                Log.Warn("Current user not set in session.");
                Response.Redirect("~/Logout.aspx", false);
                return;
            }
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }
    }
}