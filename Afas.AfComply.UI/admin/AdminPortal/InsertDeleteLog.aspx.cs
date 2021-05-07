using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class InsertDeleteLog : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(ErrorLog));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the ErrorLog page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=28", false);
            }
        }
        //Export CSV data for various insert/delete log of various tables on select.
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            DataTable export = new DataTable();

            using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
            {

                using (var cmd = new SqlCommand("spGetInsertDeletedLog", connString))
                {

                    int logValues = int.Parse(DdlLog.SelectedItem.Value);
                    int employerId = 0;
                    //To allow null to let user pull in all or by employer ID.
                    if (int.TryParse(txtEmployerId.Text, out employerId))
                    {
                        cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = employerId;
                    }
                    else { cmd.Parameters.AddWithValue("@employerId", SqlDbType.Int).Value = DBNull.Value; }

                    cmd.Parameters.AddWithValue("@tableLog", SqlDbType.Int).Value = logValues;

                    using (var da = new SqlDataAdapter(cmd))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(export);

                    }

                }
            }
            string filename = "InsertDeleteLog";
            String attachment = "attachment; filename="+ filename.CleanFileName() + ".csv";

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