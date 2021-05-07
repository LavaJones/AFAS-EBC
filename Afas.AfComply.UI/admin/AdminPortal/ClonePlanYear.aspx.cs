using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afc.Core.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class ClonePlanYear : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(ClonePlanYear));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the Clone PlanYear page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=9", false);
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

        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
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

                cofein.Text = "Incorrect parameters";

                return;

            }

            employer currEmployer = employerController.getEmployer(employerId);
            cofein.Text = currEmployer.EMPLOYER_EIN;
            HfDistrictID.Value = currEmployer.EMPLOYER_ID.ToString();

            loadPlanYears(currEmployer.EMPLOYER_ID);
        }

        private void loadPlanYears(int _employerID)
        {
            var planyear =  PlanYear_Controller.getEmployerPlanYear(_employerID);
            
            //Load the Employer Plan Years. 
            DdlPlanYear.DataSource = planyear;
            DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYear.DataBind();
            DdlPlanYear.Items.Add("Select");
            DdlPlanYear.SelectedIndex = DdlPlanYear.Items.Count - 1;
        }

        protected void BtnClone_Click(object sender, EventArgs e)
        {
            int planYearId = 0;
            int employerId = 0;
            int Change = 0;
            int.TryParse(DdlPlanYear.SelectedValue, out planYearId);
            int.TryParse(HfDistrictID.Value, out employerId);
            int.TryParse(FuturePrevious.SelectedValue, out Change);

            PlanYear planYear = PlanYear_Controller.findPlanYear(planYearId, employerId);

            if (planYear != null && (Change == -1 || Change == 1)) 
            {
               
                bool success = true;
                User curr = (User) Session["CurrentUser"];

                //First create the new plan year
                int newId = PlanYear_Controller.manufactureNewPlanYear(
                        employerId,
                        FuturePrevious.SelectedItem.Text,
                        (DateTime)ChangeYear(planYear.PLAN_YEAR_START, Change),
                        (DateTime)ChangeYear(planYear.PLAN_YEAR_END, Change),
                        "Cloned From: " + planYear.PLAN_YEAR_ID, 
                        "", 
                        DateTime.Now, 
                        curr.User_UserName,
                        ChangeYear(planYear.Default_Meas_Start, Change),
                        ChangeYear(planYear.Default_Meas_End, Change),
                        ChangeYear(planYear.Default_Admin_Start, Change),
                        ChangeYear(planYear.Default_Admin_End, Change),
                        ChangeYear(planYear.Default_Open_Start, Change),
                        ChangeYear(planYear.Default_Open_End, Change),
                        ChangeYear(planYear.Default_Stability_Start, Change),
                        ChangeYear(planYear.Default_Stability_End, Change),
                        planYear.PlanYearGroupId
                    );

                PlanYear newYear = PlanYear_Controller.findPlanYear(newId, employerId);
                if (newYear == null) 
                {
                    success = false;
                    Log.Error("We tried to clone a plan year but it failed.");                
                }

                List<Measurement> pyMeasurements = measurementController.manufactureMeasurementList(employerId);
                pyMeasurements = pyMeasurements.Where(meas => meas.MEASUREMENT_PLAN_ID == planYear.PLAN_YEAR_ID).ToList();

                foreach (Measurement measurement in pyMeasurements)
                {
                    // loop thourgh each Measurement period and clone them 

                    success = success && CloneMeasurementPeriod(measurement, newId, Change);
                   
                }

                DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;

                if (success)
                {
                    // Queue up the calculation
                    employerController.insertEmployerCalculation(employerId);

                    lblMsg.Text = "Cloning was a Success, Please inspect the Data for correctness.";
                }
                else 
                {
                    Log.Error("Hit 'Contact IT' Error in [BtnClone_Click]");
                    lblMsg.Text = "An error occurred, please Contact IT.";
                }
            }
        }

        public bool CloneMeasurementPeriod(Measurement measurement, int newPlanId, int Change)
        {
            bool success = true;
            User curr = (User) Session["CurrentUser"];

            int newId = measurementController.manufactureNewMeasurement(
                measurement.MEASUREMENT_EMPLOYER_ID,
                newPlanId,
                measurement.MEASUREMENT_EMPLOYEE_TYPE_ID,
                measurement.MEASUREMENT_TYPE_ID,
                (DateTime)ChangeYear(measurement.MEASUREMENT_START, Change),
                (DateTime)ChangeYear(measurement.MEASUREMENT_END, Change),
                (DateTime)ChangeYear(measurement.MEASUREMENT_ADMIN_START, Change),
                (DateTime)ChangeYear(measurement.MEASUREMENT_ADMIN_END, Change),
                (DateTime)ChangeYear(measurement.MEASUREMENT_OPEN_START, Change),
                (DateTime)ChangeYear(measurement.MEASUREMENT_OPEN_END, Change),
                (DateTime)ChangeYear(measurement.MEASUREMENT_STAB_START, Change),
                (DateTime)ChangeYear(measurement.MEASUREMENT_STAB_END, Change),
                measurement.MEASUREMENT_NOTES,
                curr.User_UserName,
                DateTime.Now,
                "Cloned From: " + measurement.MEASUREMENT_ID, 
                null, null, null, null);

            if (newId == 0)
            {
                success = false;
                Log.Error("Tried to clone a Measurement Period but failed");
            }            

            List<BreakInService> breaks = new BreakInServiceFactory().SelectBreaksInService(measurement.MEASUREMENT_ID);

            foreach(BreakInService bis in breaks)
            {
                // loop through all breaks in service and clone them

                success = success && CloneBreakInService(bis, newId, Change);

            }

            return success;
        }

        private bool CloneBreakInService(BreakInService bis, int newMeasId, int Change)
        {
            User curr = (User)Session["CurrentUser"];

            BreakInService insert = new BreakInService();
            insert.CreatedBy = curr.User_UserName;
            //insert.CreatedDate = DateTime.Now;
            insert.EndDate = (DateTime)ChangeYear(bis.EndDate, Change);
            insert.EntityStatus = (EntityStatusEnum) 1;
            insert.ModifiedBy = curr.User_UserName;
            insert.ModifiedDate = DateTime.Now;
            insert.StartDate = (DateTime)ChangeYear(bis.StartDate, Change);

            if (false == new BreakInServiceFactory().InsertNewBreakInService(newMeasId, insert)) 
            {
                Log.Error("Attempted to clone a Break Inservice but failed.");
                return false;
            }

            return true;
        }



        /// <summary>
        /// Changes the Date to Plus or minus 'yearChange' years, acounting for leap days
        /// </summary>
        /// <param name="current">Date to change</param>
        /// <param name="yearChange">Amount to change by</param>
        /// <returns>THe new date that was change by 'yearChange' years</returns>
        private DateTime? ChangeYear(DateTime? start, int yearChange) 
        {
            if (start == null)
                return null;
            DateTime current = (DateTime)start;
            int year = current.Year + yearChange;
            bool isLeap = DateTime.IsLeapYear(year);
            if (isLeap && current.Month == 2 && current.Day == 28)
            {
                return new DateTime(year, 2, 29);
            }
            else if (false == isLeap && current.Month == 2 && current.Day == 29)
            {
                return new DateTime(year, 2, 28);
            }
            else
            {
                return new DateTime(year, current.Month, current.Day);
            }
        }
    }
}