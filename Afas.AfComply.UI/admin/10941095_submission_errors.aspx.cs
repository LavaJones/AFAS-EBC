using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;
using System.Xml;
using System.Data;
using Afas.AfComply.UI.App_Start;
using Afas.AfComply.Reporting.Core.Request;
using Afas.AfComply.Reporting.Core.Response;
using Afc.Marketing;

namespace Afas.AfComply.UI.admin
{
    public partial class _10941095_submission_errors : Afas.AfComply.UI.admin.AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(_10941095_submission_errors));

        private int currDistId { get { return int.Parse(Request.QueryString["employer"]); } }
        private int etytID { get { return int.Parse(Request.QueryString["status"]); } }
        private int taxyear { get { return int.Parse(Request.QueryString["taxyear"]); } }

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            Server.ScriptTimeout = Transmitter.Timeout * 3;

            Session["employeeErrors"] = null;
            loadEmployeeSubmissionErrors();
            loadEmployerSubmissionErrors();
        }

        #region Load Functions
        /// <summary>
        ///Retrun a list of current employees.
        /// </summary>
        /// <returns></returns>
        private List<Employee> getEmployees()
        {
            List<Employee> tempList = new List<Employee>();
            if (Session["Employees"] == null)
            {
                tempList = EmployeeController.manufactureEmployeeList(currDistId);
                Session["Employees"] = tempList;
            }
            else { tempList = (List<Employee>)Session["Employees"]; }
            return tempList;
        }


        /// <summary>
        /// Load all of the IRS Employee Transmission Errors
        /// </summary>
        public void loadEmployeeSubmissionErrors()
        {
            List<taxYearEmployeeTransmission> tyetList = new List<taxYearEmployeeTransmission>();
            tyetList = airController.manufactureEmployeeTransmissions(etytID);
            tyetList = tyetList.OrderBy(o => o.RecordID).ToList();

            GvErrors.DataSource = tyetList;
            GvErrors.DataBind();
        }

        /// <summary>
        /// Load all of the IRS Employer Transmission Errors
        /// </summary>
        public void loadEmployerSubmissionErrors()
        {
            AckFileXmlReader ackFile = new AckFileXmlReader();
            DataTable dt = ackFile.findEmployerErrorMessageInXML(currDistId, taxyear, etytID);
            int rowCount = dt.Rows.Count;
            GvEmployerErrors.DataSource = dt;
            GvEmployerErrors.DataBind();
        }

        #endregion

        #region button click functions
        /// <summary>
        /// Log user out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }

        /// <summary>
        /// Export data out to .csv file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                List<taxYearEmployeeTransmission> tyetList = airController.manufactureEmployeeTransmissions(etytID);
                List<taxYearEmployerTransmission> tyeetList = airController.manufactureEmployerTransmissions(currDistId, taxyear);
                List<airRequestError> tempList = new List<airRequestError>();

                string fileName = airController.generateIrsSubmissionErrorFile(tyetList, employerController.getEmployer(currDistId), etytID);
                string fileFullPath = Server.MapPath("..\\ftps\\export\\") + fileName;

                string appendText = "attachment; filename=" + fileName;
                Response.ContentType = "file/text";
                Response.AppendHeader("Content-Disposition", appendText);
                Response.TransmitFile(fileFullPath);
                Response.Flush();         
                Response.SuppressContent = true;                
                HttpContext.Current.ApplicationInstance.CompleteRequest();                      
                Response.End();
            }
            catch (Exception ex)
            {
                this.Log.Error("Exception in ImgBtnExport_Click:", ex);
            }
        }

        /// <summary>
        /// Create the corrections records to edit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn1095Corrections_Click(object sender, EventArgs e)
        {
            
            if (etytID <= 0)
            {

                MpeWebMessage.Show();
                LitMessage.Text = "Invalid Transmission Id.";
                return;

            }

            employerController.UnApprove1095NeedingCorrection(etytID);

            Response.Redirect("~/admin/10941095_submission_status.aspx", false);

        }


        protected void btn1095CorrectApprove_Click(object sender, EventArgs e)
        {

            if (etytID <= 0)
            {

                MpeWebMessage.Show();
                LitMessage.Text = "Invalid Transmission Id.";
                return;

            }

            employerController.UnApprove1095NeedingCorrection(etytID);


            EmployerTaxYearRequest request = new EmployerTaxYearRequest();
            request.EmployerId = currDistId;
            request.TaxYear = taxyear;
            request.Requester = ((User)Session["CurrentUser"]).User_UserName;

            var apiHelper = ContainerActivator._container.Resolve<IApiHelper> ();
            UIMessageResponse message = apiHelper.Send<UIMessageResponse, EmployerTaxYearRequest>("Finalize1095-1095summary", request);
            
            Response.Redirect("~/admin/10941095_submission_status.aspx", false);

        }

        #endregion

        #region Gridview Events
        /// <summary>
        /// Gridview Row Databound:
        ///      - This will load the employee names.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvErrors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int employeeID = 0;
            int recordID = 0;
            Employee currEmployee = null;
            GridViewRow row = e.Row;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hfempID = (HiddenField)row.FindControl("Hf_rpt_employeeID");
                HiddenField hfrecordID = (HiddenField)row.FindControl("Hf_rpt_recordID");
                Label lblFname = (Label)row.FindControl("Lbl_rpt_fname");
                Label lblLname = (Label)row.FindControl("Lbl_rpt_lname");
                Label lblEcode = (Label)row.FindControl("Lbl_rpt_error_code");
                Label lblEmessage = (Label)row.FindControl("Lbl_rpt_error_text");

                int.TryParse(hfempID.Value, out employeeID);
                int.TryParse(hfrecordID.Value, out recordID);
                currEmployee = EmployeeController.findSingleEmployee(employeeID);

                if (currEmployee != null)
                {
                    lblFname.Text = currEmployee.EMPLOYEE_FIRST_NAME;
                    lblLname.Text = currEmployee.EMPLOYEE_LAST_NAME;
                }

                AckFileXmlReader ackFile = new AckFileXmlReader();
                string errorCodeMessage = ackFile.findEmployeeErrorMessageInXML(recordID, currDistId, taxyear, etytID);

                if (errorCodeMessage != null)
                {
                    string[] messages = errorCodeMessage.Split('|');
                    lblEcode.Text = messages[0];
                    lblEmessage.Text = messages[1];
                }
            }
        }

        /// <summary>
        /// Gridview Sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvErrors_Sorting(object sender, GridViewSortEventArgs e)
        {
        }

        /// <summary>
        /// Gridview Page Indexing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvErrors_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvErrors.PageIndex = e.NewPageIndex;
            loadEmployeeSubmissionErrors();
        }
        #endregion

    }
}