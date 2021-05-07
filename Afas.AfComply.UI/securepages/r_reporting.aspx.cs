using Afc.Web.Reporting.Application;
using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Configuration;

public partial class securepages_r_reporting : Afas.AfComply.UI.securepages.SecurePageBase
{
    public string MyValue { get; set; }
    private ILog Log = LogManager.GetLogger(typeof(EmployeeType));
    protected override void PageLoadLoggedIn(User user, employer currDist)
    {
        try
        {

            if (false == Feature.QlikEnabled)
            {
                this.Response.Redirect("~/securepages/reports.aspx", false);

                return;
            }

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            StoreLocation certLoc = WebConfigurationManager.AppSettings["Feature.StoreLocation"].Equals("LocalMachine", StringComparison.CurrentCultureIgnoreCase) ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;
            LocalUserCertificateManager certificateManager = new LocalUserCertificateManager(
             WebConfigurationManager.AppSettings["Feature.ThumbPrint"], certLoc);

            QlikIframeHelper qlikHelper = new QlikIframeHelper(
                     WebConfigurationManager.AppSettings["Feature.QlikUrl"],
                     WebConfigurationManager.AppSettings["Feature.QlikLocation"],
                     certificateManager,
                     (sender, certificate, chain, error) => { return true; });

            if (user.User_Floater != true)
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>()
                {
                    { "USERID", user.User_UserName },
                    { "EMPLOYERID", currDist.EMPLOYER_ID.ToString() },
                };
                this.MyValue = qlikHelper.GetIframeUrl(user.User_UserName, WebConfigurationManager.AppSettings["Feature.QlikUserDirectory"], WebConfigurationManager.AppSettings["Feature.QlikSenseApplication"],
                new List<Dictionary<string, string>>() { attributes }, WebConfigurationManager.AppSettings["Feature.QlikReportMashup"]);
            }
            else
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>()
                {
                    { "USERID", user.User_UserName },
                    { "EMPLOYERID", "*" },
                };
                this.MyValue = qlikHelper.GetIframeUrl(user.User_UserName, WebConfigurationManager.AppSettings["Feature.QlikUserDirectory"], WebConfigurationManager.AppSettings["Feature.QlikSenseApplication"],
                new List<Dictionary<string, string>>() { attributes }, WebConfigurationManager.AppSettings["Feature.QlikReportMashup"]);
            }

        }
        catch (Exception exception)
        {

            this.Log.Warn("Warning Qlik Access is having the following issues.", exception);

        }
    }
}