using Afas.AfComply.UI.ApiControllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace Afas.AfComply.UI.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {


            context.MapRoute(
                name: "refresh-token-administration",
                url: AreaName + "/RenewToken",
                defaults: new { controller = "Authentication", action = "GetAntiForgeryToken" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "root-administration",
                url: AreaName + "/",
                defaults: new { controller = "Administration", action = "Index" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timeframe-get-all-administration",
                url: AreaName + "/TimeFrameApi/",
                defaults: new { controller = "TimeFrame", action = "GetAll" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timesframe-add-new-administration",
                url: AreaName + "/TimeFrameApi/Add/",
                defaults: new { controller = "TimeFrame", action = "Add" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("POST")
                }
            );

            context.MapRoute(
                name: "timeframe-get-many-administration",
                url: AreaName + "/TimeFrameApi/Multiple/{encryptedParameters}",
                defaults: new { controller = "TimeFrame", action = "GetMany" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timesframe-get-by-id-administration",
                url: AreaName + "/TimeFrameApi/{encryptedParameters}",
                defaults: new { controller = "TimeFrame", action = "GetSingle" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timesframe-update-administration",
                url: AreaName + "/TimeFrameApi/Update/{encryptedParameters}",
                defaults: new { controller = "TimeFrame", action = "Update" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("PUT")
                }
            );

            context.MapRoute(
                name: "timesframe-delete-administration",
                url: AreaName + "/TimeFrameApi/Delete/{encryptedParameters}",
                defaults: new { controller = "TimeFrame", action = "Delete" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("DELETE")
                }
            );


            context.MapRoute(
                name: "employer-employerList",
                url: AreaName + "/EmployerApi/EmployerList",
                defaults: new { controller = "AdminEmployer", action = "GetEmployerIdList" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
               name: "employer-GetAll1095FinalizedEmployers",
               url: AreaName + "/EmployerApi/GetAll1095FinalizedEmployers",
               defaults: new { controller = "AdminEmployer", action = "GetAll1095FinalizedEmployers" },
               constraints: new
               {
                   httpMethod = new HttpMethodConstraint("Get")
               }
           );

            context.MapRoute(
               name: "print-print1095",
               url: AreaName + "/PrintApi/Print/{encryptedParameters}",
               defaults: new { controller = "Print", action = "Printing1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
           );

            context.MapRoute(
               name: "print-admin-print1095",
               url: AreaName + "/PrintApi/AdminPrint/{encryptedParameters}",
               defaults: new { controller = "Print", action = "AdminPrinting1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
           );

            context.MapRoute(
               name: "print-all-reprint1095",
               url: AreaName + "/PrintApi/AllReprint/{encryptedParameters}",
               defaults: new { controller = "Print", action = "AllReprinting1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
           );

            context.MapRoute(
               name: "print-admin-all-print1095",
               url: AreaName + "/PrintApi/AdminAllReprint/{encryptedParameters}",
               defaults: new { controller = "Print", action = "AdminAllReprinting1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
           );

            context.MapRoute(
               name: "print-pdf-print1095",
               url: AreaName + "/PrintApi/PdfPrint/{encryptedParameters}",
               defaults: new { controller = "Print", action = "PdfPrinting1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
            );

            context.MapRoute(
               name: "print-pdf-Print1094",
               url: AreaName + "/PrintApi/Print1094/{encryptedParameters}",
               defaults: new { controller = "Print", action = "Print1094" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
            );
               
          context.MapRoute(
            name: "employer1094summary-GetAllEmployers",
            url: AreaName + "/Employer1094SummaryApi/GetAllEmployers",
            defaults: new { controller = "Employer1094Summary", action = "GetAllEmployers" },
            constraints: new
            {
                httpMethod = new HttpMethodConstraint("Post")
            }
          );

          context.MapRoute(
                name: "administration-qlik",
                url: AreaName + "/Qlik",
                defaults: new { controller = "Qlik", action = "Index" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "catch-all-administration",
                url: AreaName + "/{*url}",
                defaults: new { controller = "Administration", action = "Index" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

        }

    }

}
