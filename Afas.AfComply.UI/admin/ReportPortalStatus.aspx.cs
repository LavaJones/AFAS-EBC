using System;
using System.Collections.Generic;
using log4net;
using System.Data;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.UI.admin
{
    public partial class ReportPortalStatus : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(ReportPortalStatus));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access TransmissionStatusReport.");

                Response.Redirect("~/default.aspx?error=46", false);
            }

        }

        protected void BtnDownload_Click(object sender, EventArgs e)
        {
            DataTable TransmissionStatus = employerController.GetTransmissionStatus();
            string filename = "TransmissionStatusReport";
            string attachment = "attachment; filename= "+ filename.CleanFileName() + ".csv";
            Response.ClearContent();
            Response.BufferOutput = false;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(TransmissionStatus.GetAsCsv());
            Response.Flush();         
            Response.SuppressContent = true;                
            Response.End();
        }
    }
}