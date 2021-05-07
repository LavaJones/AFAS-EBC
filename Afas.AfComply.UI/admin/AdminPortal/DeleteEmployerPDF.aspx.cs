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

    /// <summary>
    /// This page gives the ability to delete all the client PDFs in the client_content folder for an employer/taxyear combo
    /// </summary>
    public partial class DeleteEmployerPDF : AdminPageBase
    {
        /// <summary>
        /// Standard logger
        /// </summary>
        private ILog Log = LogManager.GetLogger(typeof(DeleteEmployerPDF));

        /// <summary>
        /// Gets and validates the Employer Dropdown and provides a message if it does not validate
        /// </summary>
        private int EmployerId
        {
            get
            {
                int employerId = 0;

                // check that user has selected a valid item
                if (
                        null == DdlEmployer?.SelectedItem?.Value
                            ||
                        false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                    )
                {
                    // if the item is invalid, then show an error message
                    lblMsg.Text = "Please Select a valid employer. " + DdlEmployer?.SelectedItem?.Value ?? "";
                    lblMsg.BackColor = System.Drawing.Color.Red;
                    lblMsg.ForeColor = System.Drawing.Color.Black;
                    
                }

                return employerId;
            }
        }

        /// <summary>
        /// Gets and validates the TaxYear Dropdown and provides a message if it does not validate
        /// </summary>
        private int TaxYearId
        {
            get
            {

                int taxYear = 0;

                // check that user has selected a valid item
                if (
                        null == DdlCalendarYear?.SelectedItem?.Value
                            ||
                        false == int.TryParse(DdlCalendarYear.SelectedItem.Value, out taxYear)
                    )
                {
                    // if the item is invalid, then show an error message
                    lblMsg.Text = "Please Select a valid tax year. " + DdlCalendarYear?.SelectedItem?.Value ?? "";
                    lblMsg.BackColor = System.Drawing.Color.Red;
                    lblMsg.ForeColor = System.Drawing.Color.Black;

                }

                return taxYear;
            }
        }

        /// <summary>
        /// This is called whenever the page is logged in as an administrator
        /// </summary>
        /// <param name="user">The user that is accessing it</param>
        /// <param name="employer">THe employer that user belongs to</param>
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            // Set the timeout to be long, since this page will deal with lots of backend work
            Server.ScriptTimeout = 1800;

            // check that this page is not toggled off
            if (false == Feature.NewAdminPanelEnabled)
            {

                // Log an redirect, if the feature is not enabled
                Log.Info("A user tried to access the "+ this.GetType().Name + " page which is disabled in the web config. "+Request.UserHostAddress + Request.UserHostName + Request.RawUrl);

                Response.Redirect("~/default.aspx?error=42", false);

            }
            else
            {
                // Populate the page
                loadEmployers();
            }

        }

        /// <summary>
        /// Loads all the Employers into the Dropdown list
        /// </summary>
        private void loadEmployers()
        {
            // Load the Employers into the dropdowns
            DdlEmployer.DataSource = employerController.getAllEmployers();
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();

            // Set the "Select" option in the list and make it default
            DdlEmployer.Items.Add("Select");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;

        }

        /// <summary>
        /// This is the main action for this page that clears the PDF folder in client_content
        /// </summary>
        /// <param name="sender">Standard</param>
        /// <param name="e">Standard</param>
        protected void BtnDeletePDF_Click(object sender, EventArgs e)
        {
            // Check that the employer and taxyeasr dropdowns were selected
            if (0 == EmployerId || 0 == TaxYearId)
            {
                return;
            }

            // Get the employer that was selected
            employer selectedEmployer = employerController.getEmployer(EmployerId);

            // Get the File Path to the Employers PDFs in client_content
            string clientContentFolder = System.Web.HttpContext.Current.Server.MapPath("~/client_content/");
            string fullFolderPath = Path.Combine(clientContentFolder, TaxYearId.ToString());
            fullFolderPath = Path.Combine(fullFolderPath, selectedEmployer.ResourceId.ToString());

            // Log the folder path that we are using 
            Log.Debug("Checking folder for PDFs: " + fullFolderPath);

            // Check that there is a directory to delete
            if (Directory.Exists(fullFolderPath))
            {
                // Log the folder path that we are deleting
                Log.Warn("Clearing PDFs from folder (Archive & Delete): " + fullFolderPath);

                try
                {
                    // Use Ionic to Zip all the files together so we can archive the zip // the Using cleans it up when we're done
                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                    {
                        // Add all the contents of the folder to the zip
                        zip.AddDirectory(fullFolderPath);

                        // Save the zip to the path
                        string zipFullName = fullFolderPath + ".zip";
                        zip.Save(zipFullName);

                        // Log the folder path that we are using 
                        Log.Debug(string.Format("Created Zip Archive of Folder containing [{0}] files. Exists: [{1}], Full Name: [{2}]", zip.Count, File.Exists(zipFullName), zipFullName));

                        //Archive the Zip File, so we have them all should something go wrong. 
                        if (0 != new FileArchiverWrapper().ArchiveFile(zipFullName, selectedEmployer.ResourceId, "Admin Cleared PDF Folder", EmployerId))
                        {
                            // Then delete the Folder and all contents
                            Directory.Delete(fullFolderPath, true);

                            // Notify the user that everything finished
                            lblMsg.Text = "Sucessfully Cleared PDFs, [" + zip.Count+"] files Archived and Cleared.";
                            lblMsg.BackColor = System.Drawing.Color.Green;
                            lblMsg.ForeColor = System.Drawing.Color.White;

                        }
                    }
                }
                catch (Exception ex)
                {
                    // If there was an exception, Log it 
                    Log.Error("Exception while Deleting PDF folder", ex);
                    // And notify the user
                    lblMsg.Text = "Failed, contact IT: " + ex.Message;
                    lblMsg.BackColor = System.Drawing.Color.Red;
                    lblMsg.ForeColor = System.Drawing.Color.Black;
                }
            }
            else
            {
                // Give an error message if the folder doesn't exist because it's been deleted
                lblMsg.Text = "Directory not found: "+ fullFolderPath;
                lblMsg.BackColor = System.Drawing.Color.Red;
                lblMsg.ForeColor = System.Drawing.Color.Black;
            }
        }
    }
}