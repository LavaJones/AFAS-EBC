using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Afas.AfComply.Domain;
using log4net;
using System.Text;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.Domain.POCO;
using System.Web.Mvc;
using Afc.Core;
using Afc.Core.Presentation.Web;
using Afas.AfComply.Reporting.Core.Response;
using Afc.Marketing.Response;
using Afc.Marketing;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using AutoMapper;
using Afas.AfComply.UI.Areas.ViewModels;
using Afas.AfComply.UI.Areas.Administration.Controllers;
using Afas.AfComply.UI.Areas.ViewModels.Reporting;
using Afas.AfComply.UI.Areas.ViewModels.Enums;
using Afc.Framework.Presentation.Web;
using System.Diagnostics;
using System.Data;
using Afas.Domain;
using Afas.AfComply.UI.App_Start;
using Afas.Application.Archiver;
using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Application.Services;
using Afas.AfComply.Reporting.Application;
using Afc.Core.Application;
using Afas.Domain.POCO;
using Afas.AfComply.Reporting.Domain.Printing;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class _1094_Print : AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(_1094_Print));
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the 1094 Print page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=14", false);
            }

        }
        protected void Print1094_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var Final1094Service = ContainerActivator._container.Resolve<IFinalize1094Service>();
            var PrintService = ContainerActivator._container.Resolve<IPrintBatchService>();
            var Archiver = ContainerActivator._container.Resolve<IFileArchiver>();
            var Context = ContainerActivator._container.Resolve<ITransactionContext>();
            Final1094Service.Context = Context;
            PrintService.Context = Context;
            List<employer> employerItem = employerController.getAllEmployers();
            foreach (employer employer in employerItem)
            {
                int employerId = employer.EMPLOYER_ID; ;//this'll work for client side, but not dmin side//int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
                int taxYear = int.Parse(DdlYears.SelectedValue);
                Employee1095PrintRequests request = new Employee1095PrintRequests();
                request.EmployerId = employerId;
                request.TaxYear = taxYear;
                request.Correction = false;

                request.Requester = ((User)Session["CurrentUser"]).User_UserName;

                // Get the current prints and only print unprinted approves. 
                List<Approved1094FinalPart1> tempList = Final1094Service.GetApproved1094sForEmployerTaxYear(request.EmployerId, request.TaxYear);



              
                List<Print1094> printed = PrintService.GetForEmployerTaxYear(request.EmployerId, request.TaxYear).Select(printbatch => printbatch.AllPrinted1094s.ToList()).SelectMany(item => item).ToList();

                tempList.RemoveAll(x => printed.Any(y => y.ID == x.ID));





                foreach (Approved1094FinalPart1 part1 in tempList)
                {

                    employer employ = employerController.getEmployer(request.EmployerId);
                    Guid EmployerGuid = employ.ResourceId;

                    // Generate and save the file
                    string content = PrintFileGenerator.Generate1094PrintCSVContent(request.Correction, "1094C", part1);

                    var folder_path = System.Web.HttpContext.Current.Server.MapPath("~/ftps/Print1094/");
                    string tempFolder = System.Web.HttpContext.Current.Server.MapPath("~/ftps/Scratch/");

                    long millis = DateTime.Now.Ticks / (long)TimeSpan.TicksPerMillisecond;
                    string file_name = string.Format(@"{0}_{1}_{2}.txt", "1094C", EmployerGuid.ToString(), millis.ToString());

                    // Create the Print Batch
                    PrintBatch batch = new PrintBatch();
                    batch.CreatedBy = request.Requester;
                    batch.ModifiedBy = request.Requester;
                    batch.RequestedBy = request.Requester;
                    batch.RequestedOn = DateTime.Now;
                    batch.ModifiedDate = DateTime.Now;
                    batch.EmployerId = employ.EMPLOYER_ID;
                    batch.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
                    batch.PrintFileName = file_name;
                    batch.TaxYear = request.TaxYear;

                    batch.Reprint = true;
                    batch.AfasRequested = true;

                    // Save the file 
                    int archiveId = PrintFileGenerator.WriteCSVContentToFile(Archiver, EmployerGuid, folder_path, tempFolder, file_name, content, request.EmployerId);

                    // Grab the Archive Object
                
                    ArchiveFileInfo archive = ArchiveFileInfoFactory.GetArchivedFileInfoById(archiveId);
                    batch.ArchivedFile = archive;
                    batch.PrintFileArchivePath = archive.ArchiveFilePath;
                    batch.SentOn = DateTime.Now;


                    Print1094 print1094 = new Print1094();

                    print1094.Approved1094 = part1;
                    print1094.PrintBatch = batch;
                    print1094.ModifiedDate = DateTime.Now;
                    print1094.ModifiedBy = request.Requester;
                    print1094.CreatedBy = request.Requester;
                    print1094.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
                    print1094.OutputFilePath = "TEMP";// this path will be set once we get the files back

                    batch.AllPrinted1094s.Add(print1094);

                    PrintService.SaveBatch(batch, request.Requester);
                }
            }
        }
    }
}