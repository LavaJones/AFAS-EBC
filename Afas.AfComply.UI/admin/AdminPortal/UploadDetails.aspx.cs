using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.UI.Code.AFcomply.DataUpload;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using Afas.Domain;
using Afas.Domain.POCO;
using Afas.AfComply.UI.App_Start;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class UploadDetails : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(UploadDetails));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            int UploadId = employer.EMPLOYER_ID;
            Hf_gv_Up_id.Value = UploadId.ToString();
            loadUpload();
        }

        private void loadUpload()
        {
            int UploadId = int.Parse(Hf_gv_Up_id.Value);

            List<UploadedFileInfo> info = new List<UploadedFileInfo>();

            UploadedFileInfo file = UploadFileInfoFactory.GetUploadedFileInfoById(UploadId);
            Lit_gv_Up_name.Text = file.FileNameNoPath;

            info.Add(file);

            GvFile.DataSource = info;
            GvFile.DataBind();

            GvStaging.DataSource = StagingImportFactory.GetAllActiveStagingImportForUpload(UploadId);
            GvStaging.DataBind();
        }

        protected void GvStaging_RowCommand(object sender,
            GridViewCommandEventArgs e)
        {
            string UserId = ((User)Session["CurrentUser"]).User_UserName;
            int StagingId = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "DownloadFile")
            {
                LitMessage.Text = DownloadProcessedFile(StagingId, UserId);
                MpeWebMessage.Show();
            }
            else if (e.CommandName == "ReuploadFile")
            {
                LitMessage.Text = "This feature is not yet finished.";
                MpeWebMessage.Show();
                //ModalPopupExtender1.Show();
            }
            else if (e.CommandName == "Reprocess")
            {
                LitMessage.Text = "This feature is not yet finished.";
                MpeWebMessage.Show();
                //Reporcess(StagingId, UserId);
            }
            else if (e.CommandName == "ViewEdit")
            {
                //should I redirect?                
                // Yes, but I'm not sure exactly how/where
            }
            else
            {
                Log.Warn("Recived an unhandled command from Grid. Command: " + e.CommandName);
            }

            // Reload the data
            loadUpload();
        }

        protected void GvFile_RowCommand(object sender,
            GridViewCommandEventArgs e)
        {
            string UserId = ((User)Session["CurrentUser"]).User_UserName;
            int UploadId = int.Parse(Hf_gv_Up_id.Value);

            if (e.CommandName == "DownloadFile")
            {
                LitMessage.Text = DownloadRawFile(UploadId, UserId);
                MpeWebMessage.Show();
            }
            else if (e.CommandName == "ViewDetails")
            {
                // redirect to the page fore seeing upload details
                Session["UploadFileDetailsViewId"] = UploadId;// I hate using this here, but I don't want to put a DB Id in the Request Params either.
                Response.Redirect("~/admin/AdminPortal/UploadDetails.aspx", false);
            }
            else if (e.CommandName == "ArchiveFile")
            {
                LitMessage.Text = ArchiveUpload(UploadId, UserId);
                MpeWebMessage.Show();
            }
            else
            {
                Log.Warn("Recived an unhandled command from Grid. Command: " + e.CommandName);
            }

            // Reload the data
            loadUpload();
        }
        protected void BtnUploadFile_Click(object sender, EventArgs e)
        {
           
        }
        protected void BtnUploadProcessedFile_Click(object sender, EventArgs e)
        {
            // Re Upload Raw
            int UploadId = int.Parse(Hf_gv_Up_id.Value);
            UploadedFileInfo upload = UploadFileInfoFactory.GetUploadedFileInfoById(UploadId);

            String _appendFile = DateTime.Now.Millisecond.ToString();// to ensure uniqueness
            String _filePath = Server.MapPath("..\\ftps\\");

            String savedFileName = String.Empty;

            FileProcessing.SaveFile(FuReUpload, _filePath, new Label(), _appendFile, out savedFileName);

            User currUser = (User)Session["CurrentUser"];

            try
            {
                AfComplyFileDataImporter importer = ContainerActivator._container.Resolve<AfComplyFileDataImporter>();
                importer.Setup(upload.EmployerId, savedFileName);
                if (importer.ImportData(currUser.User_UserName, "Che Upload Admin", "Demographics"))
                {
                    MpeWebMessage.Show();
                    LitMessage.Text = "The File has been Processed.";
                }
                else
                {
                    MpeWebMessage.Show();
                    LitMessage.Text = "The File was not Processed.";
                    foreach (var val in importer.DataValidationMessages)
                    {
                        LitMessage.Text += " Because " + val.ValidationMessage;
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error("Hit 'Contact IT' Error in [BtnUploadProcessedFile_Click]", exception);
                MpeWebMessage.Show();
                LitMessage.Text = "Please Contact IT";
                return;
            }

            ArchiveUpload(UploadId, currUser.User_UserName);

            Session["UploadFileDetailsViewId"] = null;
            Response.Redirect("~/admin/AdminPortal/UploadManager.aspx", false);
        }

        private string DownloadRawFile(int UploadId, string UserId)
        {
            UploadedFileInfo upload = UploadFileInfoFactory.GetUploadedFileInfoById(UploadId);
            if (null == upload)
            {
                Log.Error("Unable to find UploadFileInfo with Id:" + UploadId);

                return "Uploaded File was not found,";
            }

            if (null == upload.ArchiveFileInfoId || upload.ArchiveFileInfoId <= 0)
            {
                //Download the file
                if (DependencyInjection.GetFileAccess().FileExists(upload.FileName))
                {
                    //log the access
                    PIILogger.LogPII(String.Format("Downloading Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", upload.FileName, Request.UserHostAddress, UserId));

                    //Send the File
                    string appendText = "attachment; filename=" + upload.FileNameNoPath;
                    Response.ContentType = "file/" + upload.FileTypeDescription;
                    Response.AppendHeader("Content-Disposition", appendText);
                    Response.TransmitFile(upload.FileName);
                    // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
                    Response.Flush(); // Sends all currently buffered output to the client.
                    Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                    Response.End();
                }
                else
                {
                    Log.Error("A file that was NOT archived was not found at the file location: " + upload.FileName);

                    return "File not found, it is possible it was already archived by another user, please check the File archives.";
                }
            }
            else
            {
                //Download the file
                ArchiveFileInfo archive = ArchiveFileInfoFactory.GetArchivedFileInfoById((int)upload.ArchiveFileInfoId);
                if (DependencyInjection.GetFileAccess().FileExists(archive.ArchiveFilePath))
                {
                    //log the access
                    PIILogger.LogPII(String.Format("Downloading Upload File -- File Path: [{0}], IP:[{1}], User Name:[{2}]", archive.ArchiveFilePath, Request.UserHostAddress, UserId));

                    //Send the File
                    string appendText = "attachment; filename=" + upload.FileNameNoPath;
                    Response.ContentType = "file/" + upload.FileTypeDescription;
                    Response.AppendHeader("Content-Disposition", appendText);
                    Response.TransmitFile(archive.ArchiveFilePath);
                    // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
                    Response.Flush(); // Sends all currently buffered output to the client.
                    Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                    Response.End();
                }
                else
                {
                    Log.Error("A file that was archived was not found at that archive location! " + archive.ArchiveFilePath);

                    return "Unable to locate archived file, please contact IT.";
                }
            }

            return "File downloaded.";
        }

        public string ArchiveUpload(int UploadId, string UserId)
        {
            UploadedFileInfo upload = UploadFileInfoFactory.GetUploadedFileInfoById(UploadId);
            if (null == upload)
            {
                Log.Error("Unable to find UploadFileInfo with Id:" + UploadId);

                return "Specified File was not found.";
            }

            if (DependencyInjection.GetFileAccess().FileExists(upload.FileName))
            {
                employer Employer = employerController.getEmployer(upload.EmployerId);


                int ArchiveId = new FileArchiverWrapper().ArchiveFile(upload.FileName, Employer.ResourceId, "Archive By Upload Mananger", Employer.EMPLOYER_ID);
                if (ArchiveId <= 0)
                {
                    Log.Error(String.Format(
                        "Failed to ArchiveFile. ArchiveId: [{0}], User: [{1}]",
                        ArchiveId,
                        UserId));

                    return "An Error occurred during file archival, if this persists then please contact IT.";
                }

                // Link the Upload File iNfo with the Archive File Info
                if (false == UploadFileInfoFactory.UpdateUploadedFileInfoArchived((int)UploadId, UserId, ArchiveId))
                {
                    Log.Error(String.Format(
                        "Failed to update Upload Info. UploadId: [{0}], ArchiveId: [{1}], User: [{2}]",
                        UploadId,
                        ArchiveId,
                        UserId));

                    return "An Error occurred while updateing the database, please contact IT.";
                }
            }

            if (false == UploadFileInfoFactory.UpdateUploadedFileInfoEntityStatus(UploadId, UserId, 2))
            {
                Log.Error(String.Format(
                    "Failed to update Entity Status on Upload Info. UploadId: [{0}], User: [{1}]",
                    UploadId,
                    UserId));

                return "An Error occurred while updateing the database, please contact IT.";
            }

            return "File was successfully archived.";
        }

        private string DownloadProcessedFile(int StagingId, string UserId)
        {
            int UploadId = int.Parse(Hf_gv_Up_id.Value);

            UploadedFileInfo upload = UploadFileInfoFactory.GetUploadedFileInfoById(UploadId);
            if (null == upload)
            {
                Log.Error("Unable to find UploadFileInfo with Id:" + UploadId);

                return "Specified File was not found.";
            }

            // Get the Item and be sure it exists
            StagingImport import = StagingImportFactory.GetStagingImportById(StagingId);
            if (null == import)
            {
                Log.Error("Unable to find StagingImport for StagingId:" + StagingId);

                return "File Data not found, please contact IT.";
            }

            if (null != import.Modified && false == import.Modified.TableName.IsNullOrEmpty() && import.Modified.Rows.Count > 0)
            {
                PIILogger.LogPII(String.Format("Downloading Staging File -- StagingId: [{0}], IP:[{1}], User Name:[{2}]", StagingId, Request.UserHostAddress, UserId));

                // Convert to a CSV string and download that as the file
                // Next 4 lines of Code from internet : http://stackoverflow.com/questions/1746701/export-datatable-to-excel-file
                string attachment = "attachment; filename=" + System.IO.Path.GetFileNameWithoutExtension(upload.FileNameNoPath).CleanFileName() +'.'+ System.IO.Path.GetExtension(upload.FileNameNoPath);
                Response.ClearContent();
                Response.BufferOutput = false;
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";

                Response.Write(import.Modified.GetAsCsv());

                // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
                Response.Flush(); // Sends all currently buffered output to the client.
                Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                Response.End();
            }
            else if (null != import.Original && false == import.Original.TableName.IsNullOrEmpty() && import.Original.Rows.Count > 0)
            {
                PIILogger.LogPII(String.Format("Downloading Satging File -- StagingId: [{0}], IP:[{1}], User Name:[{2}]", StagingId, Request.UserHostAddress, UserId));

                // Convert to a CSV string and download that as the file
                // Next 4 lines of Code from internet : http://stackoverflow.com/questions/1746701/export-datatable-to-excel-file
                string attachment = "attachment; filename=" + System.IO.Path.GetFileNameWithoutExtension(upload.FileNameNoPath).CleanFileName() + '.' + System.IO.Path.GetExtension(upload.FileNameNoPath); 
                Response.ClearContent();
                Response.BufferOutput = false;
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";

                Response.Write(import.Original.GetAsCsv());

                // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
                Response.Flush(); // Sends all currently buffered output to the client.
                Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                Response.End();
            }
            else
            {
                Log.Error("Found no Data in the Table");

                return "File Data not found, please contact IT.";
            }

            return "Data Downloaded.";
        }

        private void Reprocess(int StagingId, string UserId)
        {

            throw new NotImplementedException();

        }
    }
}