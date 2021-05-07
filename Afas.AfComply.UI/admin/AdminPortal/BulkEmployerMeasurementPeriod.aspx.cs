using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class BulkEmployerMeasurementPeriod : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(BulkEmployerMeasurementPeriod));
        
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.BulkImportEnabled)
            {
                Log.Info("A user tried to access the Bulk EmployerMeasurementPeriod page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=6", false);
            }
        }

        protected void BtnUploadFile_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("This code is no longer suitable for use.");

            string _filePath = Server.MapPath("~\\ftps\\");
            _filePath += "Measurement_Period_Bulk_Import.csv";
            if (MeasurementFile.HasFile)
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
                    new FileArchiverWrapper().ArchiveFile(_filePath, 1, "Bulk Upload File Collision");
                }

                Log.Info("Saving Bulk Import File as: " + _filePath);
                MeasurementFile.SaveAs(_filePath);

                Log.Info("Processing Bulk Import File: " + _filePath);
                //we need to process the file now
                string results = ProcessFile(_filePath);
                Log.Info("Completed Processing Bulk Import File, results: " + results);

                Log.Info("Archiving Bulk Import File:" + _filePath);
                new FileArchiverWrapper().ArchiveFile(_filePath, 1, "Bulk Upload");


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

            try
            {
                string ein = split[2].Trim();
                if (ein != null && ein != string.Empty && false == ein.Contains("-"))
                {
                    //fix formatting of ein
                    ein = ein.Substring(0, 2) + "-" + ein.Substring(2, ein.Length - 2);
                }

                if (employers == null || employers.Count <= 0)
                {
                    //build the list if it is not yet built
                    employers = employerController.getAllEmployers();
                }

                //check to see if the DB already holds this record
                var existing = (from emp in employers where emp.EMPLOYER_EIN.Equals(ein) select emp).FirstOrDefault();

                if (existing != null)
                {//Only create if we have an employer in the DB
                    int _employerID = existing.EMPLOYER_ID;

                    //NOTE mostly dredged from s_setup

                    int _planID = 0;
                    int _employeeTypeID = 0;
                    int _measurementTypeID = 0;

                    DateTime _meas_start = System.DateTime.Now.AddYears(-50);
                    DateTime _meas_end = System.DateTime.Now.AddYears(-50);
                    DateTime _admin_start = System.DateTime.Now.AddYears(-50);
                    DateTime _admin_end = System.DateTime.Now.AddYears(-50);
                    DateTime _open_start = System.DateTime.Now.AddYears(-50);
                    DateTime _open_end = System.DateTime.Now.AddYears(-50);
                    DateTime _stab_start = System.DateTime.Now.AddYears(-50);
                    DateTime _stab_end = System.DateTime.Now.AddYears(-50);
                    string _notes = null;
                    DateTime _modOn = DateTime.Now;
                    string _modBy = "BulkImport";
                    string _history = null;
                    bool validTransaction = false;
                    int measurementID = 0;
                    DateTime? _swStart = null;
                    DateTime? _swEnd = null;
                    DateTime? _swStart2 = null;
                    DateTime? _swEnd2 = null;

                    //Measurement Period Start Date
                    string MeasurementStart = split[3].Trim();
                    //Measurement Period End Date
                    string MeasurementEnd = split[4].Trim();
                    //Administrative Period Start Date
                    string AdministrativeStart = split[5].Trim();
                    //Administrative Period End Date
                    string AdministrativeEnd = split[6].Trim();
                    //Stability Period Start Date
                    string StabilityStart = split[7].Trim();
                    //Stability Period End Date
                    string StabilityEnd = split[8].Trim();


                    bool validData = true;
                    //Error check all fields. 
                    //validData = errorChecking.validateDropDownSelection(Ddl_M_PlanYear, validData);
                    //validData = errorChecking.validateDropDownSelection(Ddl_M_EmployeeType, validData);
                    //validData = errorChecking.validateDropDownSelection(Ddl_M_MeasurementType, validData);
                    validData = validData && AdministrativeEnd.IsValidDate();
                    validData = validData && AdministrativeStart.IsValidDate();
                    validData = validData && MeasurementEnd.IsValidDate();
                    validData = validData && MeasurementStart.IsValidDate();
                    validData = validData && StabilityEnd.IsValidDate();
                    validData = validData && StabilityStart.IsValidDate();

                    if (validData)
                    {
                        _meas_end = System.Convert.ToDateTime(MeasurementEnd);
                        _meas_start = System.Convert.ToDateTime(MeasurementStart);
                        _admin_end = System.Convert.ToDateTime(AdministrativeEnd);
                        _admin_start = System.Convert.ToDateTime(AdministrativeStart);
                        //Per dylan: open enrollment dates will match the admin period dates.
                        _open_end = System.Convert.ToDateTime(AdministrativeEnd);
                        _open_start = System.Convert.ToDateTime(AdministrativeStart);
                        _stab_end = System.Convert.ToDateTime(StabilityEnd);
                        _stab_start = System.Convert.ToDateTime(StabilityStart);
                        _notes = null;
                        _history = "Created on " + _modOn.ToString() + " by " + _modBy;

                        //should match the stability period start year 
                        //should all be 2015
                        var planyears = PlanYear_Controller.getEmployerPlanYear(_employerID);
                        _planID = (from plan in planyears where plan.PLAN_YEAR_START != null && ((DateTime)plan.PLAN_YEAR_START).Year == 2015 select plan.PLAN_YEAR_ID).FirstOrDefault();

                        if (_planID == 0)
                        {
                            Log.Info(String.Format("Measurement Period Plan not found for EIN: [{0}]", ein));
                            results += String.Format("Measurement Period Plan not found for EIN: [{0}]\n", ein);
                        }

                        _employeeTypeID = EmployeeTypeController.getEmployeeTypes(_employerID).FirstOrDefault().EMPLOYEE_TYPE_ID;//only one choice
                        _measurementTypeID = 2;// we asssume 

                        //NOTE: Ignore Summer window for import

                        //Send data to the database. 
                        //yes this is wrong, it will be fixed later
                        measurementID = measurementController.manufactureNewMeasurement(_employerID, _planID, _employeeTypeID, _measurementTypeID, _meas_start, _meas_end, _admin_start, _admin_end, _open_start, _open_end, _stab_start, _stab_end, _notes, _modBy, _modOn, _history, _swStart, _swEnd, _swStart2, _swEnd2);

                        if (0 == measurementID)
                        {
                            Log.Info(String.Format("Import Measurement Periods Failed: EIN: [{0}]", ein));
                            results += String.Format("Failed Measurement Period Insert (Duplicate?): EIN: [{0}]\n", ein);
                        }
                    }
                    else
                    {
                        Log.Info(String.Format("Validation Failed Measurement Period: EIN: [{0}] Measurement:[{1}]", ein, MeasurementStart));
                        results += String.Format("Validation Failed Classification: EIN: [{0}] Measurement:[{1}]\n", ein, MeasurementStart);
                    }
                }
                else
                {
                    Log.Info("Tried to import Measurement Periods for employer that didn't exist with EIN: " + ein);
                    results += "No Employer found with EIN: [" + ein + "];\n";
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exception while Importing row [" + fullRow + "]", ex);
                results += "failed to Import row [" + fullRow + "];\n";
            }

            return results;
        }
    }
}