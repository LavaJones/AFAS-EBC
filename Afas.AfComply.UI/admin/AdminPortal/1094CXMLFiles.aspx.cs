using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class _1094CXMLFiles : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(_1094CXMLFiles));
      
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the 1094C XML Files page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=3", false);
            }
            else
            {
                loadEmployers();

            }
        }
        private void loadEmployers()
        {
            DdlEmployer.DataSource = employerController.getAllEmployers();
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();
            DdlEmployer.Items.Add("Select");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
        }

        protected void BtnDownload_Click(object sender, EventArgs e)
        {
            if (DdlEmployer.SelectedItem.Text != "Select")
            {
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                DataTable filePath = employerController.GetEmployer1094CFilePath(_employerID);
                if (filePath.Rows.Count == 1)
                {
                    string xmlFilePath = filePath.Rows[0][1].ToString();
                    string xmlFileName = filePath.Rows[0][0].ToString();
                    PIILogger.LogPII(String.Format("Downloading Uploaded File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", xmlFileName, Request.UserHostAddress, ((Literal)Master.FindControl("LitUserName")).Text));
                    var appendText = "attachment; filename=" + xmlFileName;
                    Response.ContentType = "file/" + Path.GetExtension(xmlFileName);
                    Response.AppendHeader("Content-Disposition", appendText);
                    Response.TransmitFile(xmlFilePath);
                    // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
                    Response.Flush(); // Sends all currently buffered output to the client.
                    Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    Response.End();
                }
                else
                {
                    lblMessage.Visible = true;
                }
            }
            
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }
    }
}