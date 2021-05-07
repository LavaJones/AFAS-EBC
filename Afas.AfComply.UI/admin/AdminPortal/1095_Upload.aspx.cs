using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class _1095_Upload : AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(_1095_Upload));
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the 1095 Upload  page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=14", false);
            }

        }
        protected void BtnUploadFile_Click(object sender, EventArgs e)
        {
            if (Pdf.HasFile)
            {
                String path = ("~\\ftps\\Printed\\");

                String _FilePath = HttpContext.Current.Server.MapPath(path);
                FileProcessing.SaveFile(Pdf, _FilePath, LblFileUploadMessage);
                LblFileUploadMessage.Text = "File Uploaded Sucessfully";
            }
            else
            {
                Pdf.BackColor = System.Drawing.Color.Red;
                LblFileUploadErrorMessage.Text = "Please select a file";
            }

        }
    }
}