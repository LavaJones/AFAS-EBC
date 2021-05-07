using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Afas.Domain;
using Afas.AfComply.UI.App_Start;
using System.Diagnostics;
using Afas.AfComply.Reporting.Application.Ssis;
using Afc.Core.Application;
using Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer;
using System.Data;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class SsisFileTransfers : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(SsisFileTransfers));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the SsisFileTransfer  page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=18", false);
            }
        }


        protected void ImgBtnExportCSV_Click(object sender, EventArgs e)
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();
            DateTime _startDate = System.DateTime.Now.AddYears(-50);
            DateTime _endDate = System.DateTime.Now.AddYears(-50);

            bool ValidData = true;
            ValidData = errorChecking.validateTextBoxDate(Txt_Start_Date, ValidData);
            ValidData = errorChecking.validateTextBoxDate(Txt_End_Date, ValidData);

            if (ValidData == true)
            {
                _startDate = DateTime.Parse(Txt_Start_Date.Text);
                _endDate = DateTime.Parse(Txt_End_Date.Text);

                var FileTransferService = ContainerActivator._container.Resolve<ISsisFileTransferService>();
                var Context = ContainerActivator._container.Resolve<ITransactionContext>();
                FileTransferService.Context = Context;
                List<SsisFileTransfer> transferredFiles = FileTransferService.GetFileTransferredThroughSsis(_startDate, _endDate).ToList();

                watch.Stop();
                Log.Debug("Loading all the Ssis Files transferred  took: [" + watch.ElapsedMilliseconds + "] ms");
                watch.Reset();
                watch.Start();
                DataTable export = new DataTable();
                export.Columns.Add("FEIN", typeof(string));
                export.Columns.Add("FileName", typeof(string));
                export.Columns.Add("RunTime", typeof(string));
                export.Columns.Add("EmployerId", typeof(string));
                export.Columns.Add("ResourceId", typeof(string));
                export.Columns.Add("EntityStatusId", typeof(string));
                export.Columns.Add("CreatedBy", typeof(string));
                export.Columns.Add("CreatedDate", typeof(string));
                export.Columns.Add("ModifiedBy", typeof(string));
                export.Columns.Add("ModifiedDate", typeof(string));
                foreach (SsisFileTransfer transfer in transferredFiles)
                {
                    DataRow row = export.NewRow();
                    row["FEIN"] = transfer.FEIN;
                    row["FileName"] = transfer.FileName;
                    row["RunTime"] = transfer.RunTime.ToShortDateString() + " " + transfer.RunTime.ToShortTimeString();
                    row["EmployerId"] = transfer.EmployerId;
                    row["ResourceId"] = transfer.ResourceId;
                    row["EntityStatusId"] = transfer.EntityStatus;
                    row["CreatedBy"] = transfer.CreatedBy;
                    row["CreatedDate"] = transfer.CreatedDate.ToShortDateString() + " " + transfer.CreatedDate.ToShortTimeString();
                    row["ModifiedBy"] = transfer.ModifiedBy;
                    row["ModifiedDate"] = transfer.ModifiedDate.ToShortDateString() + " " + transfer.ModifiedDate.ToShortTimeString();
                    export.Rows.Add(row);


                }

                watch.Stop();
                Log.Debug("Building CSV took: [" + watch.ElapsedMilliseconds + "] ms");
                watch.Reset();
                watch.Start();

                // Next 4 lines of Code from internet : http://stackoverflow.com/questions/1746701/export-datatable-to-excel-file
                string filename = "PdfPrintReport";
                string attachment = "attachment; filename=" + filename.CleanFileName() + ".csv";
                Response.ClearContent();
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
}