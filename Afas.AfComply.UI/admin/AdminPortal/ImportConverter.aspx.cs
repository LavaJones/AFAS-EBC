using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using log4net;

using Afas.AfComply.Application;

using Afas.AfComply.Domain;
using Afas.Domain;
using Afas.Application.CSV;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class ImportConverter : AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(ImportConverter));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.BulkConverterEnabled)
            {
                Log.Info("A user tried to access the Bulk ImportConverter page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=26", false);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnUploadFile_Click(object sender, EventArgs e)
        {

            lblMsg.Text = "";

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

                lblMsg.Text = "Incorrect parameters, did you select an employer?";

                return;

            }

            FileArchiverWrapper archive = new FileArchiverWrapper();

            long millis = DateTime.Now.Ticks / (long)TimeSpan.TicksPerMillisecond;

            employer employ = employerController.getEmployer(employerId);

            String ftpPath = Server.MapPath("~\\ftps\\");

            if (DemographicsFile.HasFile)
            {

        
                String demographicsPath = ftpPath + "ConvertFrom\\" + employ.EMPLOYER_ID + "_Demographics_" + millis + ".csv";

                DemographicsFile.SaveAs(demographicsPath);

                PIILogger.LogPII(string.Format("User [{0}] Imported DemographicsFile File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, DemographicsFile.FileName, demographicsPath));

                String postConvertPath = String.Format("{0}{1}_{2}_Demographics.csv", ftpPath, employ.EMPLOYER_IMPORT_EMPLOYEE, millis);

                FileImport("Demographics", demographicsPath, postConvertPath, employ.EMPLOYER_EIN);

                PIILogger.LogPII(string.Format("User [{0}] Converted DemographicsFile File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, demographicsPath, postConvertPath));

                archive.ArchiveFile(demographicsPath, employ.EMPLOYER_ID, "Bulk Import Converter Automatic Demographics Processing");

            }

            if (CoverageFile.HasFile)
            {

                String coveragePath = ftpPath + "ConvertFrom\\" + employ.EMPLOYER_ID + "_Ins_Coverage_" + millis + ".csv";

                CoverageFile.SaveAs(coveragePath);

                PIILogger.LogPII(string.Format("User [{0}] Imported CoverageFile File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, CoverageFile.FileName, coveragePath));

                string postConvertPath = String.Format("{0}inscarrier\\{1}_{2}_Ins_Coverage.csv", ftpPath, employ.EMPLOYER_IMPORT_IC, millis);

                FileImport("Coverage", coveragePath, postConvertPath, employ.EMPLOYER_EIN);

                PIILogger.LogPII(string.Format("User [{0}] Converted CoverageFile File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, coveragePath, postConvertPath));

                archive.ArchiveFile(coveragePath, employ.EMPLOYER_ID, "Bulk Import Converter Automatic Coverage Processing");

            }

            if (PayrollFile.HasFile)
            {

                String payrollPath = ftpPath + "ConvertFrom\\" + employ.EMPLOYER_ID + "_Payroll_Hours_" + millis + ".csv";

                PayrollFile.SaveAs(payrollPath);

                PIILogger.LogPII(string.Format("User [{0}] Imported PayrollFile File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, PayrollFile.FileName, payrollPath));

                string postConvertPath = String.Format("{0}{1}_{2}_Payroll.csv", ftpPath, employ.EMPLOYER_IMPORT_PAYROLL, millis);

                FileImport("Payroll", payrollPath, postConvertPath, employ.EMPLOYER_EIN);

                PIILogger.LogPII(string.Format("User [{0}] Converted PayrollFile File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, payrollPath, postConvertPath));

                archive.ArchiveFile(payrollPath, employ.EMPLOYER_ID, "Bulk Import Converter Automatic Payroll Processing");

            }

            if (lblMsg.Text.Contains("did not match"))
            {
                lblMsg.Text = "<br/><span style='color: red;font-weight: bold;'>This file does NOT belong to this employer. Contact your manager for assistance!</span><br/>" + lblMsg.Text;
            }

        }

        private void FileImport(String importName, String sourceFileName, String destinationFileName, String ein)
        {

            int i = 0;
            FileInfo file = new FileInfo(sourceFileName);
            DataTable dataTable = new DataTable("CSVTable");

            dataTable.Clear();

            try
            {

                String[] source = File.ReadAllLines(sourceFileName);
                String[] headers = CsvParse.SplitRow(source[0]);

                headers = CsvHelper.SanitizeHeaderValues(headers);

                foreach (String header in headers)
                {
                    dataTable.Columns.Add(header);
                }

                foreach (String row in source.Skip(1))
                {


                    DateTimeStyles styles = DateTimeStyles.None;
                    CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                    String[] formats = { "yyyy/MM/dd", "MM/dd/yyyy hh:mm" };
                    String[] rowValues = CsvParse.SplitRow(row);
                    DataRow datarow = dataTable.NewRow();

                    for (i = 0; i < rowValues.Length; i++)
                    {

                        DateTime time;

                        if (DateTime.TryParseExact(rowValues[i], formats, culture, styles, out time))
                        {
                            datarow[headers[i]] = time;
                        }
                        else
                        {
                            datarow[headers[i]] = rowValues[i];
                        }

                    }

                    dataTable.Rows.Add(datarow);

                }

 
                foreach (DataColumn dataColumn in dataTable.Columns)
                {

                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        dataRow[dataColumn] = dataRow[dataColumn].ToString().TrimDoubleQuotes();
                    }

                }

                RemoveUserDefinedColumns(dataTable);

                ImportExcelConverter converter = new ImportExcelConverter();
                converter.CreateCSVTables(dataTable, sourceFileName, ein);

                if (sourceFileName.ToLower().Contains("demographic") == false)
                {

                  
                    if (dataTable.Columns.Contains("Check Date"))
                    {

                        foreach (DataRow dataRow in dataTable.Rows)
                        {

                            if (dataRow["Pay Description ID"].ToString().IsNullOrEmpty())
                            {
                                dataRow["Pay Description ID"] = "01";
                            }

                            if (dataRow["Pay Description"].ToString().IsNullOrEmpty())
                            {
                                dataRow["Pay Description"] = Branding.ProductName;
                            }

                            if (dataRow["Check Date"].ToString().IsNullOrEmpty())
                            {
                                dataRow["Check Date"] = "19200101";
                            }

                        }

                    }

                    dataTable.WriteOutCsv(destinationFileName);

                }
                else
                {


                   
                    var distinctEmployeeTypes = (from DataRow distinctDataRow in dataTable.Rows
                                                 select distinctDataRow["EEType"]
                                                ).Distinct().ToList();

                    
                    if (distinctEmployeeTypes.Count() == 1 && distinctEmployeeTypes.First().ToString().Trim().Length == 0)
                    {

                        foreach(DataRow dataRow in dataTable.Rows)
                        {
                            dataRow["EEType"] = "All Employees";
                        }

                        distinctEmployeeTypes = (from DataRow distinctDataRow in dataTable.Rows
                                                 select distinctDataRow["EEType"]
                                                ).Distinct().ToList();

                    }

                    foreach (String employeeType in distinctEmployeeTypes)
                    {

                       
                        String employeeTypeFileName = String.Format(
                                "{0}-{1}.csv", 
                                destinationFileName.Replace(".csv", String.Empty), 
                                ConverterHelper.BuildSafeFilename(employeeType)
                            );

                        
                        DataTable filteredDataTable = (from r in dataTable.AsEnumerable() where r.Field<String>("EEType") == employeeType select r).CopyToDataTable();

                        WriteoutDemographicFiles(filteredDataTable, employeeTypeFileName);

                    }

                }

                lblMsg.Text = lblMsg.Text + " \n" + importName + " SUCCESS! ";

            }
            catch (Exception exception)
            {

                this.Log.Error("Issues during file conversion.", exception);

                lblMsg.Text = lblMsg.Text + " \n" + importName + " Error : " + exception.Message;

            }

        }

        protected void DdlEmployer_SelectedIndexChanged(Object sender, EventArgs e)
        {

            int employerId = 0;

       
            if (
                    null == DdlEmployer.SelectedItem
                        ||
                    null == DdlEmployer.SelectedItem.Value
                        ||
                    false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
                )
            {

                lblMsg.Text = "Incorrect parameters";

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            cofein.Text = employ.EMPLOYER_EIN;

        }

        private void RemoveUserDefinedColumns(DataTable dataTable)
        {

            List<int> list = new List<int>();
            List<DataColumn> dataColumns = new List<DataColumn>();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {

                if (dataColumn.ColumnName.StartsWith("`") && dataColumn.ColumnName.EndsWith("`"))
                {
                    dataColumns.Add(dataColumn);
                }
            
            }

            foreach (DataColumn dataColumn in dataColumns)
            {
                dataTable.Columns.Remove(dataColumn);
            }

        }

        /// <summary>
        /// Handle the writing of the demographic data by Employee Classes for the passed dataTable.
        /// </summary>
        public void WriteoutDemographicFiles(DataTable dataTable, String destinationFileName)
        {

            dataTable.Columns.RemoveColumnIfExists("EEType");

            var distinctHrStatusIds = (from DataRow distinctDataRow in dataTable.Rows
                                       select distinctDataRow["HR Status Code"]
                                      ).Distinct().ToList();

            foreach (String hrStatusId in distinctHrStatusIds)
            {

                
                if (hrStatusId.Equals("-E3B0C44298FC1C149AF", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                
                DataTable filteredDataTable = (from r in dataTable.AsEnumerable() where r.Field<String>("HR Status Code") == hrStatusId select r).CopyToDataTable();

                String safeFilename = ConverterHelper.BuildSafeFilename(filteredDataTable.Rows[0]["HR Status Description"].ToString());

                
                String filteredFilename = String.Format("{0}-{1}.csv", destinationFileName.Replace(".csv", String.Empty), safeFilename);

                
                foreach (DataRow dataRow in filteredDataTable.Rows)
                {

                    dataRow["HR Status Code"] = "01";

                    dataRow["HR Status Description"] = Branding.ProductName;

                    if (dataRow["DOB"].ToString().IsNullOrEmpty())
                    {
                        dataRow["DOB"] = "19200101";
                    }

                    if (dataRow["SSN"].ToString().Contains("-"))
                    {
                        dataRow["SSN"] = dataRow["SSN"].ToString().Replace("-", String.Empty);
                    }

                    if (dataRow["Employee #"].ToString().IsNullOrEmpty())
                    {

                        dataRow["Employee #"] = ConverterHelper.BuildDefaultEmployeeNumber(
                                Branding.ProductName,
                                dataRow["First_Name"].ToString(),
                                dataRow["Last_Name"].ToString(),
                                dataRow["SSN"].ToString()
                            );

                    }

                }

                filteredDataTable.WriteOutCsv(filteredFilename);

            }

        }

    }

}