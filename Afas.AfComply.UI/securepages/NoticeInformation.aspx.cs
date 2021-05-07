using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using log4net;
using System.Net;
using System.Web.Configuration;
using System.Security.Cryptography.X509Certificates;
using Afc.Web.Reporting.Application;

public partial class NoticeInformation : Afas.AfComply.UI.securepages.SecurePageBase
{

    private ILog Log = LogManager.GetLogger(typeof(NoticeInformation));

    protected override  void PageLoadLoggedIn(User user, employer currDist)
    {

        this.HfDistrictID.Value = user.User_District_ID.ToString();

        this.NoticeText.Text = Feature.HomePageMessageLong;

    }
}
