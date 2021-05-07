using Afas.AfComply.Reporting.Application;
using Afas.AfComply.Reporting.Application.FileCabinetServices;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;
using Afas.AfComply.Reporting.Domain.Approvals.FileCabinet;
using Afas.AfComply.Reporting.Domain.FileCabinet;
using Afas.AfComply.Reporting.Domain.Printing;
using Afas.AfComply.UI.App_Start;
using Afas.Application.Services;
using Afas.Domain.POCO;
using Afc.Core.Application;
using Ionic.Zip;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class IRS1095PDFStaging : AdminPageBase
    {
        private static PerformanceTiming PerfTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), null, SystemSettings.UsePerformanceLog);

        private IPrintBatchService service { get; set; }

        private int EmployerId
        {

            get
            {

                int employerId = 0;

                //check that data is correct
                if (
                        null == this.DdlEmployer.SelectedItem
                            ||
                        null == this.DdlEmployer.SelectedItem.Value
                            ||
                        false == int.TryParse(this.DdlEmployer.SelectedItem.Value, out employerId)
                    )
                {

                    this.lblMsg.Text = "Incorrect parameters";

                }

                return employerId;

            }

        }

        public int TaxYear
        {

            get
            {

                int year = 0;

                int.TryParse(this.DdlTaxYear.SelectedValue, out year);

                return year;

            }

        }

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "PageLoadLoggedInAsAdmin", SystemSettings.UsePerformanceLog))
            {

                if (false == Feature.NewAdminPanelEnabled)
                {
                    this.Log.Info("A user tried to access the IRS1095PDFStaging page which is disabled in the web config.");

                    this.Response.Redirect("~/default.aspx?error=30", false);
                }
                else
                {
                    this.loadEmployers();

                    this.loadFiles();
                }
            }
        }

        private void loadEmployers()
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "loadEmployers", SystemSettings.UsePerformanceLog))
            {
                // Load the list of employers
                this.DdlEmployer.DataSource = employerController.getAllEmployers();
                this.DdlEmployer.DataTextField = "EMPLOYER_NAME";
                this.DdlEmployer.DataValueField = "EMPLOYER_ID";
                this.DdlEmployer.DataBind();

                this.DdlEmployer.Items.Add("Select");
                this.DdlEmployer.SelectedIndex = this.DdlEmployer.Items.Count - 1;

                // Load the tax years.
                this.DdlTaxYear.DataSource = employerController.getTaxYears();
                this.DdlTaxYear.DataBind();

                this.DdlTaxYear.SelectedValue = Feature.CurrentReportingYear.ToString();
            }
        }

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "DdlEmployer_SelectedIndexChanged", SystemSettings.UsePerformanceLog))
            {
                if (this.EmployerId == 0)
                {
                    return;
                }
                methodTimer.StartTimer("Get Files In Dump Path");

                employer employ = employerController.getEmployer(this.EmployerId);

                //Get the directory with all the unprocessed Files
                string printPdfDumpPath = HttpContext.Current.Server.MapPath("~" + Feature.PrintPdfDropPath);
                if (false == Directory.Exists(printPdfDumpPath))
                {
                    this.LblPdfCount.Text = "Source Directory doesn't exist. " + printPdfDumpPath;
                    return;
                }

                string[] FileNames = Directory.GetFiles(printPdfDumpPath, '*' + employ.ResourceId.ToString() + '*');

                methodTimer.LogTimeAndDispose("Get Files In Dump Path");

                methodTimer.StartTimer("Count PDFs To Unzip");

                int fileCount = FileNames.Length;

                foreach (string fileName in FileNames)
                {
                    if (Path.GetExtension(fileName) == ".zip")
                    {
                        fileCount--;
                        using (ZipFile dir = new ZipFile(fileName))
                        {
                            fileCount += dir.Entries.Count;
                            dir.Dispose();
                        }
                    }
                }

                this.LblPdfCount.Text = "There are [" + fileCount + "] pdf files found for this employer.";

                this.lblMsg.Text = "";

                methodTimer.LogTimeAndDispose("Count PDFs To Unzip");

            }
        }

        protected void DdlTaxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadFiles();
        }

        protected void BtnMoveAll1095_Click(object sender, EventArgs e)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "BtnMoveAll1095_Click", SystemSettings.UsePerformanceLog))
            {
                if (this.TaxYear == 0)
                {

                    this.LblPdfCount.Text = "You must select a Tax Year!";
                    return;

                }

                methodTimer.StartTimer("Get From Dependency Injection");

                this.service = ContainerActivator._container.Resolve<IPrintBatchService>();
                this.service.Context = ContainerActivator._container.Resolve<ITransactionContext>();

                methodTimer.LogTimeAndDispose("Get From Dependency Injection");

                methodTimer.StartTimer("Loop through All employers");

                foreach (employer emp in employerController.getAllEmployers())
                {

                    this.Cleanup1095Pdfs(emp, this.TaxYear);

                    this.Move1095PdfsOver(emp, this.TaxYear);

                }

                methodTimer.LogTimeAndDispose("Loop through All employers");

                this.loadFiles();

            }
        }

        protected void BtnMove1095_Click(object sender, EventArgs e)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "BtnMove1095_Click", SystemSettings.UsePerformanceLog))
            {
                if (this.EmployerId == 0)
                {
                    this.LblPdfCount.Text = "You must select an Employer!";
                    return;
                }

                if (this.TaxYear == 0)
                {
                    this.LblPdfCount.Text = "You must select a Tax Year!";
                    return;

                }

                methodTimer.StartTimer("Get From Dependency Injection");

                this.service = ContainerActivator._container.Resolve<IPrintBatchService>();
                this.service.Context = ContainerActivator._container.Resolve<ITransactionContext>();



                methodTimer.LogTimeAndDispose("Get From Dependency Injection");

                methodTimer.StartTimer("Run Move for the employer");

                employer employer = employerController.getEmployer(this.EmployerId);

                this.Cleanup1095Pdfs(employer, this.TaxYear);

                this.Move1095PdfsOver(employer, this.TaxYear);

                this.loadFiles();

                methodTimer.LogTimeAndDispose("Run Move for the employer");

            }
        }

        protected void BtnMoveAll1094_Click(object sender, EventArgs e)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "BtnMoveAll1094_Click", SystemSettings.UsePerformanceLog))
            {
                if (this.TaxYear == 0)
                {

                    this.LblPdfCount.Text = "You must select a Tax Year!";
                    return;

                }

                methodTimer.StartTimer("Get From Dependency Injection");

                this.service = ContainerActivator._container.Resolve<IPrintBatchService>();
                this.service.Context = ContainerActivator._container.Resolve<ITransactionContext>();

                methodTimer.LogTimeAndDispose("Get From Dependency Injection");

                methodTimer.StartTimer("Loop through All employers");

                foreach (employer emp in employerController.getAllEmployers())
                {

                    this.Move1094PdfsOver(emp, this.TaxYear);

                }

                methodTimer.LogTimeAndDispose("Loop through All employers");

                this.loadFiles();

            }
        }

        protected void BtnMove1094_Click(object sender, EventArgs e)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "BtnMove1094_Click", SystemSettings.UsePerformanceLog))
            {
                if (this.EmployerId == 0)
                {
                    this.LblPdfCount.Text = "You must select an Employer!";
                    return;
                }

                if (this.TaxYear == 0)
                {
                    this.LblPdfCount.Text = "You must select a Tax Year!";
                    return;

                }

                methodTimer.StartTimer("Get From Dependency Injection");

                this.service = ContainerActivator._container.Resolve<IPrintBatchService>();
                this.service.Context = ContainerActivator._container.Resolve<ITransactionContext>();

                methodTimer.LogTimeAndDispose("Get From Dependency Injection");

                methodTimer.StartTimer("Run Move for the employer");

                employer employer = employerController.getEmployer(this.EmployerId);

                this.Move1094PdfsOver(employer, this.TaxYear);

                methodTimer.LogTimeAndDispose("Loop through All employers");

                this.loadFiles();
            }
        }

        private void Cleanup1095Pdfs(employer employer, int TaxYear)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "Cleanup1095Pdfs", SystemSettings.UsePerformanceLog))
            {
                methodTimer.StartTimer("Build Basic Directories");

                List<Employee> employees = EmployeeController.manufactureEmployeeList(employer.EMPLOYER_ID);
                User user = ((User)this.Session["CurrentUser"]);
                var Archiver = ContainerActivator._container.Resolve<IArchiveFileInfoService>();
                var fileCabinetInfoService = ContainerActivator._container.Resolve<IFileCabinetInfoService>();
                var fileCabinetFolderInfoService = ContainerActivator._container.Resolve<IFileCabinetFolderInfoService>();
                var Context = ContainerActivator._container.Resolve<ITransactionContext>();
                Archiver.Context = Context;
                fileCabinetInfoService.Context = Context;
                fileCabinetFolderInfoService.Context = Context;

                // Since the Folder is the same for all Employers for each Tax Year, We only need to get it once.
                FileCabinetFolderInfo FolderId = fileCabinetFolderInfoService.GetFolderInfoBy1095TaxYear(TaxYear);

                //Retrieving a FolderId of a Selected taxyear from FileCabinetFolderInfo Class
                long FileCabinetFolderInfo_ID = FolderId.ID;

                //Get the directory with all the unprocessed Files
                string printPdfDumpPath = HttpContext.Current.Server.MapPath("~\\" + Feature.PrintPdfDropPath);

                if (false == Directory.Exists(printPdfDumpPath))
                {
                    this.LblPdfCount.Text = "Source Directory doesn't exist. " + printPdfDumpPath;
                    return;
                }

                string BasePath = HttpContext.Current.Server.MapPath("~\\client_content\\" + TaxYear + "\\") + employer.ResourceId.ToString() + "\\";

                if (false == Directory.Exists(BasePath))
                {
                    this.Log.Warn("Creating Base Path for Employer  in Tax Year [" + TaxYear + "] because it didn't exist, Employer Id: " + employer.EMPLOYER_ID);
                    Directory.CreateDirectory(BasePath);
                }

                //FileInfo[] files = new DirectoryInfo(printPdfDumpPath).GetFiles('*' + employer.ResourceId.ToString() + '*');
                DirectoryInfo[] allTempUnzipDirectories = new DirectoryInfo(printPdfDumpPath).GetDirectories();

                methodTimer.LogTimeAndDispose("Build Basic Directories");
                methodTimer.StartTimer("Get All Batches for Employer");

                // get all the PrintBatches to check if their Resource IDs are used
                IList<PrintBatch> batches = this.service.GetForEmployerTaxYear(employer.EMPLOYER_ID, TaxYear);

                methodTimer.LogTimeAndDispose("Get All Batches for Employer");

                methodTimer.StartLapTimerPaused("Search Batches by folder name");
                methodTimer.StartLapTimerPaused("Get Printed and Files In Folder");
                methodTimer.StartLapTimerPaused("Loop through Every File in Folder");
                methodTimer.StartLapTimerPaused("Update the Database and delete the folder");
                methodTimer.StartLapTimerPaused("Get Print For File");
                methodTimer.StartLapTimerPaused("Archiving Existing Destination File");
                methodTimer.StartLapTimerPaused("Check for Existing Alternates");
                methodTimer.StartLapTimerPaused("Archiving Alternate Duplicates");
                methodTimer.StartLapTimerPaused("Copy New File Over");
                methodTimer.StartLapTimerPaused("Archive Temp Unzipped PDF");
                methodTimer.StartLapTimerPaused("check if File  Exits in FileCabinet");
                methodTimer.StartLapTimerPaused("if File exists, deactivate the Existing File");
                methodTimer.StartLapTimerPaused("Copy the new File into FileCabinet");
                methodTimer.StartLapTimerPaused("Update DB and Clean up");

                methodTimer.StartLapTimer("Run Move for each Directory");

                // check any leftover directories
                foreach (DirectoryInfo directory in allTempUnzipDirectories.OrderBy(item => item.CreationTime))
                {
                    methodTimer.UnpauseLapTimer("Search Batches by folder name");

                    // get just the name of the folder
                    string folderName = directory.Name;

                    // check for any batches where the foldername matches
                    PrintBatch unfinished = batches.Where(bat => bat.ResourceId == new Guid(folderName)).FirstOrDefault();

                    // get the full path for use throughout the method
                    string tempUnzipFolder = directory.FullName;

                    methodTimer.LapAndSwitchTimers("Search Batches by folder name", "Get Printed and Files In Folder");

                    // We only move Items if we can link it to a print batch, otherwise we email and rely on a human process
                    if (unfinished != null)
                    {

                        // We found the batch that the unfinished folder belongs to.
                        FileArchiverWrapper archive = new FileArchiverWrapper();

                        List<Print1095> printFiles = unfinished.AllPrinted1095s.ToList();

                        // grab the unzipped files
                        FileInfo[] files = new DirectoryInfo(tempUnzipFolder).GetFiles("*");

                        methodTimer.LapAndSwitchTimers("Get Printed and Files In Folder", "Loop through Every File in Folder");

                        foreach (FileInfo file in files.OrderBy(fileinfo => fileinfo.CreationTimeUtc))
                        {
                            methodTimer.UnpauseLapTimer("Get Print For File");

                            
                            //ignore any that aren't pdfs
                            if (false == Path.GetExtension(file.Name).Equals(".pdf"))
                            {

                                this.Log.Error("Found Print File Name that is not a PDF with path: " + file.FullName);

                                continue;

                            }
                            string fileNameAndPath = file.FullName;
                            string Filename = file.Name;

                            // Split the PDF name on the UNDERSCORES, this is a set naming convention
                            string[] parts = Filename.Split('_');

                            // Check that there are enough parts that the File can have the LastName, FirstName, and ResourceId 
                            if (parts.Count() < 3)
                            {
                                this.Log.Info(string.Format("The Filename[{0}] is not in a correct Format", Filename));
                                // Continue will skip to the next iteration of the loop. aka: the next file
                                continue;

                            }
                            // Split out the important info, last Name, first Name, and the Employee's Resource Id 
                            string FirstName = parts[1];
                            string LastName = parts[0];
                            string EmployeeResourceIdText = Path.GetFileNameWithoutExtension(Filename.Split('_').Last());
                            Guid employeeResourceId = Guid.Empty;

                            // If the GUID parse fails, log it and continue to the next file 
                            if (false == Guid.TryParse(EmployeeResourceIdText, out employeeResourceId))
                            {
                                this.Log.Info(string.Format("Could not parse the Guid from the section [{0}] of the FileName [{1}].", EmployeeResourceIdText, Filename));

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

                            // Begin Processing File
                            Log.Debug(String.Format("Passing a File with Name [{0}] EmployeeResourceId[{1}] and EmployerResourceId[{2}]", Filename, employeeResourceId, employer.ResourceId));


                            string pdfFileName = Path.GetFileNameWithoutExtension(fileNameAndPath);
                            string destinationPath = BasePath + Path.GetFileName(fileNameAndPath);

                            methodTimer.LapAndSwitchTimers("Check File Name", "Find the 1095 Print for File");

                            // see if we have a print object for it
                            Print1095 thisPrint = printFiles.Where(item => pdfFileName.Contains(item.Approved1095.EmployeeResourceId.ToString())).FirstOrDefault();

                            methodTimer.LapAndSwitchTimers("Get Print For File", "Archiving Existing Destination File");

                            //if the exact file already exists, archive the old one
                            if (File.Exists(destinationPath))
                            {
                                this.Log.Info("Found Existing PDF file for Print with File Name: " + pdfFileName);

                                archive.ArchiveFile(destinationPath, employer.ResourceId, "Replacing old PDF File with new Print Version.", employer.EMPLOYER_ID);
                            }

                            methodTimer.LapAndSwitchTimers("Archiving Existing Destination File", "Check for Existing Alternates");

                            // If we found a matching print item then we will use that to check for dulicates
                            if (thisPrint != null)
                            {
                                methodTimer.UnpauseLapTimer("Archiving Alternate Duplicates");

                                // update the Print object
                                thisPrint.OutputFilePath = destinationPath;

                                //resource Id should be unique, this takes care of any name changes so we still archive old pdfs
                                foreach (string oldEmployeeFile in Directory.GetFiles(BasePath, '?' + thisPrint.Approved1095.EmployeeResourceId.ToString() + '?'))
                                {
                                    this.Log.Info("Found Existing file for Employee Resource Id: " + thisPrint.Approved1095.EmployeeResourceId);

                                    archive.ArchiveFile(oldEmployeeFile, employer.ResourceId, "Replacing old PDF File with new Print Version. (Name Change or Duplicate)", employer.EMPLOYER_ID);

                                    methodTimer.Lap("Archiving Alternate Duplicates");
                                }

                                methodTimer.PauseLap("Archiving Alternate Duplicates");
                            }

                            methodTimer.LapAndSwitchTimers("Check for Existing Alternates", "Copy New File Over");

                            // move it to the final location and archive this one
                            try
                            {

                                File.Copy(fileNameAndPath, destinationPath);

                                methodTimer.LapAndSwitchTimers("Copy New File Over", "Archive Temp Unzipped PDF");

                                //now check whether this file already exists in File Cabinet or not, if exists Inactive the old file and add new File to File Cabinet
                                FileCabinetInfo FileCabinetFiles = fileCabinetInfoService.GetFilesForEmployeeResourceId(employeeResourceId, FileCabinetFolderInfo_ID);
                                this.Log.Info(string.Format("Retrieving Existing Files[{0}] of a Employee by using Employee ResourceId[{1}]", FileCabinetFiles, employeeResourceId));

                                //if the File already exists in File Cabinet 
                                if (FileCabinetFiles != null)
                                {
                                    methodTimer.UnpauseLapTimer("check if File  Exits in FileCabinet");
                                    methodTimer.UnpauseLapTimer("if File exists, deactivate the Existing File");

                                    /*DeactivateFileCabinetInfo method is used to Inactive the Existing File In FileCabinet("Inactive means Changing Entity status = 2") 
                                     This method check for  file in the File cabiet with the help of employeResourceId and FileCabinetFolderInfo_ID if the file exists it changes existing File 
                                     Entity Status ="2" */
                                    fileCabinetInfoService.DeactivateFileCabinetInfo(employeeResourceId, user.User_UserName, FileCabinetFolderInfo_ID);

                                    methodTimer.PauseLap("if File exists, deactivate the Existing File");

                                    this.Log.Info(string.Format("Deactivated the Entitystatus of a File[{0}] with  Employee ResourceId[{1}]", FileCabinetFiles, employeeResourceId));
                                    methodTimer.PauseLap("check if File  Exits in FileCabinet");

                                }

                                // Archive the file and Grab the archive ID
                                int archiveId = archive.ArchiveFile(fileNameAndPath, employer.ResourceId, "Print File Moved for Zipping.", employer.EMPLOYER_ID);

                                // Get the Archive File Info to Link to teh File Cabinet
                                ArchiveFileInfo archiver = Archiver.GetById(archiveId);
                                this.Log.Info(string.Format("Retrieving the Folder Info[{0}] for the selected taxyear[{1}]", FolderId, TaxYear));

                                //  Build the File Cabinet Object and set all the values
                                FileCabinetInfo fileCabinetInfo = new FileCabinetInfo()
                                {
                                    Filename = LastName + " " + FirstName,
                                    FileDescription = LastName + " " + FirstName + " 1095",
                                    FileType = Path.GetExtension(file.Name),
                                    OwnerResourceId = employer.ResourceId,
                                    OtherResourceId = employeeResourceId,
                                    ApplicationId = 1,
                                    ArchiveFileInfo = archiver,
                                    FileCabinetFolderInfo = FolderId,

                                };
                                this.Log.Info(string.Format("Moving a File[{0}] with EmployerResourceId[{1}] and EmployeeResourceID[{2}]  into FileCabinet", Filename, employer.ResourceId, employeeResourceId));
                                methodTimer.UnpauseLapTimer("Copy the new File into FileCabinet");
                                // Save everything to the DB so we can move on to the next File
                                fileCabinetInfoService.SaveFileCabinetInfo(fileCabinetInfo, user.User_UserName);
                                this.Log.Info(string.Format("Saved a File[{0}] with EmployerResourceId[{1}] and EmployeeResourceID[{2}]  into FileCabinet", Filename, employer.ResourceId, employeeResourceId));
                                methodTimer.PauseLap("copy the new File into FileCabinet");

                            }
                            catch (Exception ex)
                            {

                                this.Log.Error(string.Format("Exception Moving PDF. EmployerId: [{0}], fileNameAndPath: [{1}], destinationPath: [{2}]",
                                    employer.EMPLOYER_ID,
                                    fileNameAndPath,
                                    destinationPath), ex);

                                methodTimer.LapAndPause("Copy New File Over"); // incase it errored before it paused

                            }

                            methodTimer.LapAndPause("Archive Temp Unzipped PDF");
                            methodTimer.Lap("Loop through Every File in Folder");

                        }

                        methodTimer.LogAllLapsAndDispose("Loop through Every File in Folder");
                        methodTimer.UnpauseLapTimer("Update the Database and delete the folder");

                        //save the print batch stuff
                        this.service.UpdateBatchReceived(unfinished, user.User_UserName);

                        // Delete the temp folder
                        Directory.Delete(tempUnzipFolder);

                        // Set the Transmission status, since it may not have been set yet
                        EmployerTaxYearTransmissionStatus currentEployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employer.EMPLOYER_ID, TaxYear);
                        if (null != currentEployerTaxYearTransmissionStatus)
                        {

                            EmployerTaxYearTransmissionStatus newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                                        currentEployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                                        TransmissionStatusEnum.Printed,
                                        user.User_UserName,
                                        DateTime.Now
                                    );

                            newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);

                        }

                        methodTimer.LapAndPause("Update the Database and delete the folder");

                        methodTimer.LapAndLog("Run Move for each Directory");

                    }

                }

                methodTimer.LogAllLapsAndDispose("Search Batches by folder name");
                methodTimer.LogAllLapsAndDispose("Get Printed and Files In Folder");
                methodTimer.LogAllLapsAndDispose("Get Print For File");
                methodTimer.LogAllLapsAndDispose("Archiving Existing Destination File");
                methodTimer.LogAllLapsAndDispose("Check for Existing Alternates");
                methodTimer.LogAllLapsAndDispose("Archiving Alternate Duplicates");
                methodTimer.LogAllLapsAndDispose("Copy New File Over");
                methodTimer.LogAllLapsAndDispose("Archive Temp Unzipped PDF");
                methodTimer.LogAllLapsAndDispose("Update the Database and delete the folder");
                methodTimer.LogAllLapsAndDispose("Run Move for each Directory", "", true);

            }
        }

        private void Move1095PdfsOver(employer employer, int TaxYear)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "Move1095PdfsOver", SystemSettings.UsePerformanceLog))
            {
                methodTimer.StartTimer("Get Basic Path Info");

                List<Employee> employees = EmployeeController.manufactureEmployeeList(employer.EMPLOYER_ID);
                User user = ((User)this.Session["CurrentUser"]);
                var Archiver = ContainerActivator._container.Resolve<IArchiveFileInfoService>();
                var fileCabinetInfoService = ContainerActivator._container.Resolve<IFileCabinetInfoService>();
                var fileCabinetFolderInfoService = ContainerActivator._container.Resolve<IFileCabinetFolderInfoService>();
                var Context = ContainerActivator._container.Resolve<ITransactionContext>();
                Archiver.Context = Context;
                fileCabinetInfoService.Context = Context;
                fileCabinetFolderInfoService.Context = Context;

               // Since the Folder is the same for all Employers for each Tax Year, We only need to get it once.
                FileCabinetFolderInfo FolderId = fileCabinetFolderInfoService.GetFolderInfoBy1095TaxYear(TaxYear);

               //Retrieving a FolderId of a Selected taxyear from FileCabinetFolderInfo Class
                long FileCabinetFolderInfo_ID = FolderId.ID;
                
                //Get the directory with all the unprocessed Files
                string printPdfDumpPath = HttpContext.Current.Server.MapPath("~\\" + Feature.PrintPdfDropPath);

                if (false == Directory.Exists(printPdfDumpPath))
                {
                    this.LblPdfCount.Text = "Source Directory doesn't exist. " + printPdfDumpPath;
                    return;
                }

                string BasePath = HttpContext.Current.Server.MapPath("~\\client_content\\" + TaxYear + "\\") + employer.ResourceId.ToString() + "\\";

                if (false == Directory.Exists(BasePath))
                {
                    this.Log.Warn("Creating Base Path for Employer  in Tax Year [" + TaxYear + "] because it didn't exist, Employer Id: " + employer.EMPLOYER_ID);
                    Directory.CreateDirectory(BasePath);

                }

                FileInfo[] files = new DirectoryInfo(printPdfDumpPath).GetFiles('*' + employer.ResourceId.ToString() + '*');
                FileArchiverWrapper archive = new FileArchiverWrapper();


                methodTimer.LogTimeAndDispose("Get Basic Path Info");

                methodTimer.StartLapTimerPaused("Search Batches by file name");
                methodTimer.StartLapTimerPaused("Unzip the File into Temp");
                methodTimer.StartLapTimerPaused("Get All Unzipped Files");
                methodTimer.StartLapTimerPaused("Process Each File");
                methodTimer.StartLapTimerPaused("Check File Name");
                methodTimer.StartLapTimerPaused("Find the 1095 Print for File");
                methodTimer.StartLapTimerPaused("Archive Existing File");
                methodTimer.StartLapTimerPaused("Archive Alternate Files");
                methodTimer.StartLapTimerPaused("Archiving Alternate Duplicates");
                methodTimer.StartLapTimerPaused("Copy New File Over");
                methodTimer.StartLapTimerPaused("Archive Temp Unzipped PDF");
                methodTimer.StartLapTimerPaused("check if File  Exits in FileCabinet");
                methodTimer.StartLapTimerPaused("if File exists, deactivate the Existing File");
                methodTimer.StartLapTimerPaused("Copy the new File into FileCabinet");
                methodTimer.StartLapTimerPaused("Update DB and Clean up");

                methodTimer.StartLapTimer("Loop through Every File in Folder");

                foreach (FileInfo fileName in files.OrderBy(item => item.CreationTime))
                {

                    if (fileName.Extension == ".zip")
                    {

                        methodTimer.UnpauseLapTimer("Search Batches by file name");

                        string filename = fileName.Name;
                        //Print shop added this if they had to restart a print
                        if (filename.Contains("ProdCOPY"))
                        {
                            filename.Replace("ProdCOPY", "");
                        }

                        // Get the PtintBatch if we can 
                        PrintBatch batch = this.service.GetForFileName(Path.GetFileNameWithoutExtension(filename)).FirstOrDefault();
                        List<Print1095> printFiles = new List<Print1095>();
                        string tempUnzipFolder = printPdfDumpPath;

                        methodTimer.LapAndSwitchTimers("Search Batches by file name", "Unzip the File into Temp");


                        // if we found a batch then use that Resource Id 
                        if (batch != null)
                        {

                            tempUnzipFolder = printPdfDumpPath + batch.ResourceId;

                            // to force it to fecth from DB all
                            printFiles = batch.AllPrinted1095s.ToList();
                            batch.PdfReceivedOn = DateTime.Now;
                        }
                        else
                        {
                            // else use a random subfolder
                            tempUnzipFolder = printPdfDumpPath + Guid.NewGuid();
                        }

                        // if the temp folder doesn't exist create it
                        if (false == Directory.Exists(tempUnzipFolder))
                        {
                            Directory.CreateDirectory(tempUnzipFolder);
                        }

                        // Only unzip and and archive the zip file if the folder is emplty, otherwise skip this step and empty out the file as if it has been unzipped.
                        if (Directory.EnumerateFiles(tempUnzipFolder).Count() <= 0)
                        {
                            //Extract everything to the temp file
                            using (ZipFile dir = new ZipFile(fileName.FullName))
                            {
                                // Use a staging sub folder 
                                dir.ExtractAll(tempUnzipFolder, ExtractExistingFileAction.Throw);
                                dir.Dispose();
                            }

                            // archive the zip file now
                            archive.ArchiveFile(fileName.FullName, employer.ResourceId, "UnZipped by " + user.User_UserName, employer.EMPLOYER_ID);

                        }

                        methodTimer.LapAndSwitchTimers("Unzip the File into Temp", "Get All Unzipped Files");

                        // grab the unzipped files
                        files = new DirectoryInfo(tempUnzipFolder).GetFiles("*");
                        if (files.Length <= 0)
                        {
                            this.LblPdfCount.Text = "No files found for Employer.";
                            return;
                        }

                        methodTimer.LapAndSwitchTimers("Get All Unzipped Files", "Process Each File");
                        try
                        {
                            foreach (FileInfo file in files.OrderBy(fileinfo => fileinfo.CreationTimeUtc))
                            {

                                methodTimer.UnpauseLapTimer("Check File Name");

                                // Check for Non Pdf Files and skip them
                                if (".pdf" != file.Extension.ToLower())
                                {
                                    this.Log.Info(string.Format("The File[{0}] of a Employer[{1}] is not in a pdf format", file.Name, employer.ResourceId));

                                    // Continue will skip to the next iteration of the loop. aka: the next Employer
                                    continue;
                                }

                                string fileNameAndPath = file.FullName;
                                string Filename = file.Name;

                                // Split the PDF name on the UNDERSCORES, this is a set naming convention
                                string[] parts = Filename.Split('_');

                                // Check that there are enough parts that the File can have the LastName, FirstName, and ResourceId 
                                if (parts.Count() < 3)
                                {
                                    this.Log.Info(string.Format("The Filename[{0}] is not in a correct Format", Filename));
                                    // Continue will skip to the next iteration of the loop. aka: the next file
                                    continue;

                                }
                                // Split out the important info, last Name, first Name, and the Employee's Resource Id 
                                string FirstName = parts[1];
                                string LastName = parts[0];
                                string EmployeeResourceIdText = Path.GetFileNameWithoutExtension(Filename.Split('_').Last());
                                Guid employeeResourceId = Guid.Empty;

                                // If the GUID parse fails, log it and continue to the next file 
                                if (false == Guid.TryParse(EmployeeResourceIdText, out employeeResourceId))
                                {
                                    this.Log.Info(string.Format("Could not parse the Guid from the section [{0}] of the FileName [{1}].", EmployeeResourceIdText, Filename));

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

                                // Begin Processing File
                                Log.Debug(String.Format("Passing a File with Name [{0}] EmployeeResourceId[{1}] and EmployerResourceId[{2}]", Filename, employeeResourceId, employer.ResourceId));
                                string pdfFileName = Path.GetFileNameWithoutExtension(fileNameAndPath);
                                string destinationPath = BasePath + Path.GetFileName(fileNameAndPath);

                                methodTimer.LapAndSwitchTimers("Check File Name", "Find the 1095 Print for File");

                                // see if we have a print object for it
                                Print1095 thisPrint = printFiles.Where(item => pdfFileName.Contains(item.Approved1095.EmployeeResourceId.ToString())).FirstOrDefault();

                                methodTimer.LapAndSwitchTimers("Find the 1095 Print for File", "Archive Existing File");

                                //if the file already exists, archive the old one
                                if (File.Exists(destinationPath))
                                {
                                    this.Log.Info("Found Existing PDF file for Print with File Name: " + pdfFileName);

                                    archive.ArchiveFile(destinationPath, employer.ResourceId, "Replacing old PDF File with new Print Version.", employer.EMPLOYER_ID);
                                }

                                methodTimer.LapAndSwitchTimers("Archive Existing File", "Archive Alternate Files");

                                // If we found a matching print item then we will use that to check for dulicates
                                if (thisPrint != null)
                                {
                                    methodTimer.UnpauseLapTimer("Archiving Alternate Duplicates");

                                    // update the Print object
                                    thisPrint.OutputFilePath = destinationPath;

                                    //resource Id should be unique, this takes care of any name changes so we still archive old pdfs
                                    foreach (string oldEmployeeFile in Directory.GetFiles(BasePath, '?' + thisPrint.Approved1095.EmployeeResourceId.ToString() + '?'))
                                    {
                                        this.Log.Info("Found Existing file for Employee Resource Id: " + thisPrint.Approved1095.EmployeeResourceId);

                                        archive.ArchiveFile(oldEmployeeFile, employer.ResourceId, "Replacing old PDF File with new Print Version. (Name Change or Duplicate)", employer.EMPLOYER_ID);

                                        methodTimer.Lap("Archiving Alternate Duplicates");

                                    }

                                    methodTimer.PauseLap("Archiving Alternate Duplicates");

                                }


                                // move it to the final location and archive this one
                                try
                                {

                                    methodTimer.UnpauseLapTimer("Copy New File Over");

                                    File.Copy(fileNameAndPath, destinationPath);

                                    methodTimer.LapAndSwitchTimers("Copy New File Over", "Archive Temp Unzipped PDF");

                                  //now check whether this file already exists in File Cabinet or not, if exists Inactive the old file and add new File to File Cabinet
                                    FileCabinetInfo FileCabinetFiles = fileCabinetInfoService.GetFilesForEmployeeResourceId(employeeResourceId, FileCabinetFolderInfo_ID);
                                    this.Log.Info(string.Format("Retrieving Existing Files[{0}] of a Employee by using Employee ResourceId[{1}]", FileCabinetFiles, employeeResourceId));

                                    //if the File already exists in File Cabinet 
                                    if (FileCabinetFiles != null)
                                    {
                                        methodTimer.UnpauseLapTimer("check if File  Exits in FileCabinet");
                                        methodTimer.UnpauseLapTimer("if File exists, deactivate the Existing File");

                                        /*DeactivateFileCabinetInfo method is used to Inactive the Existing File In FileCabinet("Inactive means Changing Entity status = 2") 
                                         This method check for  file in the File cabiet with the help of employeResourceId and FileCabinetFolderInfo_ID if the file exists it changes existing File 
                                         Entity Status ="2" */
                                        fileCabinetInfoService.DeactivateFileCabinetInfo(employeeResourceId, user.User_UserName, FileCabinetFolderInfo_ID);

                                        methodTimer.PauseLap("if File exists, deactivate the Existing File");

                                        this.Log.Info(string.Format("Deactivated the Entitystatus of a File[{0}] with  Employee ResourceId[{1}]", FileCabinetFiles, employeeResourceId));
                                        methodTimer.PauseLap("check if File  Exits in FileCabinet");

                                    }


                                    // Archive the file and Grab the archive ID
                                    int archiveId = archive.ArchiveFile(fileNameAndPath, employer.ResourceId, "Print File Moved for Zipping.", employer.EMPLOYER_ID);

                                    // Get the Archive File Info to Link to teh File Cabinet
                                    ArchiveFileInfo archiver = Archiver.GetById(archiveId);
                                    this.Log.Info(string.Format("Retrieving the Folder Info[{0}] for the selected taxyear[{1}]", FolderId, TaxYear));

                                    //  Build the File Cabinet Object and set all the values
                                    FileCabinetInfo fileCabinetInfo = new FileCabinetInfo()
                                    {
                                        Filename = LastName + " " + FirstName,
                                        FileDescription = LastName + " " + FirstName + " 1095",
                                        FileType = Path.GetExtension(file.Name),
                                        OwnerResourceId = employer.ResourceId,
                                        OtherResourceId = employeeResourceId,
                                        ApplicationId = 1,
                                        ArchiveFileInfo = archiver,
                                        FileCabinetFolderInfo = FolderId,

                                    };
                                    this.Log.Info(string.Format("Moving a File[{0}] with EmployerResourceId[{1}] and EmployeeResourceID[{2}]  into FileCabinet", Filename, employer.ResourceId, employeeResourceId));
                                    methodTimer.UnpauseLapTimer("Copy the new File into FileCabinet");
                                    // Save everything to the DB so we can move on to the next File
                                    fileCabinetInfoService.SaveFileCabinetInfo(fileCabinetInfo, user.User_UserName);
                                    this.Log.Info(string.Format("Saved a File[{0}] with EmployerResourceId[{1}] and EmployeeResourceID[{2}]  into FileCabinet", Filename, employer.ResourceId, employeeResourceId));
                                    methodTimer.PauseLap("copy the new File into FileCabinet");

                                }
                                catch (Exception ex)
                                {
                                    this.Log.Error(string.Format("Exception Moving PDF. EmployerId: [{0}], fileNameAndPath: [{1}], destinationPath: [{2}]",
                                        employer.EMPLOYER_ID,
                                        fileNameAndPath,
                                        destinationPath), ex);

                                    methodTimer.LapAndPause("Copy New File Over");

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Log.Error(string.Format("Exception occured while lopping through unzipped Files of  an employer[{0}]", employer.ResourceId), ex);
                        }

                        methodTimer.LapAndPause("Archive Temp Unzipped PDF");

                        methodTimer.LapAndSwitchTimers("Process Each File", "Update DB and Clean up");

                        if (batch != null)
                        {
                            //save the print batch stuff
                            this.service.UpdateBatchReceived(batch, user.User_UserName);
                        }

                        // Delete the temp folder
                        Directory.Delete(tempUnzipFolder);

                        EmployerTaxYearTransmissionStatus currentEployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employer.EMPLOYER_ID, TaxYear);
                        if (null != currentEployerTaxYearTransmissionStatus)
                        {

                            EmployerTaxYearTransmissionStatus newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                                        currentEployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                                        TransmissionStatusEnum.Printed,
                                        user.User_UserName,
                                        DateTime.Now
                                    );

                            newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);
                        }

                        methodTimer.PauseLap("Update DB and Clean up");
                    }

                    methodTimer.Lap("Loop through Every File in Folder");
                }

                methodTimer.LogAllLapsAndDispose("Loop through Every File in Folder", "", true);
                methodTimer.LogAllLapsAndDispose("Search Batches by file name");
                methodTimer.LogAllLapsAndDispose("Unzip the File into Temp");
                methodTimer.LogAllLapsAndDispose("Get All Unzipped Files");
                methodTimer.LogAllLapsAndDispose("Process Each File");
                methodTimer.LogAllLapsAndDispose("Check File Name");
                methodTimer.LogAllLapsAndDispose("Find the 1095 Print for File");
                methodTimer.LogAllLapsAndDispose("Archive Existing File");
                methodTimer.LogAllLapsAndDispose("Archive Alternate Files");
                methodTimer.LogAllLapsAndDispose("Archiving Alternate Duplicates");
                methodTimer.LogAllLapsAndDispose("Copy New File Over");
                methodTimer.LogAllLapsAndDispose("Archive Temp Unzipped PDF");
                methodTimer.LogAllLapsAndDispose("check if File  Exits in FileCabinet");
                methodTimer.LogAllLapsAndDispose("if File exists, deactivate the Existing File");
                methodTimer.LogAllLapsAndDispose("Copy the new File into FileCabinet");
                methodTimer.LogAllLapsAndDispose("Update DB and Clean up");

                string[] remainingFileNames = Directory.GetFiles(printPdfDumpPath, '*' + employer.ResourceId.ToString() + '*');
                this.LblPdfCount.Text = "There are [" + remainingFileNames.Length + "] pdf files found for this employer.";
                this.lblMsg.Text = "File Sucessfully moved to Client_Content folder and File Cabinet";

            }
        }

        private void Move1094PdfsOver(employer employer, int TaxYear)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "Move1094PdfsOver", SystemSettings.UsePerformanceLog))
            {
                User user = ((User)this.Session["CurrentUser"]);

                //Get the directory with all the unprocessed Files

                string printPdfDumpPath1094 = HttpContext.Current.Server.MapPath("~\\" + Feature.PrintPdfDropPath1094);

                if (false == Directory.Exists(printPdfDumpPath1094))
                {
                    this.LblPdfCount.Text = "Source Directory doesn't exist. " + printPdfDumpPath1094;
                    return;
                }

                string BasePath1094 = HttpContext.Current.Server.MapPath("~\\client_content\\Pdf1094\\" + TaxYear + "\\") + employer.ResourceId.ToString() + "\\";

                if (false == Directory.Exists(BasePath1094))
                {
                    this.Log.Warn("Creating Base Path for Employer  in Tax Year [" + TaxYear + "] because it didn't exist, Employer Id: " + employer.EMPLOYER_ID);
                    Directory.CreateDirectory(BasePath1094);
                }

                FileInfo[] files1094 = new DirectoryInfo(printPdfDumpPath1094).GetFiles('*' + employer.ResourceId.ToString() + '*');
                FileArchiverWrapper archive = new FileArchiverWrapper();

                foreach (FileInfo fileName1094 in files1094.OrderBy(item => item.CreationTime))
                {
                    if (fileName1094.Extension == ".zip")
                    {
                        string filename1094 = fileName1094.Name;
                        //Print shop added this if they had to restart a print
                        if (filename1094.Contains("ProdCOPY"))
                        {
                            filename1094.Replace("ProdCOPY", "");
                        }

                        // Get the PtintBatch if we can 
                        PrintBatch batch = this.service.GetForFileName(Path.GetFileNameWithoutExtension(filename1094)).FirstOrDefault();
                        List<Print1094> printFiles1094 = new List<Print1094>();
                        string tempfolder1094 = printPdfDumpPath1094;
                        if (batch != null)
                        {
                            tempfolder1094 = tempfolder1094 + batch.ResourceId;
                            printFiles1094 = batch.AllPrinted1094s.ToList();
                            batch.PdfReceivedOn = DateTime.Now;
                        }
                        else
                        {
                            // else use a random subfolder
                            tempfolder1094 = tempfolder1094 + Guid.NewGuid();
                        }

                        // if the temp folder doesn't exist create it
                        if (false == Directory.Exists(tempfolder1094))
                        {
                            Directory.CreateDirectory(tempfolder1094);
                        }

                        // Only unzip and and archive the zip file if the folder is emplty, otherwise skip this step and empty out the file as if it has been unzipped.
                        if (Directory.EnumerateFiles(tempfolder1094).Count() <= 0)
                        {

                            //Extract everything to the temp file
                            using (ZipFile dir = new ZipFile(fileName1094.FullName))
                            {
                                // Use a staging sub folder 
                                dir.ExtractAll(tempfolder1094, ExtractExistingFileAction.Throw);
                                dir.Dispose();
                            }

                            // archive the zip file now
                            archive.ArchiveFile(fileName1094.FullName, employer.ResourceId, "UnZipped by " + user.User_UserName, employer.EMPLOYER_ID);

                        }

                        // grab the unzipped files
                        files1094 = new DirectoryInfo(tempfolder1094).GetFiles("*");
                        if (files1094.Length <= 0)
                        {
                            this.LblPdfCount.Text = "No files found for Employer.";
                            return;
                        }

                        foreach (FileInfo file in files1094.OrderBy(fileinfo => fileinfo.CreationTimeUtc))
                        {
                            string fileNameAndPath = file.FullName;

                            //ignore any that aren't pdfs
                            if (false == Path.GetExtension(fileNameAndPath).Equals(".pdf"))
                            {

                                this.Log.Error("Found Print File Name that is not a PDF with path: " + fileNameAndPath);
                                continue;
                            }

                            string pdfFileName = Path.GetFileNameWithoutExtension(fileNameAndPath);
                            string destinationPath1094 = BasePath1094 + Path.GetFileName(fileNameAndPath);

                            // see if we have a print object for it
                            Print1094 thisPrint1094 = printFiles1094.Where(item => pdfFileName.Contains(item.Approved1094.EmployerResourceId.ToString())).FirstOrDefault();

                            //if the file already exists, archive the old one
                            if (File.Exists(destinationPath1094))
                            {
                                this.Log.Info("Found Existing PDF file for Print with File Name: " + pdfFileName);

                                archive.ArchiveFile(destinationPath1094, employer.ResourceId, "Replacing old PDF File with new Print Version.", employer.EMPLOYER_ID);
                            }

                            if (thisPrint1094 != null)
                            {
                                // update the Print object
                                thisPrint1094.OutputFilePath = destinationPath1094;

                                //resource Id should be unique, this takes care of any name changes so we still archive old pdfs
                                foreach (string oldEmployeeFile in Directory.GetFiles(BasePath1094, '?' + thisPrint1094.Approved1094.ResourceId.ToString() + '?'))
                                {
                                    this.Log.Info("Found Existing file for Employee Resource Id: " + thisPrint1094.Approved1094.ResourceId);

                                    archive.ArchiveFile(oldEmployeeFile, employer.ResourceId, "Replacing old PDF File with new Print Version. (Name Change or Duplicate)", employer.EMPLOYER_ID);
                                }
                            }

                            // move it to the final location and archive this one
                            try
                            {

                                File.Copy(fileNameAndPath, destinationPath1094);

                                archive.ArchiveFile(fileNameAndPath, employer.ResourceId, "Print File Moved for Zipping.", employer.EMPLOYER_ID);

                            }
                            catch (Exception ex)
                            {
                                this.Log.Error(string.Format("Exception Moving PDF. EmployerId: [{0}], fileNameAndPath: [{1}], destinationPath: [{2}]",
                                    employer.EMPLOYER_ID,
                                    fileNameAndPath,
                                    destinationPath1094), ex);
                            }

                        }

                        if (batch != null)
                        {
                            //save the print batch stuff
                            this.service.UpdateBatchReceived(batch, user.User_UserName);
                        }

                        // Delete the temp folder
                        Directory.Delete(tempfolder1094);

                        // Update the Status if it hasn't already
                        EmployerTaxYearTransmissionStatus currentEployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employer.EMPLOYER_ID, TaxYear);
                        if (null != currentEployerTaxYearTransmissionStatus)
                        {

                            EmployerTaxYearTransmissionStatus newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                                        currentEployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                                        TransmissionStatusEnum.Printed,
                                        user.User_UserName,
                                        DateTime.Now
                                    );

                            newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);
                        }

                        string[] remainingFileNames1094 = Directory.GetFiles(printPdfDumpPath1094, '*' + employer.ResourceId.ToString() + '*');
                        this.LblPdfCount1094.Text = "There are [" + remainingFileNames1094.Length + "] pdf files found for this employer.";
                        this.lblMsg.Text = "Success";
                    }
                }
            }
        }

        private void loadFiles()
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "loadFiles", SystemSettings.UsePerformanceLog))
            {
                try
                {

                    if (this.TaxYear == 0)
                    {
                        return;
                    }

                    string printPdfDumpPath = HttpContext.Current.Server.MapPath("~\\" + Feature.PrintPdfDropPath);

                    DirectoryInfo directory = new DirectoryInfo(printPdfDumpPath);

                    List<DirectoryInfo> directories = directory.GetDirectories().ToList();
                    this.GvCurrentDirectories.DataSource = directories;
                    this.GvCurrentDirectories.DataBind();

                    List<FileInfo> ZipFiles = directory.GetFiles().ToList<FileInfo>();
                    this.GvCurrentFiles.DataSource = ZipFiles;
                    this.GvCurrentFiles.DataBind();

                }
                catch (Exception ex)
                {
                    this.Log.Error("Errors while loading  list of Files from IRS1095PDFStaging.aspx page.", ex);
                }
            }
        }

        protected void GvCurrentFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "GvCurrentFiles_RowDeleting", SystemSettings.UsePerformanceLog))
            {
                int index = Convert.ToInt32(e.RowIndex);

                Label Namelbl = (Label)this.GvCurrentFiles.Rows[index].Cells[2].FindControl("LblFileName");
                HiddenField FilePathlbl = (HiddenField)this.GvCurrentFiles.Rows[index].Cells[2].FindControl("HfFilePath");

                if (0 != new FileArchiverWrapper().ArchiveFile(FilePathlbl.Value.ToString(), 1, "Admin Deleted Zip File: " + Namelbl.Text))
                {
                    this.MpeWebMessage.Show();
                    this.LitMessage.Text = Namelbl.Text + " has been DELETED.";
                }

                this.loadFiles();
            }
        }

        protected void GvCurrentDirectories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (PerformanceTiming methodTimer = new PerformanceTiming(typeof(IRS1095PDFStaging), "GvCurrentDirectories_RowDeleting", SystemSettings.UsePerformanceLog))
            {
                methodTimer.StartTimer("Get Values and Check the File System");

                int index = Convert.ToInt32(e.RowIndex);

                Label Namelbl = (Label)this.GvCurrentDirectories.Rows[index].Cells[2].FindControl("LblFileName");
                HiddenField FilePathlbl = (HiddenField)this.GvCurrentDirectories.Rows[index].Cells[2].FindControl("HfFilePath");

                string fullFolderPath = FilePathlbl.Value.ToString();
                if ((!Directory.Exists(fullFolderPath)))
                {
                    return;
                }

                methodTimer.LogTimeAndDispose("Get Values and Check the File System");
                methodTimer.StartTimer("Zip Up Folder for Archive");

                // First zip the folder up so we can just archive the Zip
                Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
                zip.AddDirectory(fullFolderPath);
                string zipFullName = fullFolderPath + ".zip";
                zip.Save(zipFullName);

                methodTimer.LogTimeAndDispose("Zip Up Folder for Archive");
                methodTimer.StartTimer("Archive Zipped Up Folder");

                // Archive the Zip
                if (0 != new FileArchiverWrapper().ArchiveFile(zipFullName, 1, "Admin Deleted Folder: " + Namelbl.Text))
                {
                    // then delete the Folder finally
                    Directory.Delete(fullFolderPath, true);

                    this.MpeWebMessage.Show();
                    this.LitMessage.Text = Namelbl.Text + " has been DELETED.";
                }

                methodTimer.LogTimeAndDispose("Archive Zipped Up Folder");
                // The grid view already has timming on it

                this.loadFiles();

            }
        }

        private ILog Log = LogManager.GetLogger(typeof(IRS1095PDFStaging));

    }
}
