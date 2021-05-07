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
    public partial class EmployerRegistration : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(EmployerRegistration));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.BulkImportEnabled)
            {
                Log.Info("A user tried to access the Bulk EmployerRegistration page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=23", false);
            }
        }

        protected void BtnUploadFile_Click(object sender, EventArgs e)
        {                
            string _filePath = Server.MapPath("~\\ftps\\");
            _filePath += "Employer_Bulk_Import.csv";
            if (EmployersFile.HasFile)
            {
                if (false == Directory.Exists(Path.GetDirectoryName(_filePath))) 
                {
                    Log.Warn("Directory For Bulk Upload did not exist: " + Path.GetDirectoryName(_filePath));
                    return;
                }
                if (File.Exists(_filePath)) 
                {
                    Log.Warn("File For Bulk Upload already exists on server, Archiving file: " + _filePath);
                    new FileArchiverWrapper().ArchiveFile(_filePath, 1, "Bulk Import File Collision");
                }

                Log.Info("Saving Bulk Import File as: " + _filePath);
                EmployersFile.SaveAs(_filePath);

                Log.Info("Processing Bulk Import File: " + _filePath);
                //we need to process the file now
                string results = ProcessFile(_filePath);
                Log.Info("Completed Processing Bulk Import File, results: " + results);

                Log.Info("Archiving Bulk Import File:" + _filePath);
                new FileArchiverWrapper().ArchiveFile(_filePath, 1, "Bulk Import");


                LblFileUploadMessage.Text = results;
            }
            else 
            {
                //if no file provided then ignore.
                return;
            }
        }

        private string ProcessFile(string FilePath)
        {
            string results = string.Empty;

            //start reading the file and parsing each row.
            string[] lines = File.ReadAllLines(FilePath);
            foreach (string line in lines)
            {

                string[] split = line.Split('\t');

                results += new EmployerCreation().CreateEmployer(null, split[2].Trim(), split[3].Trim() + " " + split[4].Trim(),
                        split[5].Trim(), split[6].Trim(), split[7].Trim(), split[1].Trim(), split[8].Trim(),
                        split[9].Trim(), split[10].Trim(), split[11].Trim(), split[13].Trim(), split[14].Trim(), split[2].Trim());
            }

            //return either the list of failures, or Sucess
            if (results != string.Empty)
            {
                return results;
            }
            else
            {
                return "Success!";
            }
        }
    }
}