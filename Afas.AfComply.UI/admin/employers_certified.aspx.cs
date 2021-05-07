using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afas.AfComply.Domain;

namespace Afas.AfComply.UI.admin
{
    public partial class employers_certified : AdminPageBase
    {

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the Employers Certified page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=46", false);
            }

        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
           if(IsPostBack == false)
                GetEmployersCertified();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            User user = ((User)Session["CurrentUser"]);
            foreach (GridViewRow row in gvEmployersCertified.Rows)
            {
                CheckBox ChkBoxRow = (CheckBox)row.FindControl("chkEmp");
                if (ChkBoxRow.Checked)
                {
                    int EmployerTaxYearTransmissionId = Convert.ToInt32(gvEmployersCertified.DataKeys[row.RowIndex].Values["EmployerTaxYearTransmissionId"]);
                    
                    employerController.insertUpdateEmployerTaxYearTransmissionStatus(
                          new EmployerTaxYearTransmissionStatus(
                           EmployerTaxYearTransmissionId,
                           TransmissionStatusEnum.CompanyApproved,
                           user.User_UserName,
                           DateTime.Now
                       ));
                }
            }

            GetEmployersCertified();

        }

        protected void GetEmployersCertified()
        {

            DataTable dtEmployersCertified = new DataTable();

            using (var connString = new SqlConnection(ConfigurationManager.ConnectionStrings["ACA_Conn"].ConnectionString))
            {
                using (var cmd = new SqlCommand("SELECT_employers_in_transmission_status_for_tax_year", connString))
                {
                    int taxYearID = DateTime.Now.AddYears(-1).Year;
                    cmd.Parameters.AddWithValue("@taxYearId", SqlDbType.Int).Value = taxYearID;

                    cmd.Parameters.AddWithValue("@transmissionStatusId", SqlDbType.Int).Value = TransmissionStatusEnum.Certified;

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(dtEmployersCertified);
                    }
                }
            }

            gvEmployersCertified.DataSource = dtEmployersCertified;
            gvEmployersCertified.DataBind();

            if (dtEmployersCertified.Rows.Count == 0)
            {
                lblMsg.Text = CASSPrintFileGenerator.NoRecordsFoundErrorMessage;
                btnApprove.Visible = false;
            }
            else
            {
                btnApprove.Visible = true;
            }

        }


        private ILog Log = LogManager.GetLogger(typeof(employers_certified));

    }

}