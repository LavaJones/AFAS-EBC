using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using log4net;
using System.Data;
using System.Configuration;

using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class EmployeeCount : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(EmployeeCount));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the EmployeeCount page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=18", false);
            }
        }

        protected void ImgBtnExportCSV_Click(object sender, EventArgs e)
        {
            DataTable export = new DataTable();
            using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
            {
                using (var cmd = new SqlCommand("SELECT_EmployeeCount", connString))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(export);
                    }
                }
            }
            DataTable dataTable = export;
            // Next 4 lines of Code from internet : http://stackoverflow.com/questions/1746701/export-datatable-to-excel-file
            string filename = "EmployeeCount";
            string attachment = "attachment; filename="+ filename.CleanFileName() + ".csv";
            Response.ClearContent();
            Response.BufferOutput = false;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(export.GetAsCsv());

            // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
            Response.Flush(); // Sends all currently buffered output to the client.
            Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            Response.End();
        }
    }
}