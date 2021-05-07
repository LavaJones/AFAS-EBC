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
    public partial class ErrorLog : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(ErrorLog));

        private string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString;

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the ErrorLog page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=25", false);
            }
        }

        protected void BtnDump_Click(object sender, EventArgs e)
        {
            //Dump a bunch of DB log rows to the log file for easier access.
            using (SqlConnection conn = new SqlConnection(connString))
            {
                StringBuilder builder = new StringBuilder();
                string query = "SELECT TOP 100 * FROM [dbo].[ErrorLog] ORDER BY ErrorTime DESC";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Log.Warn("Writing DB Error Log Table to Log4Net.");
                        while (reader.Read())
                        {
                            builder = new StringBuilder();
                            for(int i = 0; i < reader.FieldCount; i++)
                            {
                                if (reader[i] != null)
                                {
                                    builder.Append(reader[i].ToString());
                                }
                                else
                                {
                                    builder.Append("null");
                                } 
                                builder.Append(", ");
                            }
                            Log.Warn(builder.ToString());
                        }
                    }
                }
            }

            Log.Error("Dump Error Log Called.");

            LblFileUploadMessage.Text = "Dumped to log.";
        }
    }
}