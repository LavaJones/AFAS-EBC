
using System;
using log4net;
using Afas.AfComply.Reporting.Domain;
using Afas.AfComply.Reporting.Domain.LegacyData;
using System.Linq;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class _1095CInformation : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(BulkEmployerMeasurementPeriod));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the 1095-C Information page which is disabled in the web config.");
                Response.Redirect("~/default.aspx?error=4", false);
            }
            else
            {
                getDate();
            }
        }

        private void getDate()
        {
            //using (var ctx = new AcaEntities())
            //{
            //    User curr = (User)Session["CurrentUser"];
            //    var Employees = ctx.View_air_replacement_employee_monthly_detail.Where(a => a.employer_id == curr.User_District_ID).ToList();
            //    lblTotalEmployees.Text = Employees.GroupBy(c => c.employee_id).Count().ToString();
            //    var EmployeesReceiving1095C = (from View_air_replacement_employee_monthly_detail employee
            //                                   in Employees
            //                                   where employee.Rececing1095C
            //                                   select employee);
            //    lblEmployeesReceiving.Text = EmployeesReceiving1095C.GroupBy(c => c.employee_id).Count().ToString();
            //}
        }
    }
}