using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class EmployerTransmitReport : AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(DownloadEmployerFile));
        private IList<employer> Employers { get; set; }
        
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the Employer Transmit Report page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=65", false);
            }
            else
            {
                Employers = employerController.getAllEmployers();
            }
        }

        protected void BtnExportToCSV_Click(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.Parse(txtsdate.Text);
            DateTime endDate = DateTime.Parse(txtedate.Text);
            DataTable EmployerInfo = employerController.GetEmployerTransmitReport(startDate, endDate);
            string filename = "EmployerInfo";
            string attachment = "attachment; filename="+ filename.CleanFileName() + ".csv";
            Response.ClearContent();
            Response.BufferOutput = false;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(EmployerInfo.GetAsCsv());
            // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
            Response.Flush(); // Sends all currently buffered output to the client.
            Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            Response.End();
        }
     
    }

}