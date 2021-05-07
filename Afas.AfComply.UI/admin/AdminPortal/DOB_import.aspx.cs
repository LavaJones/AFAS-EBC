using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using log4net;

using Afas.AfComply.Application;

using Afas.AfComply.Domain;
using System.Text;
using Afas.Domain;
using Afas.Application.CSV;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class DobImport : AdminPageBase
    {

        private ILog Log = LogManager.GetLogger(typeof(DobImport));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.BulkConverterEnabled)
            {
                Log.Info("A user tried to access the Bulk DobImport page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=12", false);
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

        protected void DdlEmployer_SelectedIndexChanged(Object sender, EventArgs e)
        {

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

                lblMsg.Text = "Incorrect parameters";

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            cofein.Text = employ.EMPLOYER_EIN;

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

            if (DobFile.HasFile)
            {

                String DobPath = ftpPath + "ConvertFrom\\" + employ.EMPLOYER_ID + "_Ins_DOB_" + millis + ".csv";

                DobFile.SaveAs(DobPath);

                PIILogger.LogPII(string.Format("User [{0}] Imported DOBFile File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, DobFile.FileName, DobPath));

                string postConvertPath = String.Format("{0}inscarrier\\{1}_{2}_Ins_DOB.csv", ftpPath, employ.EMPLOYER_IMPORT_IC, millis);

                FileImport(DobPath, employ);

                PIILogger.LogPII(string.Format("User [{0}] Converted DOBFile File [{1}] to [{2}]",
                    ((User)Session["CurrentUser"]).User_UserName, DobPath, postConvertPath));

                archive.ArchiveFile(DobPath, employ.EMPLOYER_ID, "Bulk Import Converter Automatic DOB Processing");

            }
            else
            {
                lblMsg.Text = "<br/><span style='color: red;font-weight: bold;'>You must select a valid file.</span><br/>" + lblMsg.Text;
            }

        }

        private void FileImport(String sourceFileName, employer Employer)
        {

            int i = 0;
            FileInfo file = new FileInfo(sourceFileName);
            DataTable dataTable = new DataTable("CSVTable");

            dataTable.Clear();

            StringBuilder failedRows = new StringBuilder();

            try
            {
                List<Employee_E> allEmployees = EmployeeController.manufactureEmployeeExportList(Employer.EMPLOYER_ID);
                List<Employee_E> ChangedEmployees = new List<Employee_E>();

                DateTimeStyles styles = DateTimeStyles.None;
                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                String[] formats = { "yyyy/MM/dd", "MM/dd/yyyy hh:mm", "yyyyMMdd" };

                String[] source = File.ReadAllLines(sourceFileName);
                String[] headers = CsvParse.SplitRow(source[0]);

                headers = CsvHelper.SanitizeHeaderValues(headers);

                foreach (String header in headers)
                {
                    dataTable.Columns.Add(header);
                }

                foreach (String row in source.Skip(1))
                {

                    //parse each row
                    String[] rowValues = CsvParse.SplitRow(row);
                    DataRow datarow = dataTable.NewRow();

                    for (i = 0; i < rowValues.Length; i++)
                    {
                        datarow[headers[i]] = rowValues[i];
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

                if (false == dataTable.VerifyContainsOnlyThisFederalIdentificationNumber(Employer.EMPLOYER_EIN))
                {
                    this.Log.Warn("FEIN check failed.");
                    lblMsg.Text = "<br/><span style='color: red;font-weight: bold;'>FEIN check failed, please check that the file belongs to the selected Employer.</span><br/>" + lblMsg.Text;
                    return;
                }

                if (dataTable.Columns.Contains("DOB") && dataTable.Columns.Contains("SSN"))
                {
                    int count = 0;
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        count++;
                        if (dataRow["DOB"].ToString().IsNullOrEmpty() || dataRow["SSN"].ToString().IsNullOrEmpty())
                        {
                            if (failedRows.Length > 0) { failedRows.Append(", "); }
                            failedRows.Append(count);

                            continue;
                        }

                        //parse the data and skip if it fails parsing
                        string ssn = dataRow["SSN"].ToString();
                        DateTime dob = new DateTime(1920, 1, 1);
                        if (DateTime.TryParseExact(dataRow["DOB"].ToString(), formats, culture, styles, out dob))
                        {
                            if (ssn.IsValidSsn())
                            {
                                Employee_E emp = (from Employee_E found in allEmployees where found.Employee_SSN_Visible.Equals(ssn) select found).FirstOrDefault();

                                if (null != emp)
                                {
                                    //All checks complete
                                    //change the DOB and add it to the changed list
                                    emp.EMPLOYEE_DOB = dob;
                                    ChangedEmployees.Add(emp);

                                }
                                else
                                {
                                    // no employee found by ssn
                                    if (failedRows.Length > 0) { failedRows.Append(", "); }
                                    failedRows.Append(count);

                                    continue;
                                }
                            }
                            else
                            {
                                // invalid ssn
                                if (failedRows.Length > 0) { failedRows.Append(", "); }
                                failedRows.Append(count);

                                continue;
                            }
                        }
                        else
                        {
                            // invalid date of birth
                            if (failedRows.Length > 0) { failedRows.Append(", "); }
                            failedRows.Append(count);

                            continue;
                        }
                    }

                    if (ChangedEmployees.Count > 0)
                    {

                        foreach (Employee_E employee in ChangedEmployees)
                        {

                            EmployeeController.updateEmployeeLineIII_DOB(
                                    employee.EMPLOYEE_ID, 
                                    DateTime.Now, 
                                    ((User)Session["CurrentUser"]).User_UserName, 
                                    (DateTime)employee.EMPLOYEE_DOB, 
                                    employee.EMPLOYEE_FIRST_NAME, 
                                    employee.EMPLOYEE_LAST_NAME);

                        }

                    }
                    else
                    {
                        Log.Warn("DOB Import did not find any rows to update.");
                        lblMsg.Text = "<br/><span style='color: red;font-weight: bold;'>No Data found to update Date Of Births.</span><br/>" + lblMsg.Text;
                    }

                    if (failedRows.Length > 0)
                    {
                        this.Log.Error("Failed to process rows: " + failedRows.ToString());
                        lblMsg.Text = "<br/><span style='color: red;font-weight: bold;'>File processed, failures on rows:" + failedRows.ToString() + "</span><br/>" + lblMsg.Text;
                    }
                    else
                    {
                        lblMsg.Text = "File Processed Sucessfully.";
                    }

                }
                else
                {
                    this.Log.Error("DOB or SSN not found in file");
                    lblMsg.Text = "<br/><span style='color: red;font-weight: bold;'>File does not contain DOB or SSN Column.</span><br/>" + lblMsg.Text;

                }

            }
            catch (Exception exception)
            {

                this.Log.Error("Issues during file conversion.", exception);

                lblMsg.Text = lblMsg.Text + " \n Date Of Birth Error : " + exception.Message;

            }

        }

    }

}