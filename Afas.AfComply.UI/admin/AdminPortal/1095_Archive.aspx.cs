using Afas.AfComply.Reporting.Application.FileCabinetServices;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;
using Afas.AfComply.Reporting.Domain.Approvals.FileCabinet;
using Afas.AfComply.Reporting.Domain.FileCabinet;
using Afas.AfComply.UI.App_Start;
using Afas.Application.Archiver;
using Afas.Application.Services;
using Afas.Domain.POCO;
using Afc.Core.Application;
using log4net;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class _1095_Archive : AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(_1095_Archive));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                this.Log.Warn("A user tried to access the 1095 Archive page which is disabled in the web config.");

                this.Response.Redirect("~/default.aspx?error=14", false);
            }

        }

        protected void Archive1095_Click(object sender, EventArgs e)
        {
            try
            {
                this.Log.Info("1095_Archive.Archive1095_Click started. ");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // We use warn here simply because this is an important item at the start of the method. It's good for tracking Progress
                this.Log.Warn("Archive1095 Running on Tax Year: " + this.DdlYears.SelectedValue);

                // Grab the Tax Year selected by the user, and parse it to an int.
                int taxYear = int.Parse(this.DdlYears.SelectedValue);

                // Grab the currently logged in user and save their name for logging in the DB.
                string RequestorUsername = ((User)this.Session["CurrentUser"]).User_UserName;

                // Get all the Dependencies, this uses the bad approach, because APSX
                IFileArchiver Archiver = ContainerActivator._container.Resolve<IFileArchiver>();
                IArchiveFileInfoService Archive = ContainerActivator._container.Resolve<IArchiveFileInfoService>();
                IFileCabinetInfoService fileCabinetInfoService = ContainerActivator._container.Resolve<IFileCabinetInfoService>();
                IFileCabinetFolderInfoService fileCabinetFolderInfoService = ContainerActivator._container.Resolve<IFileCabinetFolderInfoService>();

                // Set the DB transaction Context
                ITransactionContext Context = ContainerActivator._container.Resolve<ITransactionContext>();
                Archive.Context = Context;
                fileCabinetInfoService.Context = Context;
                fileCabinetFolderInfoService.Context = Context;

                this.Log.Info("All Archive1095 Dependencies Loaded."); 

                // Get the Folder Id for the Tax Year
                // Since the Folder is the same for all Employers for each Tax Year, We only need to get it once.
                FileCabinetFolderInfo TaxYear1095Folder = fileCabinetFolderInfoService.GetFolderInfoBy1095TaxYear(taxYear);
                // If we get a null back from the Service then we need to break and tell the User
                if (null == TaxYear1095Folder)
                {
                    this.Log.Error("Failed to find File Cabinet Folder for Tax Year: " + taxYear);

                    // Add an error message 
                    this.LblMessage.Text = "Failed to find File Cabinet Folder for Tax Year: " + taxYear;
                    this.LblMessage.ForeColor = System.Drawing.Color.White;
                    this.LblMessage.BackColor = System.Drawing.Color.Red;

                    // Don't try to continue, we cannot do this without the Folder
                    return;
                }

                // If the Folder is Good, log the info and continue
                this.Log.Warn(string.Format("Retrieving Folder Info [{0}] for selected TaxYear [{1}]. It has DB Id: [{2}], Folder Depth: [{3}], and Folder Resource Id: [{4}] ", TaxYear1095Folder.FolderName, taxYear, TaxYear1095Folder.ID, TaxYear1095Folder.FolderDepth, TaxYear1095Folder.ResourceId));

                // Add a message that everything worked, but we will replace it if there was an error
                this.LblMessage.Text = "Files Sucessfully moved to File Cabinet";

                // Get All the employers in alphabetical order so we can Loop through them
                List<employer> employerItem = employerController.getAllEmployers();

                // For each Employer, we need to process their PDFs (or Skip the ones that already finished)
                foreach (employer employer in employerItem)
                {
                    // Since this is the main element of this code, I want to be sure we log each loop
                    this.Log.Warn(string.Format("Begining PDF Processing on Employer [{0}] with Resource Id: [{1}]", employer.EMPLOYER_NAME_And_EIN, employer.ResourceId));
                    try
                    {
                        // Get the paths that we will be working with
                        string path = ("~\\client_content\\" + taxYear + "\\" + employer.ResourceId.ToString() + "\\");
                        string mappedPath = HttpContext.Current.Server.MapPath(path);

                        // Check that there is a PDF source Path to move. If not, skip this Employer.
                        if (false == Directory.Exists(mappedPath))
                        {
                            this.Log.Warn(string.Format("Client_content mapped path [{0}] does not exist, Skipping Employer [{1}], Resource ID: [{2}]", mappedPath, employer.EMPLOYER_NAME_And_EIN, employer.ResourceId));
                        }
                        // If there is a Client Content PDF source Path, then Process it.
                        else
                        {

                            this.Log.Debug(string.Format("Client Content mapped path [{0}] exists for Employer [{1}], Resource ID: [{2}], TaxYear:[{3}]", mappedPath, employer.EMPLOYER_NAME_And_EIN, employer.ResourceId, taxYear));

                            // First do all the copying and moving around of the files
                            //This block contains the Code for Copying the PDF files over into the Staging Folder before it is Processed.
                            #region Copy Files to Staging

                            // Build each path value 
                            // Path for the Copy Folder 
                             string CopyFolderPath = ("~\\client_content\\" + taxYear + "\\" + employer.ResourceId.ToString() + "CopyFolder");
                            string mappedCopyFolderPath = HttpContext.Current.Server.MapPath(CopyFolderPath);

                            // Path for the Staging Folder
                            string StagingFolderPath = ("~\\client_content\\" + taxYear + "\\" + employer.ResourceId.ToString() + "\\" + "StagingFolder");
                            string mappedStagingFolderPath = HttpContext.Current.Server.MapPath(StagingFolderPath);

                            this.Log.Debug(string.Format("Mapping Copy Path value [{0}] and Staging Path Value [{1}] for Employer Resource ID: [{2}] TaxYear: [{3}]", mappedCopyFolderPath, mappedStagingFolderPath, employer.ResourceId, taxYear));

                            // If there already exists a finished staging folder, then we skip the copy and move
                            if (Directory.Exists(mappedStagingFolderPath))
                            {
                                // Log it and skip this section
                                this.Log.Info(string.Format("Skipping File Copy Step because Staging folder Exists at Path [{0}] for Employer Resource ID: [{1}] TaxYear: [{2}]", mappedStagingFolderPath, employer.ResourceId, taxYear));
                            }
                            // Otherwise we need to copy and stage files
                            else
                            {
                                // If there already exists a copy folder, we assume it crashed and is corrupted
                                if (Directory.Exists(mappedCopyFolderPath))
                                {
                                    // we assume it crashed and is corrupted, therfore we must delete it and start over.
                                    this.Log.Warn(string.Format("Copy Folder already Exists at [{0}]. Deleting it because it's Probaly corrupted due to a crash. Employer Resource ID: [{1}] TaxYear: [{2}]", mappedCopyFolderPath, employer.ResourceId, taxYear));

                                    Directory.Delete(mappedCopyFolderPath, true);

                                    this.Log.Info("Copy Folder Deleted. Exists: " + Directory.Exists(mappedCopyFolderPath));

                                }

                                // Create the Copy Folder, if it did exist, we deleted it
                                Directory.CreateDirectory(mappedCopyFolderPath);
                                this.Log.Debug(string.Format("Created the copy folder for Employer [{0}] with path: [{1}]", employer.ResourceId, mappedCopyFolderPath));

                                // Double check that the folder exists and that its not actually a file with that name
                                if (Directory.Exists(mappedCopyFolderPath) && (false == File.Exists(mappedCopyFolderPath)))
                                {

                                    // First copy into a Temp Copy Folder
                                    FileSystem.CopyDirectory(mappedPath, mappedCopyFolderPath, true);
                                    this.Log.Debug(string.Format("Files of Employer[{0}] copied to copyfolder[{1}]", employer.ResourceId, mappedCopyFolderPath));

                                    // Then move to the Staging Folder
                                    Directory.Move(mappedCopyFolderPath, mappedStagingFolderPath);
                                    this.Log.Debug(string.Format("Files of Employer[{0}] moved from copyfolder[{1}] to stagingfolder[{2}]", employer.ResourceId, mappedCopyFolderPath, mappedStagingFolderPath));

                                }
                                // Somthing bad happened, Log the info and throw an exception
                                else
                                {
                                    // This is an error case that should stop processing of this employer.
                                    this.Log.Error(string.Format("Cannot copy and move PDFs if the Copy Folder is invalid! CopyFolder: [{0}] Exists: [{1}], File Exists: [{2}]", mappedCopyFolderPath, Directory.Exists(mappedCopyFolderPath), File.Exists(mappedCopyFolderPath)));

                                    // throw an exception to stop processing of this Employer
                                    throw new InvalidOperationException("Cannot copy and move PDFs if the Copy Folder is invalid!");

                                }
                                
                                // Log that we Finished moving the Files for this Employer's PDFs
                                this.Log.Warn(string.Format("Completed Copying of Files to Staging Folder [{0}] for Employer: [{1}], with Resource ID: [{2}], and TaxYear: [{3}]", mappedStagingFolderPath, employer.EMPLOYER_NAME_And_EIN, employer.ResourceId, taxYear));
                            }
                            
                            // If anything goes wrong above, we should get an exception and skip the rest of this employers processing
                            #endregion Copy Files to Staging
                            
                            #region Process the Files in Staging
                            // Then do the Archiving and File Cabineting of all the staged files
                            
                            // Get the list of all the PDFs that are not yet added to the File Cabinet
                            List<FileInfo> PdfFiles = new DirectoryInfo(mappedStagingFolderPath).GetFiles().ToList<FileInfo>();

                            // If there are no Files to be processed then skip to next Employer
                            if (PdfFiles.Count <= 0)
                            {
                                this.Log.Debug(string.Format("Skipping Employer [{0}] PDFs, the Staging Folder [{1}] is empty [{2}].", employer.ResourceId, mappedStagingFolderPath, PdfFiles.Count));

                                // Continue will skip to the next iteration of the loop. aka: the next Employer
                                continue;
                            }

                            // Get all the Employee's from the Database. This is used later to see if we can match each File to an Employee
                            List<Employee> theseEmployees = EmployeeController.manufactureEmployeeList(employer.EMPLOYER_ID);

                            // Individually Add each one
                            foreach (FileInfo pdfFile in PdfFiles)
                            {
                                try
                                {// If one File Fails, we still want to move on to the next file

                                    // Check for NON Pdf files and skip them
                                    if (".pdf" != pdfFile.Extension.ToLower())
                                    {
                                        this.Log.Info(string.Format("Skipping the File[{0}], it is not in a pdf format.", pdfFile.FullName));

                                        // Continue will skip to the next iteration of the loop. aka: the next file
                                        continue;
                                    }

                                    this.Log.Debug(string.Format("Begin Processing PDF file [{0}] for Employer [{1}] in StagingFolder [{2}]", pdfFile.FullName, employer.EMPLOYER_NAME_And_EIN, mappedStagingFolderPath));
                                    this.Log.Debug(string.Format("Created a path string [{0}] for pdffile [{1}] in StagingFolder [{2}]", pdfFile.FullName, pdfFile.Name, mappedStagingFolderPath));

                                    // Split the PDF name on the UNDERSCORES, this is a set naming convention
                                    string PdfFileName = pdfFile.Name;
                                    string[] parts = PdfFileName.Split('_');

                                    // Check that there are enough parts that the File can have the LastName, FirstName, and ResourceId 
                                    if (parts.Count() < 3)
                                    {
                                        this.Log.Info(string.Format("The Filename [{0}] is not in a correct Format.", PdfFileName));

                                        // Continue will skip to the next iteration of the loop. aka: the next file
                                        continue;
                                    }

                                    // Split out the important info, last Name, first Name, and the Employee's Resource Id 
                                    string LastName = parts[0];
                                    string FirstName = parts[1];
                                    string EmployeeResourceIdText = Path.GetFileNameWithoutExtension(PdfFileName.Split('_').Last());
                                    Guid employeeResourceId = Guid.Empty;

                                    // If the GUID parse fails, log it and continue to the next file 
                                    if (false == Guid.TryParse(EmployeeResourceIdText, out employeeResourceId))
                                    {
                                        this.Log.Info(string.Format("Could not parse the Guid from the section [{0}] of the FileName [{1}].", EmployeeResourceIdText, PdfFileName));

                                        // Continue will skip to the next iteration of the loop. aka: the next file
                                        continue;
                                    }

                                    // This is a double check that should be impossible to hit. Should be...
                                    // The Guid can't be the empty Guid (all 0's)
                                    if (Guid.Empty.Equals(employeeResourceId))
                                    {
                                        this.Log.Error(string.Format("Parsed Guid [{0}] was the Empty Guid [{1}].", employeeResourceId, Guid.Empty));

                                        // Continue will skip to the next iteration of the loop. aka: the next file
                                        continue;
                                    }

                                    // Begin Processing File, first archive it, then Add it to FC
                                    this.Log.Debug(string.Format("Starting Processing PDF File with Name [{0}] EmployeeResourceId [{1}] and EmployerResourceId [{2}]", PdfFileName, employeeResourceId, employer.ResourceId));

                                    // Archive the file and Grab the archive ID
                                    int archiveId = Archiver.ArchiveFile(pdfFile.FullName, employer.ResourceId, "1095 Archived", employer.EMPLOYER_ID);
                                    this.Log.Debug(string.Format("Archived file named [{0}] with archiveid : [{1}]", PdfFileName, archiveId));


                                    // If the Resource Id in the File Name is the EmployER resource Id not EmployEE, then it's a Duplicate, from an old bug in the system, skip it
                                    if (employer.ResourceId.Equals(employeeResourceId))
                                    {
                                        this.Log.Debug(string.Format("Skipping File [{0}] because it's Resource Id [{1}] was the Employer ResourceId [{2}]. The archiveFileInfoId is [{3}]", PdfFileName, employeeResourceId, employer.ResourceId, archiveId));

                                        // Continue will skip to the next iteration of the loop. aka: the next file
                                        continue;
                                    }

                                    // try to find the Employee from the list by searching their Resource Id
                                    Employee employeeData = theseEmployees.Where(emp => emp.ResourceId.Equals(employeeResourceId)).FirstOrDefault();

                                    // If we didn't match the Resource Id, then the employee will be null
                                    if (employeeData == null)
                                    {
                                        // We just log it, since this could happen if there was an error at the print shop, or a deleted duplicate employee
                                        this.Log.Warn(string.Format("Found PDF [{0}] that did not match any Employees by GUID. The PDF Resource Id: [{1}], Employer ResourceId [{2}], The archiveFileInfoId is [{3}]", PdfFileName, employeeResourceId, employer.ResourceId, archiveId));

                                        // Don't do anything else, this is an expected/acceptable case.
                                    }

                                    // Get the Archive File Info to Link to teh File Cabinet
                                    ArchiveFileInfo archive = Archive.GetById(archiveId);

                                    //  Build the File Cabinet Object and set all the values
                                    FileCabinetInfo fileCabinetInfo = new FileCabinetInfo()
                                    {
                                        Filename = LastName + " " + FirstName,
                                        FileDescription = LastName + " " + FirstName + " 1095",
                                        FileType = Path.GetExtension(PdfFileName),
                                        OwnerResourceId = employer.ResourceId,
                                        OtherResourceId = employeeResourceId,
                                        ApplicationId = 1, // Hardcoded to AFcomply
                                        ArchiveFileInfo = archive,
                                        FileCabinetFolderInfo = TaxYear1095Folder

                                    };

                                    // Log the info from the object we built
                                    this.Log.Debug(string.Format("Copying Archived File[{0}] of Employer with ownerResourceId[{1}] into FileCabinetInfoClass[{2}]", fileCabinetInfo.OtherResourceId, fileCabinetInfo.OwnerResourceId, fileCabinetInfo.ResourceId));

                                    // Save everything to the DB so we can move on to the next File
                                    fileCabinetInfoService.SaveFileCabinetInfo(fileCabinetInfo, RequestorUsername);

                                    this.Log.Debug(string.Format("Saved File information from [{0}] into FileCabinet Table with Employee ResourceId : [{1}]", PdfFileName, employeeResourceId));

                                }
                                catch (Exception ex)
                                {
                                    // If one File Fails, we still want to move on to the next file, so catch and continue

                                    this.Log.Error(string.Format("Exception occured while Processing a PDF File with Name [{0}], for Employer: [{1}], Employer Resource Id: [{2}]", pdfFile.Name, employer.EMPLOYER_NAME_And_EIN, employer.ResourceId), ex);

                                    // Since it is expected that some PDF files will fail, due to bad data, etc. we will do nothing but log it and just continue processing files. 
                                }
                            }

                            #endregion Process the Files in Staging

                        }
                    }
                    catch (Exception ex)
                    {

                        // We want to continue and process the next employer even if this one had an Exception, so Catch and Continue

                        this.Log.Error(string.Format("Exception occured while Processing al PDFs for Employer: [{0}], Employer Resource Id: [{1}]", employer.EMPLOYER_NAME_And_EIN, employer.ResourceId), ex);

                        // Since it is expected that some Employers will fail, due to bad data, etc. we will do nothing but log it and just continue processing Employers. 
                    }
                }
            }
            catch (Exception ex)
            {

                // We expect this to happen anytime we Time out, or possibly in other cases
                this.Log.Error("Exception Archiveing and Moving 1095s in 'Archive1095_Click'", ex);

                // Add an error message 
                this.LblMessage.Text = "Failure in Main: " + ex.Message;
                this.LblMessage.ForeColor = System.Drawing.Color.White;
                this.LblMessage.BackColor = System.Drawing.Color.Red;

            }

            this.Log.Info("1095_Archive.Archive1095_Click Exited.");

        }
    }
}
