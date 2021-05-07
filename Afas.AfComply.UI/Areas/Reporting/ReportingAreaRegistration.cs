using Afas.AfComply.UI.ApiControllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace Afas.AfComply.UI.Areas.Reporting
{
    public class ReportingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                name: "refresh-token-reporting",
                url: AreaName + "/RenewToken",
                defaults: new { controller = "Authentication", action = "GetAntiForgeryToken" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "root-reporting",
                url: AreaName + "/",
                defaults: new { controller = "Reporting", action = "Index" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timeframe-get-all-reporting",
                url: AreaName + "/TimeFrameApi/",
                defaults: new { controller = "TimeFrame", action = "GetAll" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timeframe-get-many-reporting",
                url: AreaName + "/TimeFrameApi/Multiple/{encryptedParameters}",
                defaults: new { controller = "TimeFrame", action = "GetMany" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timesframe-get-by-id-reporting",
                url: AreaName + "/TimeFrameApi/{encryptedParameters}",
                defaults: new { controller = "TimeFrame", action = "GetSingle" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );
            
            context.MapRoute(
                name: "qlik-get-all-reporting",
                url: AreaName + "/QlikApi/",
                defaults: new { controller = "Qlik", action = "GetAll" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "approved1095final-get-many-reporting",
                url: AreaName + "/Approved1095FinalApi/Multiple/{encryptedParameters}",
                defaults: new { controller = "Finalize1095", action = "GetMany" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "approved1095final-get-taxyears",
                url: AreaName + "/Approved1095FinalApi/GetTaxYears/",
                defaults: new { controller = "Finalize1095", action = "GetTaxYears" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
               name: "approved1095final-unfinalizeAll1095",
               url: AreaName + "/Approved1095FinalApi/UnFinalizeAll1095/{encryptedParameters}",
               defaults: new { controller = "Finalize1095", action = "UnFinalizeAll1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
           );

            context.MapRoute(
               name: "approved1095final-unfinalize1095",
               url: AreaName + "/Approved1095FinalApi/UnFinalize1095/{encryptedParameters}",
               defaults: new { controller = "Finalize1095", action = "UnFinalize1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
            );

            context.MapRoute(
               name: "approved1095final-export-part2",
               url: AreaName + "/Approved1095FinalApi/GetPart2CsvReport/{encryptedParameters}",
               defaults: new { controller = "Finalize1095", action = "GetPart2FileExport" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Get")
               }
            );

            context.MapRoute(
                name: "employee1095summary-get-all-reporting",
                url: AreaName + "/Employee1095summaryApi/",
                defaults: new { controller = "Employee1095summary", action = "GetAll" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "employee1095summary-get-many-reporting",
                url: AreaName + "/Employee1095summaryApi/Multiple/{encryptedParameters}",
                defaults: new { controller = "Employee1095summary", action = "GetMany" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "employee1095summary-get-file-reporting",
                url: AreaName + "/Employee1095summaryApi/GetFileExport/{encryptedParameters}",
                defaults: new { controller = "Employee1095summary", action = "GetFileExport" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );            

            context.MapRoute(
                name: "employee1095summary-get-by-id-reporting",
                url: AreaName + "/Employee1095summaryApi/{encryptedParameters}",
                defaults: new { controller = "Employee1095summary", action = "GetSingle" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "employee1095summary-get-single-by-id-reporting",
                url: AreaName + "/Employee1095summaryApi/GetSingle/{encryptedParameters}",
                defaults: new { controller = "Employee1095summary", action = "GetSingle" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
              name: "employee1095summary-get-part2-by-employee",
              url: AreaName + "/Employee1095summaryApi/LoadPart2/{encryptedParameters}",
              defaults: new { controller = "Employee1095summary", action = "GetPart2ForEmployee" },
              constraints: new
              {
                  encryptedParameters = new EncryptedParametersRouteConstraint(),
                  httpMethod = new HttpMethodConstraint("Get")
              }
            );
            
            context.MapRoute(
              name: "employee1095summary-get-part3-by-employee",
              url: AreaName + "/Employee1095summaryApi/LoadPart3/{encryptedParameters}",
              defaults: new { controller = "Employee1095summary", action = "GetPart3ForEmployee" },
              constraints: new
              {
                  encryptedParameters = new EncryptedParametersRouteConstraint(),
                  httpMethod = new HttpMethodConstraint("Get")
              }
            );

            context.MapRoute(
                 name: "employee1095summary-UpdatePart3InsuranceCoverage",
                 url: AreaName + "/Employee1095detailsPart3Api/Update/{encryptedParameters}",
                 defaults: new { controller = "Employee1095summary", action = "UpdatePart3InsuranceCoverage" },
                 constraints: new
                 {
                     encryptedParameters = new EncryptedParametersRouteConstraint(),
                     httpMethod = new HttpMethodConstraint("put")
                 }
            );

            context.MapRoute(
                name: "employee1095summary-DeletePart3InsuranceCoverageDependant",

                url: AreaName + "/Employee1095detailsPart3Api/Delete/{encryptedParameters}",
                defaults: new { controller = "Employee1095summary", action = "DeletePart3InsuranceCoverage" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("delete")
                }
            );

            context.MapRoute(
                name: "employee1095summary-AddPart3InsuranceCoverage",
                url: AreaName + "/Employee1095summaryApi/AddPart3IC/{encryptedParameters}",
                defaults: new { controller = "Employee1095summary", action = "AddPart3InsuranceCoverage" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Post")
                }
            );

            context.MapRoute(
                name: "employee1095summary-get-taxyears",
                url: AreaName + "/Employee1095summaryApi/GetTaxYears/",
                defaults: new { controller = "Employee1095summary", action = "GetTaxYears" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
               name: "employee1095summary-Finalize1095",
               url: AreaName + "/Employee1095summaryApi/Finalize1095/{encryptedParameters}",
               defaults: new { controller = "Employee1095summary", action = "Finalize1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
           );

            context.MapRoute(
               name: "employee1095summary-review1095",
               url: AreaName + "/Employee1095summaryApi/Review1095/{encryptedParameters}",
               defaults: new { controller = "Employee1095summary", action = "Review1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
            );


            context.MapRoute(
                name: "employee1095summary-part1-edits",
                url: AreaName + "/Employee1095summaryApi/Update/{encryptedParameters}",
                defaults: new { controller = "Employee1095summary", action = "SavePart1UserEdits" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Put")
                }
            );
            
            context.MapRoute(
                name: "Employee1095detailsPart2-put-edits",
                url: AreaName + "/Employee1095summaryApi/UpadatePart2s/{encryptedParameters}",
                defaults: new { controller = "Employee1095summary", action = "SavePart2UserEdits" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Put")
                }
            );
            
            context.MapRoute(
               name: "employee1095summary-upload-edits",
               url: AreaName + "/Employee1095summaryApi/UploadFileEdits/{encryptedParameters}",
               defaults: new { controller = "Employee1095summary", action = "UploadFileEdits" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
            );

            context.MapRoute(
                name: "Verification-get-all-reporting",
                url: AreaName + "/VerificationApi/",
                defaults: new { controller = "Verification", action = "GetAll" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "Verification-get-many-reporting",
                url: AreaName + "/VerificationApi/Multiple/{encryptedParameters}",
                defaults: new { controller = "Verification", action = "GetMany" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "Verification-get-by-id-reporting",
                url: AreaName + "/VerificationApi/{encryptedParameters}",
                defaults: new { controller = "Verification", action = "GetSingle" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );



            context.MapRoute(
                name: "ConfirmationPage-confirm",
                url: AreaName + "/ConfirmationPage/Confirm",
                defaults: new { controller = "ConfirmationPage", action = "Confirm" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Post")
                }
            );

            context.MapRoute(
               name: "ConfirmationPage-IRSContactUser",
               url: AreaName + "/ConfirmationPage/GetAllIRSContactUser",
               defaults: new { controller = "ConfirmationPage", action = "GetAllIRSContactUser" },
               constraints: new
               {
                   httpMethod = new HttpMethodConstraint("Get")
               }
           );
            context.MapRoute(
               name: "ConfirmationPage-SafeHarborCodes",
               url: AreaName + "/ConfirmationPage/GetAllIRSSafeHarborCodes",
               defaults: new { controller = "ConfirmationPage", action = "GetAllIRSSafeHarborCodes" },
               constraints: new
               {
                   httpMethod = new HttpMethodConstraint("Get")
               }
           );
            context.MapRoute(
               name: "ConfirmationPage-AllEmployees",
               url: AreaName + "/ConfirmationPage/GetAllEmployees",
               defaults: new { controller = "ConfirmationPage", action = "GetAllEmployees" },
               constraints: new
               {
                   httpMethod = new HttpMethodConstraint("Get")
               }
           );

            context.MapRoute(
              name: "ConfirmationPage-SaveEmployees",
              url: AreaName + "/ConfirmationPage/SaveEmployees",
              defaults: new { controller = "ConfirmationPage", action = "SaveEmployees" },
              constraints: new
              {
                  httpMethod = new HttpMethodConstraint("Post")
              }
          );


            context.MapRoute(
              name: "ConfirmationPage-DoAlertExists",
              url: AreaName + "/ConfirmationPage/DoAlertExists",
              defaults: new { controller = "ConfirmationPage", action = "DoAlertExists" },
              constraints: new
              {
                  httpMethod = new HttpMethodConstraint("Get")
              }
          );


            context.MapRoute(
              name: "ConfirmationPage-GetEmployerName",
              url: AreaName + "/ConfirmationPage/GetEmployerName",
              defaults: new { controller = "ConfirmationPage", action = "GetEmployerName" },
              constraints: new
              {
                  httpMethod = new HttpMethodConstraint("Get")
              }
          );

            context.MapRoute(
               name: "client-print-print1095",
               url: AreaName + "/PrintApi/Print/{encryptedParameters}",
               defaults: new { controller = "Print", action = "Printing1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
           );

            context.MapRoute(
               name: "client-print-all-reprint1095",
               url: AreaName + "/PrintApi/AllReprint/{encryptedParameters}",
               defaults: new { controller = "Print", action = "AllReprinting1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
           );

            context.MapRoute(
               name: "client-print-pdf-print1095",
               url: AreaName + "/PrintApi/PdfPrint/{encryptedParameters}",
               defaults: new { controller = "Print", action = "PdfPrinting1095" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
            );

            context.MapRoute(
               name: "client-print-has-printed",
               url: AreaName + "/PrintApi/HasPrinted/{encryptedParameters}",
               defaults: new { controller = "Print", action = "HasPrinted" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
            );



            context.MapRoute(
                name: "employer1094summary-get-many-reporting",
                url: AreaName + "/Employer1094SummaryApi/Multiple/{encryptedParameters}",
                defaults: new { controller = "Employer1094Summary", action = "GetMany" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
              name: "employer1094summary-Finalize1094",
              url: AreaName + "/Employer1094SummaryApi/Finalize1094/{encryptedParameters}",
              defaults: new { controller = "Employer1094Summary", action = "Finalize1094" },
              constraints: new
              {
                  encryptedParameters = new EncryptedParametersRouteConstraint(),
                  httpMethod = new HttpMethodConstraint("Post")
              }
          );


            context.MapRoute(
             name: "employer1094summary-AdminFinalize1094",
             url: "Administration/Employer1094SummaryApi/AdminFinalize1094/{encryptedParameters}",
             defaults: new { controller = "Employer1094Summary", action = "AdminFinalize1094" },
             constraints: new
             {
                 encryptedParameters = new EncryptedParametersRouteConstraint(),
                 httpMethod = new HttpMethodConstraint("Post")
             }
         );


            context.MapRoute(
             name: "employer1094summary-Confirm",
             url: "Administration/Employer1094SummaryApi/Confirm/{encryptedParameters}",
             defaults: new { controller = "Employer1094Summary", action = "Confirm" },
             constraints: new
             {
                 encryptedParameters = new EncryptedParametersRouteConstraint(),
                 httpMethod = new HttpMethodConstraint("Post")
             }
         );




            context.MapRoute(
                name: "catch-all-reporting",
                url: AreaName + "/{*url}",
                defaults: new { controller = "Reporting", action = "Index" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

        }
    }
}