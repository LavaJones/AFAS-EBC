using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

namespace Afas.AfComply.UI.admin.AdminPortal
{

    public partial class IRS_SubmissionErrors : AdminPageBase
    {

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            if (false == Feature.NewAdminPanelEnabled)
            {

                this.Log.Info("A user tried to access the IRS_SubmissionErrors page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=29", false);
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

                lblMsg.Text = "Incorrect parameters";

                return;

            }

            employer employ = employerController.getEmployer(employerId);

            cofein.Text = employ.EMPLOYER_EIN;


            loadSubmissions(employ.EMPLOYER_EIN, 2016);
        
        }

        public void loadSubmissions(string _ein, int _year)
        {
            //DdlSubmissions.DataSource = airController.manufactureIRSSubmissions(_ein, _year);
            //DdlSubmissions.DataTextField = "SUB_DATETIME";
            //DdlSubmissions.DataValueField = "SUB_UNIQUE_TRANSMISSION_ID";
            //DdlSubmissions.DataBind();

            //DdlSubmissions.Items.Add("Select");
            //DdlSubmissions.SelectedIndex = DdlSubmissions.Items.Count - 1;
        }

        protected void DdlSubmissions_SelectedIndexChanged(object sender, EventArgs e)
        {
            // int employerId = 0;

            ////check that data is correct
            //if (
            //        null == DdlEmployer.SelectedItem
            //            ||
            //        null == DdlEmployer.SelectedItem.Value
            //            ||
            //        false == int.TryParse(DdlEmployer.SelectedItem.Value, out employerId)
            //    )
            //{

            //    lblMsg.Text = "Incorrect parameters";

            //    return;

            //}

            //employer employ = employerController.getEmployer(employerId);

            //string subId = "";

            //if (DdlSubmissions.SelectedItem.Value != "Select")
            //{
            //    subId = DdlSubmissions.SelectedItem.Value;
            //}
            //else 
            //{ 
            //    lblMsg.Text = "Incorrect parameters";

            //    return;
            //}

            //List<submission> subs = airController.manufactureIRSSubmissions(employ.EMPLOYER_EIN, 2016);
            //submission sub = subs.Where(s => s.SUB_UNIQUE_TRANSMISSION_ID == subId).FirstOrDefault();

            //if (sub != null)
            //{
            //    loadSubmissionErrors(sub.SUB_RECEIPT_ID, employerId, sub.SUB_UNIQUE_TRANSMISSION_ID);
            //}
            
        }

        public void loadSubmissionErrors(string _receiptID, int _employerID, string _utID)
        {
            List<airRequestError> tempList = new List<airRequestError>();

            if (Session["errors"] == null)
            {
                //airStatusRequest asr = airController.manufactureAckFiles(_receiptID).FirstOrDefault();
                //tempList = airController.manufactureAckErrorFiles(asr, _employerID, _utID);
                Session["errors"] = tempList;
            }
            else
            {
                tempList = (List<airRequestError>)Session["errors"];
            }

            GvErrors.DataSource = tempList;
            GvErrors.DataBind();
        }

        protected void GvErrors_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortBy = e.SortExpression;
            string lastSortExpression = "";
            string lastSortDirection = "ASC";
            employer currEmployer = (employer)Session["CurrentDistrict"];
            int _employerID = currEmployer.EMPLOYER_ID;

            List<airRequestError> tempList = (List<airRequestError>)Session["errors"];

            if (Session["sortExpe"] != null)
            {
                lastSortExpression = Session["sortExpe"].ToString();
            }
            if (Session["sortDire"] != null)
            {
                lastSortDirection = Session["sortDire"].ToString();
            }

            try
            {
                if (e.SortExpression != lastSortExpression)
                {
                    lastSortExpression = e.SortExpression;
                    lastSortDirection = "ASC";
                }
                else
                {
                    if (lastSortDirection == "ASC")
                    {
                        lastSortExpression = e.SortExpression;
                        lastSortDirection = "DESC";
                    }
                    else
                    {
                        lastSortExpression = e.SortExpression;
                        lastSortDirection = "ASC";
                    }
                }

                switch (lastSortDirection)
                {
                    case "ASC":
                        switch (e.SortExpression)
                        {
                            case "RE_ID":
                                tempList = tempList.OrderBy(o => o.RE_ID).ToList();
                                break;
                            case "RE_ERROR_FIRST_NAME":
                                tempList = tempList.OrderBy(o => o.RE_ERROR_FIRST_NAME).ToList();
                                break;
                            case "RE_ERROR_LAST_NAME":
                                tempList = tempList.OrderBy(o => o.RE_ERROR_LAST_NAME).ToList();
                                break;
                            case "RE_ERROR_CODE":
                                tempList = tempList.OrderBy(o => o.RE_ERROR_CODE).ToList();
                                break;
                            case "RE_ERROR_TEXT":
                                tempList = tempList.OrderBy(o => o.RE_ERROR_TEXT).ToList();
                                break;
                            default:
                                break;
                        }
                        break;
                    case "DESC":
                        tempList.Reverse();
                        break;
                    default:
                        break;
                }

                Session["errors"] = tempList;
                Session["sortDire"] = lastSortDirection;
                Session["sortExpe"] = lastSortExpression;
                loadSubmissionErrors("", 0, ""); //Doesn't need correct parameters passed as the Session will pick up data. 
            }
            catch (Exception ex)
            { }
        }

        protected void GvErrors_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvErrors.PageIndex = e.NewPageIndex;
            loadSubmissionErrors("", 0, ""); //Doesn't need correct parameters passed as the Session will pick up data. 
        }

        protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
        {
            //try
            //{
            //    List<airRequestError> tempList = (List<airRequestError>)Session["errors"];
            //    User currUser = (User)Session["CurrentUser"];
            //    employer currEmployer = (employer)Session["CurrentDistrict"];

            //    string fileName = airController.generateIrsSubmissionErrorFile(tempList, currEmployer);
            //    string fileFullPath = Server.MapPath("..\\ftps\\export\\") + fileName;

            //    string appendText = "attachment; filename=" + fileName;
            //    Response.ContentType = "file/text";
            //    Response.AppendHeader("Content-Disposition", appendText);
            //    Response.TransmitFile(fileFullPath);
            //    Response.End();
            //}
            //catch
            //{

            //}
        }

        private ILog Log = LogManager.GetLogger(typeof(IRS_SubmissionErrors));

    }

}