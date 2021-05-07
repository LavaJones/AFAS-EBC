using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using log4net;
using System.Data;
using System.Configuration;
using Afas.AfComply.Domain;
using Afas.Domain;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Security.Cryptography.X509Certificates;
using Afas.AfComply.Reporting.Application.Services;
using System.Web.Configuration;
using Afc.Web.Reporting.Application;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class QlikAccess : AdminPageBase
    {
        public string MyValue { get; set; }
        private ILog Log = LogManager.GetLogger(typeof(EmployeeCount));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {

            try
            {
                //new UserCertificateService("f031930c8f44b36fda331fd01234b178cb322072");
                //new LocalUserCertificateManager("f031930c8f44b36fda331fd01234b178cb322072")
                
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12; 
 
                var certLoc = WebConfigurationManager.AppSettings["Feature.StoreLocation"].Equals("LocalMachine", StringComparison.CurrentCultureIgnoreCase) ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;
                var certificateManager = new LocalUserCertificateManager(
                 WebConfigurationManager.AppSettings["Feature.ThumbPrint"], certLoc);

                var qlikHelper = new QlikIframeHelper(
                         WebConfigurationManager.AppSettings["Feature.QlikUrl"],
                         WebConfigurationManager.AppSettings["Feature.QlikLocation"],
                         certificateManager,
                         (sender, certificate, chain, error) => { return true; });

                this.MyValue = qlikHelper.GetIframeUrl(WebConfigurationManager.AppSettings["Feature.QlikUser"], WebConfigurationManager.AppSettings["Feature.QlikUserDirectory"]);

            }
            catch (Exception exception)
            {

                Log.Warn("Warning Qlik Access is having the following issues.", exception);

            }
        }
    }
}