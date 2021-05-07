using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using log4net;

using Afas.AfComply.Domain;

using Afas.AfComply.UI.ApiControllers;
using System.Web.Mvc;

namespace Afas.AfComply
{

    public class WebApplication : System.Web.HttpApplication
    {

        protected void Application_Start(Object sender, EventArgs eventArgs)
        {
            
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();

            RouteTable.Routes.MapHttpRoute(
                    name: "automation-calculation-payroll",
                    routeTemplate: "api/queue/payroll/{authorizationKey}/process/",
                    defaults: new
                    {
                        controller = "Calculations",
                        action = "ProcessQueuedPayrollCalculations"
                    },
                    constraints: new
                    {
                        httpMethod = new HttpMethodConstraint("POST"),
                        authorizationKey = new GuidRouteConstraint(),
                    }
                );











       /*     RouteTable.Routes.MapHttpRoute(
                  name: "automation-transmission-cass-confirm",
                    routeTemplate: "api/cass/{authorizationKey}/confirm/",
                    defaults: new
                   {
                        controller = "Calculations",
                        action = "ProcessQueuedPayrollCalculations"
                   },
                    constraints: new
                    {
                       httpMethod = new HttpMethodConstraint("POST"),
                       authorizationKey = new GuidRouteConstraint(),
                   }
               );

            RouteTable.Routes.MapHttpRoute(
                    name: "automation-transmission-print-confirm",
                   routeTemplate: "api/print/{authorizationKey}/confirm/",
                   defaults: new
                    {
                       controller = "Calculations",
                       action = "ProcessQueuedPayrollCalculations"
                   },
                   constraints: new
                   {
                        httpMethod = new HttpMethodConstraint("POST"),
                        authorizationKey = new GuidRouteConstraint(),
                   }
                );*/














            RouteTable.Routes.MapHttpRoute(
                    name: "status-check",
                    routeTemplate: "api/status-check/{authorizationToken}/",
                    defaults: new
                    {
                        controller = "StatusCheck",
                        action = "Available"
                    },
                    constraints: new
                    {
                        httpMethod = new HttpMethodConstraint("GET"),
                        authorizationToken = new GuidRouteConstraint(),
                    }
                );
        

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.AdministrationEnabled", Feature.AdministrationEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.AdministrationEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.AutoLogoutTime", Feature.AutoLogoutTime));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.AutoLogoutTime"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.BulkConverterEnabled", Feature.BulkConverterEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.BulkConverterEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.BulkImportEnabled", Feature.BulkImportEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.BulkImportEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.CheckDateDefaultValueEnabled", Feature.CheckDateDefaultValueEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.CheckDateDefaultValueEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.DangerousErrorsEnabled", Feature.DangerousErrorsEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.DangerousErrorsEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.DOBDefaultValueEnabled", Feature.DOBDefaultValueEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.DOBDefaultValueEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.EmployeeDemographicEmployeeNumberRequired", Feature.EmployeeDemographicEmployeeNumberRequired));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.EmployeeDemographicEmployeeNumberRequired"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.EmployeeDemographicGenerateEmployeeNumberEnabled", Feature.EmployeeDemographicGenerateEmployeeNumberEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.EmployeeDemographicGenerateEmployeeNumberEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.EmployeeExportCarrierFileEnabled", Feature.EmployeeExportCarrierFileEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.EmployeeExportCarrierFileEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.EmployeeExportOfferFileEnabled", Feature.EmployeeExportOfferFileEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.EmployeeExportOfferFileEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.FullDataExportEnabled", Feature.FullDataExportEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.FullDataExportEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.HomePageMessageEnabled", Feature.HomePageMessageEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.HomePageMessageEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.InvoicingEnabled", Feature.InvoicingEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.InvoicingEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.IsRealEmailsEnabled", Feature.IsRealEmailsEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.IsRealEmailsEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.CalculationBatchLimit", Feature.CalculationBatchLimit));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.CalculationBatchLimit"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.NewAdminPanelEnabled", Feature.NewAdminPanelEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.NewAdminPanelEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.SelfMeasurementPeriodsEnabled", Feature.SelfMeasurementPeriodsEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.SelfMeasurementPeriodsEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.SelfRegistrationEnabled", Feature.SelfRegistrationEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.SelfRegistrationEnabled"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.ShowDOB", Feature.ShowDOB));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.ShowDOB"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Feature.UserManagementEnabled", Feature.UserManagementEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for FeatureToggle:{0}", "Feature.UserManagementEnabled"), exception);
            }

            // branding parts.
            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Branding.IrsDeadlineCertify", Branding.IrsDeadlineCertify));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for BrandingToggle:{0}", "Branding.IrsDeadlineCertify"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Branding.IrsDeadlineSetup", Branding.IrsDeadlineSetup));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for BrandingToggle:{0}", "Branding.IrsDeadlineSetup"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Branding.IrsDeadlineTransmit", Branding.IrsDeadlineTransmit));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for BrandingToggle:{0}", "Branding.IrsDeadlineTransmit"), exception);
            }

            try
            {
                this.Log.Debug(String.Format("{0} is {1}.", "Branding.IrsReprintFeeEnabled", Branding.IrsReprintFeeEnabled));
            }
            catch (Exception exception)
            {
                this.Log.Warn(String.Format("Suppressing error for BrandingToggle:{0}", "Branding.IrsReprintFeeEnabled"), exception);
            }


        }

        void Application_Error(Object sender, EventArgs eventArgs)
        {

            Exception exception = Server.GetLastError();

            // If it is a user who got logged out we follow a different path
            if (exception is System.Web.HttpException && exception.Message.Contains("Validation of viewstate MAC failed."))
            {
                this.Log.Warn("User was timed out but tried interacting.", exception);

                Response.Redirect("~/Logout.aspx", false);

                return;
            }
            
            this.Log.Error("Last chance handler caught the exception:", exception);

            if (IsStatusCheckControllerException(exception))
            {

                // fall through intentional.

            }
            else if (IsSafeExceptionsEnabled())
            {
                Response.Redirect("/error.aspx?error-code=A4B8E0DB-486D-4EE6-87C2-977369BEBE6D", false);
            }

            // Yellow page of death is coming.

        }

        /// <summary>
        /// When deployed to servers, this method will always return true based on the web.config transforms.
        /// Ensures we always use our site error page instead of displaying sensitive information.
        /// This is a configuration item to ensure we can see errors locally when developing.
        /// </summary>
        /// <returns></returns>
        private Boolean IsSafeExceptionsEnabled()
        {
            return Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["Feature.DangerousErrorsEnabled"]) == false;
        }

        /// <summary>
        /// The Status Check process is special since it is used by the load balancer and monitoring solutions to determine a node's availability.
        /// The status check process will not display any sensitive information as it logs and suppresses the original exception and thows a new
        /// Exception without the existing stack trace.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private Boolean IsStatusCheckControllerException(Exception exception)
        {
            return exception.InnerException.TargetSite.DeclaringType.FullName.Equals("status_check_Default", StringComparison.CurrentCultureIgnoreCase);
        }

        private ILog Log = LogManager.GetLogger(typeof(WebApplication));

    }

}