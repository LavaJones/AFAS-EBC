using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Afc.Core;
using Afc.Core.Logging;
using Afc.Core.Presentation.Web;
using Afas.AfComply.Reporting.Core.Response;
using Afc.Marketing.Response;
using Afc.Marketing;
using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using AutoMapper;
using Afas.AfComply.UI.Areas.ViewModels;
using Afas.AfComply.UI.Areas.Administration.Controllers;
using Afas.AfComply.UI.Areas.ViewModels.Reporting;
using Afas.AfComply.UI.Areas.ViewModels.Enums;
using Afc.Framework.Presentation.Web;
using System.Web.UI;
using System.Diagnostics;
using System.Data;
using Afas.Domain;
using System.Text;
using Afas.AfComply.UI.App_Start;
using Afas.AfComply.Domain;
using log4net;

namespace Afas.AfComply.UI.admin.AdminPortal
{
    public partial class Finalize1094 :AdminPageBase
    {
        private ILog Log = log4net.LogManager.GetLogger(typeof(EmployerPlanYear));
        public string Finalize1094Key { get; set; }
        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            if (false == Feature.NewAdminPanelEnabled)
            {
                Log.Info("A user tried to access the Administration Employer Plan Year page which is disabled in the web config.");

                Response.Redirect("~/default.aspx?error=22", false);
            }
            else
            {
                //HfUserName.Value = user.User_UserName;
                loadEmployers();
            }
        }

        private void loadEmployers()
        {
            DdlEmployer.DataSource = employerController.getAllEmployers();
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();

            DdlEmployer.Items.Add("Select");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
        }

        private void BtnFinalize1094_Click()
        {
            // Get the employer Id and the TaxYear we are loading
          
            Employee1095summaryForTaxYearRequest request = new Employee1095summaryForTaxYearRequest();
            request.EmployerId = int.Parse(this.EncryptedParameters["EmployerId"]); 
            request.TaxYear = int.Parse(this.EncryptedParameters["TaxYear"]);
            User currUser = (User)Session["CurrentUser"];
            request.Requester = currUser.User_UserName;


            // Send the Request and wait for the response
            UIMessageResponse message = this.ApiHelper.Send<UIMessageResponse, Employee1095summaryForTaxYearRequest>(Finalize1094Key, request);


            if (message.Status != "OK")
            {
                string allErrors = "";

                foreach (string errorMessage in message.Errors)
                {
                    allErrors += errorMessage + " \n\r ";
                }
                Log.Warn(allErrors);

                throw new Exception(allErrors);
            }
        }

    }
}