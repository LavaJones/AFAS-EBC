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

    public partial class AlertsExport : AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(DownloadEmployerFile));
        private IList<employer> Employers { get; set; }
        
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the Alert Export page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=65", false);
            }
            else
            {
                Employers = employerController.getAllEmployers();
            }
        }

        protected void BtnExportToCSV_Click(object sender, EventArgs e)
        {
            DataTable Allexport = CreateExportDataTable();

            Employers = employerController.getAllEmployers();

            foreach (employer emp in Employers)
            { 
                foreach(alert al in alert_controller.manufactureEmployerAlertListAll(emp.EMPLOYER_ID))
                {
                    DataRow row = Allexport.NewRow();

                    row["FEIN"] = emp.EMPLOYER_EIN;
                    row["Employer Name"] = emp.EMPLOYER_NAME;
                    row["Alert Name"] = al.ALERT_NAME;
                    row["Alert Count"] = al.ALERT_COUNT;

                    Allexport.Rows.Add(row);
                }
            }

            DownloadEmployerData(Allexport);
        }
        
        private void DownloadEmployerData(DataTable allexport)
        {

            string fileName = "AllAlertReport_" + DateTime.Now.ToShortDateString();
            string body = "All Alert Export has been downloaded by " + ((Literal)Master.FindControl("LitUserName")).Text + " at " + DateTime.Now.ToString();
            PIILogger.LogPII(String.Format("All Alert Export Download: {0} -- Row Count:[{1}], IP:[{2}], User Id:[{3}]", body, allexport.Rows.Count, Request.UserHostAddress, ((Literal)Master.FindControl("LitUserName")).ID));
            fileName = fileName.Replace('/', '_');
            // Convert to a CSV string and download that as the file
            // Next 4 lines of Code from internet : http://stackoverflow.com/questions/1746701/export-datatable-to-excel-file
            string attachment = "attachment; filename=" + fileName.CleanFileName() + ".csv";
            Response.ClearContent();
            Response.BufferOutput = false;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(allexport.GetAsCsv());
            // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
            Response.Flush(); // Sends all currently buffered output to the client.
            Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            Response.End();
        }

        protected DataTable CreateExportDataTable()
        {
            DataTable export = new DataTable();
            export.Columns.Add("FEIN", typeof(string));
            export.Columns.Add("Employer Name", typeof(string));
            export.Columns.Add("Alert Name", typeof(string));
            export.Columns.Add("Alert Count", typeof(string));
            return export;

        }
        
    }

}