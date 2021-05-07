using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class ReQueueAll : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(ReQueueAll));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the ErrorLog page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=39", false);
            }
        }

        protected void BtnReQueue_Click(object sender, EventArgs e)
        {
            Log.Error("Requeueing all employers.");

            List<employer> employers = employerController.getAllEmployers();
            
            foreach(employer emp in employers)
            {
                employerController.insertEmployerCalculation(emp.EMPLOYER_ID);
            }

            Log.Error("Requeueing all employers finished.");

            LblFileUploadMessage.Text = "ReQueued all.";
        }
    }
}