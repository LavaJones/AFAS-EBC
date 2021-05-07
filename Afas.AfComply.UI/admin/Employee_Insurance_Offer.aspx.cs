using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using log4net;
using Afas.AfComply.UI.Code.POCOs;


    public partial class admin_Employee_Insurance_Offer : Afas.AfComply.UI.admin.AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(admin_Employee_Insurance_Offer));

        private IList<employer> Employers { get; set; }
        private IList<Employee> Employees { get; set; }
        int _employerID;
        int _employeeID;
        List<offered_Insurance> employeeInsuranceOffers;
        
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            LitUserName.Text = user.User_UserName;
            LoadEmployers();

        }

        private void LoadEmployers()
        {
            Employers = employerController.getAllEmployers();
            DdlFilterEmployers.DataSource = Employers;
            DdlFilterEmployers.DataTextField = "EMPLOYER_NAME";
            DdlFilterEmployers.DataValueField = "EMPLOYER_ID";
            DdlFilterEmployers.DataBind();
            DdlFilterEmployers.Items.Add("Select");
            DdlFilterEmployers.SelectedIndex = DdlFilterEmployers.Items.Count - 1;

        }

        protected void DdlFilterEmployers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _employerID = int.Parse(DdlFilterEmployers.SelectedItem.Value);
            Employees = EmployeeController.manufactureEmployeeList(_employerID);
            DdlFilterEmployees.DataSource = Employees;
            DdlFilterEmployees.DataTextField = "EMPLOYEE_FULL_NAME";
            DdlFilterEmployees.DataValueField = "EMPLOYEE_ID";
            DdlFilterEmployees.DataBind();
            DdlFilterEmployees.Items.Add("Select");
            DdlFilterEmployees.SelectedIndex = DdlFilterEmployees.Items.Count - 1;
        }

        protected void DdlFilterEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DdlFilterEmployees.SelectedItem.Value != "Select")
            {
                employeeInsuranceOffers = GetInsuranceOffers();
                gvEmployeeInsuranceOffer.DataSource = employeeInsuranceOffers;
                gvEmployeeInsuranceOffer.DataBind();
            }
        }

        protected void Gridview1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmployeeInsuranceOffer.DataSource = GetInsuranceOffers();
            gvEmployeeInsuranceOffer.PageIndex = e.NewPageIndex;            
            gvEmployeeInsuranceOffer.Page.DataBind();
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }


        private List<offered_Insurance> GetInsuranceOffers()
        {
            _employeeID = int.Parse(DdlFilterEmployees.SelectedItem.Value);
            return insuranceController.findAllInsurancesofferedToEmployee(_employeeID);
        }
    }
