using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Data;
using System.Text;
using Afas.AfComply.Domain;
using Afas.Domain;

public partial class admin_view_employers_meas : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_view_employers_meas));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;

        DdlFilterEmployers.DataSource = employerController.getAllEmployers();
        DdlFilterEmployers.DataTextField = "EMPLOYER_NAME";
        DdlFilterEmployers.DataValueField = "EMPLOYER_ID";
        DdlFilterEmployers.DataBind();

        DdlFilterEmployers.Items.Add("Select");
        DdlFilterEmployers.SelectedIndex = DdlFilterEmployers.Items.Count - 1;

        loadMeasurementPeriods();
    }

    /*********************************************************************************************
    GROUP 1: All functions that load data into dropdown lists & gridviews. ****************** 
   *********************************************************************************************/
    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

    private void loadMeasurementPeriods()
    {
        GvMeasurementPeriods.DataSource = measurementController.manufactureMeasurementList();
        GvMeasurementPeriods.DataBind();
    }


    protected void ImgBtnExportCSV_Click(object sender, EventArgs e)
    {
        measurementController.manufactureMeasurementList();

        DataTable export = new DataTable();

        export.Columns.Add("Employer Name", typeof(string));
        export.Columns.Add("Employee Type Count", typeof(string));
        export.Columns.Add("MP Start", typeof(string));
        export.Columns.Add("MP End", typeof(string));
        export.Columns.Add("Admin Start", typeof(string));
        export.Columns.Add("Admin End", typeof(string));
        export.Columns.Add("Stability Start", typeof(string));
        export.Columns.Add("Stability End", typeof(string));
        export.Columns.Add("Open Enroll Start", typeof(string));
        export.Columns.Add("Open Enroll End", typeof(string));
        export.Columns.Add("PlanYear Start", typeof(string));
        export.Columns.Add("PlanYear End", typeof(string));
        export.Columns.Add("MEASUREMENT_ID", typeof(string));
        export.Columns.Add("MEASUREMENT_EMPLOYER_ID", typeof(string));
        export.Columns.Add("MEASUREMENT_EMPLOYEE_TYPE_ID", typeof(string));
        export.Columns.Add("MEASUREMENT_TYPE_ID", typeof(string));
        export.Columns.Add("MEASUREMENT_PLAN_ID", typeof(string));

        foreach (employer employer in employerController.getAllEmployers())
        {
            try
            {
                List<EmployeeType> types = EmployeeTypeController.getEmployeeTypes(employer.EMPLOYER_ID);
                List<PlanYear> years = PlanYear_Controller.getEmployerPlanYear(employer.EMPLOYER_ID);
                foreach (Measurement meas in measurementController.manufactureMeasurementList(employer.EMPLOYER_ID))
                {
                    PlanYear year = (from PlanYear y in years where y.PLAN_YEAR_ID == meas.MEASUREMENT_PLAN_ID select y).First();
                    DataRow row = export.NewRow();
                    row["Employer Name"] = employer.EMPLOYER_NAME;
                    row["Employee Type Count"] = types.Count.ToString();
                    row["MP Start"] = meas.MEASUREMENT_START;
                    row["MP End"] = meas.MEASUREMENT_END;
                    row["Admin Start"] = meas.MEASUREMENT_ADMIN_START;
                    row["Admin End"] = meas.MEASUREMENT_ADMIN_END;
                    row["Stability Start"] = meas.MEASUREMENT_STAB_START;
                    row["Stability End"] = meas.MEASUREMENT_STAB_END;
                    row["Open Enroll Start"] = meas.MEASUREMENT_OPEN_START;
                    row["Open Enroll End"] = meas.MEASUREMENT_OPEN_END;
                    row["PlanYear Start"] = year.PLAN_YEAR_START;
                    row["PlanYear End"] = year.PLAN_YEAR_END;
                    row["MEASUREMENT_ID"] = meas.MEASUREMENT_ID;
                    row["MEASUREMENT_EMPLOYER_ID"] = meas.MEASUREMENT_EMPLOYER_ID;
                    row["MEASUREMENT_EMPLOYEE_TYPE_ID"] = meas.MEASUREMENT_EMPLOYEE_TYPE_ID;
                    row["MEASUREMENT_TYPE_ID"] = meas.MEASUREMENT_TYPE_ID;
                    row["MEASUREMENT_PLAN_ID"] = meas.MEASUREMENT_PLAN_ID;

                    export.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                Log.Warn("Exception while Checking all Measurement periods.", ex);
            }
        }

        DataTable dataTable = export;
        string filename = "MeasurementPeriods";
        string attachment = "attachment; filename= "+ filename.CleanFileName() + ".csv";
        Response.ClearContent();
        Response.BufferOutput = false;
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.ms-excel";

        StringBuilder builder = new StringBuilder();

        int columnCount = 0;
        foreach (DataColumn dataColumn in dataTable.Columns)
        {

            builder.Append(dataColumn.ColumnName.GetCsvEscaped());

            columnCount++;

            if (columnCount < dataTable.Columns.Count)
            {
                builder.Append(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
            }

        }

        builder.Append("\n");

        int iColCount = dataTable.Columns.Count;
        foreach (DataRow dataRow in dataTable.Rows)
        {

            for (int i = 0; i < iColCount; i++)
            {

                if (!Convert.IsDBNull(dataRow[i]))
                {

                    builder.Append(dataRow[i].ToString().GetCsvEscaped());

                }

                if (i < iColCount - 1)
                {
                    builder.Append(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                }

            }

            builder.Append("\n");

        }

        Response.Write(builder.ToString());

        Response.Flush();         
        Response.SuppressContent = true;                
        HttpContext.Current.ApplicationInstance.CompleteRequest();                      
        Response.End();
    }
}