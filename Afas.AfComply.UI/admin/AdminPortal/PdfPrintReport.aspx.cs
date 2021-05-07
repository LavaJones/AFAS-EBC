using System;
using System.Web;
using System.Data.SqlClient;
using log4net;
using System.Data;
using System.Configuration;
using Afas.Domain;
using Afas.AfComply.Reporting.Application;
using Afc.Core.Application;
using Afas.AfComply.UI.App_Start;
using System.Linq;
using System.Collections.Generic;
using Afas.AfComply.Reporting.Domain.Printing;
using System.Diagnostics;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class PdfPrintReport : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(PdfPrintReport));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the PdfPrintReport page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=18", false);
            }
        }
        

        protected void ImgBtnExportCSV_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            DataTable export = new DataTable();

            var PrintService = ContainerActivator._container.Resolve<IPrintBatchService>();
            var Context = ContainerActivator._container.Resolve<ITransactionContext>();
            PrintService.Context = Context;

            List<PrintBatch> allPrints = PrintService.GetAllPrintBatchesAndPrints().ToList();
            List<employer> allEmployers = employerController.getAllEmployers();

            watch.Stop();
            Log.Debug("Loading all the Print Batches took: ["+ watch .ElapsedMilliseconds+ "] ms");
            watch.Reset();
            watch.Start();

            export.Columns.Add("FEIN", typeof(string));
            export.Columns.Add("EmployerId", typeof(string));
            export.Columns.Add("TaxYear", typeof(string));
            export.Columns.Add("EmployerName", typeof(string));
            export.Columns.Add("Reprint", typeof(string));
            export.Columns.Add("EmpCount", typeof(string));
            export.Columns.Add("RequestedBy", typeof(string));
            export.Columns.Add("RequestedOn", typeof(string));
            export.Columns.Add("SentOn", typeof(string));
            export.Columns.Add("CreatedBy", typeof(string));
            export.Columns.Add("CreatedDate", typeof(string));
            export.Columns.Add("ModifiedBy", typeof(string));
            export.Columns.Add("ModifiedDate", typeof(string));
            export.Columns.Add("PrintFileName", typeof(string)); 
            export.Columns.Add("AfasRequested", typeof(string));
            export.Columns.Add("PdfReceivedOn", typeof(string));


            //Stopwatch shortWatch = new Stopwatch();
            foreach (PrintBatch batch in allPrints)
            {

                DataRow row = export.NewRow();

                employer current = allEmployers.Where(emp => emp.EMPLOYER_ID == batch.EmployerId).First();
                
                row["FEIN"] = current.EMPLOYER_EIN;
                row["EmployerId"] = current.EMPLOYER_ID;
                row["TaxYear"] = batch.TaxYear;
                row["EmployerName"] = current.EMPLOYER_NAME;
                row["Reprint"] = batch.Reprint.ToString();
                row["EmpCount"] = batch.AllPrinted1095s.Count;
                row["RequestedBy"] = batch.RequestedBy;
                row["RequestedOn"] = batch.RequestedOn.ToShortDateString() + " " + batch.RequestedOn.ToShortTimeString();
                row["SentOn"] = batch.SentOn.ToShortDateString() + " " +  batch.SentOn.ToShortTimeString();
                row["CreatedBy"] = batch.CreatedBy; 
                row["CreatedDate"] = batch.CreatedDate.ToShortDateString() + " " + batch.CreatedDate.ToShortTimeString();
                row["ModifiedBy"] = batch.ModifiedBy;
                row["ModifiedDate"] = batch.ModifiedDate.ToShortDateString() + " " + batch.ModifiedDate.ToShortTimeString();
                row["PrintFileName"] = batch.PrintFileName;
                row["AfasRequested"] = batch.AfasRequested.ToString();
                row["PdfReceivedOn"] = "";
                if (batch.PdfReceivedOn != null)
                {
                    row["PdfReceivedOn"] = ((DateTime)batch.PdfReceivedOn).ToShortDateString() +" "+ ((DateTime)batch.PdfReceivedOn).ToShortTimeString();
                }
                
                export.Rows.Add(row);

            }

            watch.Stop();
            Log.Debug("Building CSV took: [" + watch.ElapsedMilliseconds + "] ms");
            watch.Reset();
            watch.Start();

            // Next 4 lines of Code from internet : http://stackoverflow.com/questions/1746701/export-datatable-to-excel-file
            string filename = "PdfPrintReport";
            string attachment = "attachment; filename="+ filename.CleanFileName() + ".csv";
            Response.ClearContent();
            Response.BufferOutput = false;
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";

            Response.Write(export.GetAsCsv());

            // https://stackoverflow.com/questions/20988445/how-to-avoid-response-end-thread-was-being-aborted-exception-during-the-exce
            Response.Flush(); // Sends all currently buffered output to the client.
            Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.

            watch.Stop();
            Log.Debug("Sending the CSV took : [" + watch.ElapsedMilliseconds + "] ms");

            Response.End();

        }
    }
}