using Afas.AfComply.Reporting.Api.Controllers;
using Afc.Marketing.Framework.WebApi.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Afas.AfComply.Reporting.Api
{

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new Plumbing.WindsorResolver(
                    App_Start.ContainerActivator._container.Resolve<Castle.Windsor.IWindsorContainer>()
                );

            config.Filters.Add(new ActionTimingAttribute());
            config.Filters.Add(new ExceptionRoutingAttribute());

            config.Routes.MapHttpRoute(
                      name: "timeframe-getById",
                    routeTemplate: "TimeFrame/GetById/{ResourceId}",
                    defaults: new
                    {
                        controller = "WApiTimeFrame",
                        action = "GetById"
                    },
                    constraints: new
                    {
                        resourceId = new GuidRouteConstraint(),
                        httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                    }
            );

            config.Routes.MapHttpRoute(
                 name: "timeframe-getAll",
                 routeTemplate: "TimeFrame/GetAll/",
                 defaults: new
                 {
                     controller = "WApiTimeFrame",
                     action = "GetAll"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                 }
            );

            config.Routes.MapHttpRoute(
                 name: "timeframe-getForYear",
                 routeTemplate: "TimeFrame/GetForYear/",
                 defaults: new
                 {
                     controller = "WApiTimeFrame",
                     action = "GetForYear"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                 }
            );

            config.Routes.MapHttpRoute(
                name: "timeframe-addNew",
                routeTemplate: "TimeFrame/New/",
                defaults: new
                {
                    controller = "WApiTimeFrame",
                    action = "AddNew"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
            );

            config.Routes.MapHttpRoute(
                name: "timeframe-update",
                routeTemplate: "TimeFrame/Update/",
                defaults: new
                {
                    controller = "WApiTimeFrame",
                    action = "Update"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
            );

            config.Routes.MapHttpRoute(
                name: "timeframe-delete",
                routeTemplate: "TimeFrame/Delete/",
                defaults: new
                {
                    controller = "WApiTimeFrame",
                    action = "Delete"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
            );

            config.Routes.MapHttpRoute(
                 name: "qlik-getAll",
                 routeTemplate: "Qlik/GetAll/",
                 defaults: new
                 {
                     controller = "WApiQlik",
                     action = "GetKlikUrl"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Get)
                 }
            );

            config.Routes.MapHttpRoute(
                 name: "approved1095final-get-many",
                 routeTemplate: "Approved1095Final/GetForTaxYear/",
                 defaults: new
                 {
                     controller = "WApiApproved1095Final",
                     action = "GetForTaxYear"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
                 name: "approved1095final-unfinalize-all-1095",
                 routeTemplate: "Approved1095Final/UnFinalizeAll1095/",
                 defaults: new
                 {
                     controller = "WApiApproved1095Final",
                     action = "UnFinalizeAll1095"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
                 name: "approved1095final-unfinalize-1095",
                 routeTemplate: "Approved1095Final/UnFinaliz1095/",
                 defaults: new
                 {
                     controller = "WApiApproved1095Final",
                     action = "UnFinalize1095"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
                 name: "employee1095summary-get-many",
                 routeTemplate: "Employee1095summary/GetForTaxYear/",
                 defaults: new
                 {
                     controller = "WApiEmployee1095summary",
                     action = "GetForTaxYear"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );


            config.Routes.MapHttpRoute(
                 name: "employee1095summary-Finalize1095",
                 routeTemplate: "Employee1095summary/Finalize1095/",
                 defaults: new
                 {
                     controller = "WApiEmployee1095summary",
                     action = "Finalize1095"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
                 name: "employee1095summary-un-reviewed1095",
                 routeTemplate: "Employee1095summary/UnReviewed1095/",
                 defaults: new
                 {
                     controller = "WApiEmployee1095summary",
                     action = "UnReviewed1095"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
                  name: "employee1095summary-reviewed1095",
                  routeTemplate: "Employee1095summary/Reviewed1095/",
                  defaults: new
                  {
                      controller = "WApiEmployee1095summary",
                      action = "Reviewed1095"
                  },
                  constraints: new
                  {
                      httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                  }
             );

            config.Routes.MapHttpRoute(
                 name: "employee1095summary-get-many-monthly-details",
                 routeTemplate: "Employee1095summary/GetPart2ForEmployee/",
                 defaults: new
                 {
                     controller = "WApiEmployee1095summary",
                     action = "GetPart2ForEmployee"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
                 name: "employee1095summary-get-many-covered-individuals",
                 routeTemplate: "Employee1095summary/GetPart3ForEmployee/",
                 defaults: new
                 {
                     controller = "WApiEmployee1095summary",
                     action = "GetPart3ForEmployee"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
                 name: "employee1095summary-get-1095summary-by-resource-id",
                 routeTemplate: "Employee1095summary/GetByResourceId/",
                 defaults: new
                 {
                     controller = "WApiEmployee1095summary",
                     action = "GetSingleForTaxYear"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }


            );

            config.Routes.MapHttpRoute(
                 name: "employee1095summary-update-many-covered-individuals",
                 routeTemplate: "Employee1095summary/UpdatePart2ForEmployee/",
                 defaults: new
                 {
                     controller = "WApiEmployee1095summary",
                     action = "UpdatePart2ForEmployee"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)

                 }
                 );

            config.Routes.MapHttpRoute(
                  name: "employee1095Part3-Update",
                  routeTemplate: "Employee1095summary/UpdateInsuranceCoverage/",
                  defaults: new
                  {
                      controller = "WApiEmployee1095summary",
                      action = "UpdateInsuranceCoverage"
                  },
                  constraints: new
                  {
                      httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                  }
             );


            config.Routes.MapHttpRoute(
                 name: "employee1095Part3-Delete",
                 routeTemplate: "Employee1095summary/DeleteInsuranceCoverage/",
                 defaults: new
                 {
                     controller = "WApiEmployee1095summary",
                     action = "DeleteInsuranceCoverage"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
                  name: "FileCabinetInfo-Save",
                  routeTemplate: "FileCabinetInfo/SaveFileCabinetInfo/",
                  defaults: new
                  {
                      controller = "WApiFileCabinetInfo",
                      action = "SaveFileCabinetInfo"
                  },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }

                );

            config.Routes.MapHttpRoute(
                  name: "FileCabinetInfo-Delete",
                  routeTemplate: "FileCabinetInfo/DeleteFileCabinetInfo/",
                  defaults: new
                  {
                      controller = "WApiFileCabinetInfo",
                      action = "DeleteFileCabinetInfo"
                  },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }

                );

            config.Routes.MapHttpRoute(
                name: "FileCabinetAccess-GetByFolders",
                routeTemplate: "FileCabinetAccess/GetByOwnerGuid/",
                defaults: new
                {
                    controller = "WApiFileCabinetInfo",
                    action = "GetFilesInFolders"
                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }

                );
            config.Routes.MapHttpRoute(
                 name: "FileCabinetInfo-Get",
                 routeTemplate: "FileCabinetInfo/GetFilesForEmployerTaxYear/",
                 defaults: new
                 {
                     controller = "WApiFileCabinetInfo",
                     action = "GetFilesForEmployerTaxYear"
                 },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }

               );


            config.Routes.MapHttpRoute(
                name: "FileCabinetInfo-GetFilesForParticularFolder",
                routeTemplate: "FileCabinetInfo/GetFilesForParticularFolder/",
                defaults: new
                {
                    controller = "WApiFileCabinetInfo",
                    action = "GetFilesForParticularFolder"
                },
               constraints: new
               {
                   httpMethod = new HttpMethodConstraint(HttpMethod.Post)
               }

              );


            config.Routes.MapHttpRoute(
               name: "FileCabinetInfo-DeleteFolderFileCabinetInfo",
               routeTemplate: "FileCabinetInfo/DeleteFolderFileCabinetInfo/",
               defaults: new
               {
                   controller = "WApiFileCabinetInfo",
                   action = "DeleteFolderFileCabinetInfo"
               },
              constraints: new
              {
                  httpMethod = new HttpMethodConstraint(HttpMethod.Post)
              }

             );

            config.Routes.MapHttpRoute(
                    name: "filecabinet-getsingle",
                    routeTemplate: "FileCabinetInfo/GetOneFileCabinetInfo/",
                    defaults: new
                    {
                        controller = "WApiFileCabinetInfo",
                        action = "GetByResourceId"
                    },
                    constraints: new
                    {
                        httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                    }
            );

            config.Routes.MapHttpRoute(
                 name: "employee1095Part3-Add",
                 routeTemplate: "Employee1095summary/AddInsuranceCoverage/",
                 defaults: new
                 {
                     controller = "WApiEmployee1095summary",
                     action = "AddInsuranceCoverage"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
                 name: "print-1095",
                 routeTemplate: "Print1095/",
                 defaults: new
                 {
                     controller = "WApiPrintBatch",
                     action = "Print1095"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );


            config.Routes.MapHttpRoute(
                name: "employer1094Summary",
                routeTemplate: "Employer1094summary/GetEmployers1094DetailsForTaxYear/",
                defaults: new
                {
                    controller = "WApiEmployer1094summary",
                    action = "GetEmployers1094DetailsForTaxYear"

                },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                }
           );


            config.Routes.MapHttpRoute(
                 name: "employer1094Summary-Finalize1094",
                 routeTemplate: "Employer1094summary/Finalize1094/",
                 defaults: new
                 {
                     controller = "WApiEmployer1094summary",
                     action = "Finalize1094"
                 },
                 constraints: new
                 {
                     httpMethod = new HttpMethodConstraint(HttpMethod.Post)
                 }
            );

            config.Routes.MapHttpRoute(
               name: "employer1094Summary-GetAllActiveEmployers",
               routeTemplate: "Employee1095summary/GetAllActiveEmployers/",
               defaults: new
               {
                   controller = "Employee1095summary",
                   action = "GetAllActiveEmployers"
               },
               constraints: new
               {
                   httpMethod = new HttpMethodConstraint(HttpMethod.Post)
               }
          );
        }
    }
}
