using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Domain.POCO;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class ArchiveFileView : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(ArchiveFileView));
        
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the ArchiveFileView page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=5", false);
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

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {

            int employerId = 0;

            //check that data is correct
            if (
                    null == DdlEmployer.SelectedItem
                        ||
                    null == DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                cofein.Text = "Incorrect parameters";

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            cofein.Text = employ.EMPLOYER_EIN;

            List<ArchiveFileInfo> files = ArchiveFileInfoFactory.GetAllArchivedFilesForEmployerId(employ.EMPLOYER_ID);
            List<ArchiveFileInfo> sortedFiles = files.OrderByDescending(t => t.ArchivedTime).ToList();

            GvCurrentFiles.DataSource = sortedFiles;
            GvCurrentFiles.DataBind();
        }

        protected void GvCurrentFiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps"));
            List<FileInfo> fileList = directory.GetFiles().ToList<FileInfo>();
            GridViewRow row = (GridViewRow)GvCurrentFiles.Rows[e.RowIndex];
            HiddenField hf = (HiddenField)row.FindControl("HfFilePath");
            Label lbl = (Label)row.FindControl("LblFileName");

            string filePath = hf.Value;
            string fileName = lbl.Text;

            //log the access
            PIILogger.LogPII(String.Format("Downloading Archived File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", fileName, Request.UserHostAddress, ((User)Session["CurrentUser"]).User_UserName));

            string appendText = "attachment; filename=" + fileName;
            Response.ContentType = "file/" + Path.GetExtension(fileName);
            Response.AppendHeader("Content-Disposition", appendText);
            Response.TransmitFile(filePath);
            // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
            Response.Flush(); // Sends all currently buffered output to the client.
            Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
            Response.End();
        }
    }
}