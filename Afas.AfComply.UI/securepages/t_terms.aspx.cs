using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;

public partial class securepages_t_terms : Afas.AfComply.UI.securepages.SecurePageBase
{

    private ILog Log = LogManager.GetLogger(typeof(securepages_t_terms));

    protected override void PageLoadLoggedIn(User user, employer employer)
    {
        LitUserName.Text = user.User_UserName;

        HfDistrictID.Value = user.User_District_ID.ToString();

        GvReports.DataSource = termController.getTerms();
        GvReports.DataBind();
        GvReports.SelectedIndex = 0;
        loadData();
    }

    protected void GvReports_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadData();
    }

    private void loadData()
    {
        HiddenField hfID = null;
        LinkButton lbTerm = null;

        if (GvReports.Rows.Count > 0)
        {
            GridViewRow row = GvReports.SelectedRow;

            lbTerm = (LinkButton)row.FindControl("LbReportName");
            hfID = (HiddenField)row.FindControl("HfReportID");

            LblTerm.Text = lbTerm.Text;
            LitDefinition.Text = hfID.Value.ToString();
        }
        else
        {
            LitDefinition.Text = "The Terms are currently not available.";
        }
    }











    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }
}