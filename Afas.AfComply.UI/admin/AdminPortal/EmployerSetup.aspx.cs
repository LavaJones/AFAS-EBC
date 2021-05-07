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
    public partial class EmployerSetup : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(EmployerSetup));
        
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.BulkImportEnabled)
            {
                Log.Info("A user tried to access the Bulk EmployerSetup page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=24", false);
            }
        }

        protected void BtnUploadFile_Click(object sender, EventArgs e)
        {
            string _filePath = Server.MapPath("~\\ftps\\");
            _filePath += "Classification_Bulk_Import.csv";
            if (ClassificationsFile.HasFile)
            {
                if (false == Directory.Exists(Path.GetDirectoryName(_filePath)))
                {
                    Log.Warn("Directory For Bulk Upload did not exist: " + Path.GetDirectoryName(_filePath));
                    //Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                    return;
                }
                if (File.Exists(_filePath))
                {
                    Log.Warn("File For Bulk Upload already exists on server, Archiving file: " + _filePath);
                    new FileArchiverWrapper().ArchiveFile(_filePath, 1, "Bulk Import File Collision");
                }

                Log.Info("Saving Bulk Import File as: " + _filePath);
                ClassificationsFile.SaveAs(_filePath);

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

                results += ProcessRow(line);
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

        private List<employer> employers;

        private string ProcessRow(string fullRow)
        {
            string results = string.Empty;

            string[] split = fullRow.Split('\t');

            string ein = split[1].Trim();
            string description = split[2].Trim();
            string safeHarbor = split[31] ?? string.Empty; //null check using ?? shorthand
            string ooc = null;
            int waitingPeriodDefault = 6;
            safeHarbor = safeHarbor.ToLower().Trim();
            if (safeHarbor.Equals(string.Empty))
            {
                //check all the remaining columns and take the last one with a value
                for (int i = 43; i > 31; i--)
                {
                    //chech the next 12 columns in reverse order 
                    if (split[i] != null && split[i].ToLower().Trim() != string.Empty)
                    {
                        Log.Debug("Found ASH Code in column[" + i + "] with value: " + split[i].ToLower().Trim());
                        safeHarbor = split[i].ToLower().Trim();
                        break;
                    }
                }

                //if none have a value then leave safe harbor as empty string
            }

            return new EmployerCreation().CreateNewClassification(ein, description, waitingPeriodDefault, safeHarbor, ooc);
        }
    }
}