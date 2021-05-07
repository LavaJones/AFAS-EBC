using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Hosting;
using System.Threading.Tasks;
using Afas.AfComply.Domain;
using log4net;

namespace Afas.AfComply.UI.securepages
{

    public partial class _1095_unapproval : Afas.AfComply.UI.securepages.SecurePageBase
    {

        protected override void PageLoadLoggedIn(User user, employer employer)
        {
            
            if (null == employer || false == employer.IrsEnabled)
            {

                Log.Info("A user [" + user.User_UserName + "] tried to access the IRS page [1095_unapproval] which is is not yet enabled for them.");

                Response.Redirect("~/default.aspx?error=60", false);

                return;
            
            }

                    LitUserName.Text = user.User_UserName;

            HfDistrictID.Value = user.User_District_ID.ToString();
            loadFilteredEmployees();

        }

        private void loadFilteredEmployees()
        {

            int _employerID = int.Parse(HfDistrictID.Value);
            int _taxyear = int.Parse(DdlTaxYear.SelectedItem.Value);
            string filterText = TxtSearch.Text.ToUpper();
            List<Employee> tempList = new List<Employee>();
            List<Employee> filteredList = new List<Employee>();
            bool searchTextEntered = true;

            searchTextEntered = errorChecking.validateTextBoxNull(TxtSearch, searchTextEntered);
            TxtSearch.BackColor = System.Drawing.Color.White;

            if (Session["Approved1095Employees"] == null)
            {
                Session["Approved1095Employees"] = EmployeeController.ManufactureEmployeeList1095Finalized(_employerID, _taxyear);
            }

            tempList = (List<Employee>)Session["Approved1095Employees"];
            if (searchTextEntered == true)
            {
                foreach (Employee emp in tempList)
                {
                    if (emp.EMPLOYEE_LAST_NAME.ToUpper().Contains(filterText))
                    {
                        filteredList.Add(emp);
                    }
                }
            }
            else
            {
                filteredList = tempList;
            }

            Session["FilteredApproved1095Employees"] = filteredList;
            GvNo1095c.DataSource = filteredList;
            GvNo1095c.DataBind();

            Session["Employees1095Filtered"] = null;            
            Session["Employees1095"] = null;                    

        }

        protected void BtnApplyFilters_Click(object sender, EventArgs e)
        {
            loadFilteredEmployees();
        }

        protected void GvNo1095c_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvNo1095c.PageIndex = e.NewPageIndex;
            loadFilteredEmployees();
        }

        protected void GvNo1095c_Sorting(object sender, GridViewSortEventArgs e)
        {

            string sortBy = e.SortExpression;
            string lastSortExpression = "";
            string lastSortDirection = "ASC";
            int _employerID = int.Parse(HfDistrictID.Value);

            List<Employee> tempList = (List<Employee>)Session["FilteredApproved1095Employees"];

            if (Session["sortExp"] != null)
            {
                lastSortExpression = Session["sortExp"].ToString();
            }
            if (Session["sortDir"] != null)
            {
                lastSortDirection = Session["sortDir"].ToString();
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
                            case "LastName":
                                tempList = tempList.OrderBy(o => o.EMPLOYEE_LAST_NAME).ToList();
                                break;
                            case "EmpID":
                                tempList = tempList.OrderBy(o => o.EMPLOYEE_EXT_ID).ToList();
                                break;
                            case "SSN":
                                tempList = tempList.OrderBy(o => o.Employee_SSN_Hidden).ToList();
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

                Session["FilteredApproved1095Employees"] = tempList;

                Session["sortDir"] = lastSortDirection;
                Session["sortExp"] = lastSortExpression;
                GvNo1095c.DataSource = tempList;
                GvNo1095c.DataBind();
            }
            catch
            { }
        }

        protected void GvNo1095c_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            employer currDist = (employer)Session["CurrentDistrict"];
            int _employerID = 0;
            int _employeeID = 0;
            int _taxYear = int.Parse(DdlTaxYear.SelectedItem.Value);
            GridViewRow row = (GridViewRow)GvNo1095c.Rows[e.RowIndex];

            HiddenField hfEmpID = (HiddenField)row.FindControl("HfEmployeeID");

            _employeeID = int.Parse(hfEmpID.Value);
            _employerID = currDist.EMPLOYER_ID;

            bool deleteEmployeeApproval = EmployeeController.deleteEmployee1095cApproval(_employeeID, _employerID, _taxYear);

            if (deleteEmployeeApproval == true)
            {

                MpeWebMessage.Show();
                LitMessage.Text = "If you wish to review this record now, click the link listed in the instructions at the top of the page.";

                Session["Employees1095Filtered"] = null;            
                Session["Employees1095"] = null;                    

                Session["FilteredApproved1095Employees"] = null;
                List<Employee> tempList = (List<Employee>)Session["Approved1095Employees"];
                foreach (Employee emp in tempList)
                {
                    if (emp.EMPLOYEE_ID == _employeeID)
                    {
                        tempList.Remove(emp);
                        break;
                    }
                }
                
                Session["Approved1095Employees"] = tempList;
                
                loadFilteredEmployees();
            
            }
            else
            {
                
                MpeWebMessage.Show();
                
                LitMessage.Text = "It does not appear that this record was approved, please review the approval process as this record may be located in the NOT getting a 1095 filter.";
            
            }
        
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }

        private ILog Log = LogManager.GetLogger(typeof(_1095_unapproval));

    }

}