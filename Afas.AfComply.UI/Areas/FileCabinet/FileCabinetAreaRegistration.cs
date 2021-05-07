using Afas.AfComply.UI.ApiControllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace Afas.AfComply.UI.Areas.FileCabinet
{
    public class FileCabinetAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FileCabinet";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                name: "refresh-token-file-cabinet",
                url: AreaName + "/RenewToken",
                defaults: new { controller = "Authentication", action = "GetAntiForgeryToken" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "root-file-cabinet",
                url: AreaName + "/",
                defaults: new { controller = "FileCabinet", action = "Index" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timeframe-get-all-file-cabinet",
                url: AreaName + "/TimeFrameApi/",
                defaults: new { controller = "TimeFrame", action = "GetAll" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timeframe-get-many-file-cabinet",
                url: AreaName + "/TimeFrameApi/Multiple/{encryptedParameters}",
                defaults: new { controller = "TimeFrame", action = "GetMany" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
                name: "timesframe-get-by-id-file-cabinet",
                url: AreaName + "/TimeFrameApi/{encryptedParameters}",
                defaults: new { controller = "TimeFrame", action = "GetSingle" },
                constraints: new
                {
                    encryptedParameters = new EncryptedParametersRouteConstraint(),
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );


            context.MapRoute(
                name: "uploadfile-filecabinet",
                url: AreaName + "/FileCabinetInfoApi/UploadFile",
                defaults: new { controller = "FileCabinet", action = "uploadFile" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Post")
                }
            );

            context.MapRoute(
                name: "GetByFolders-filecabinet",
                url: AreaName + "/FileCabinetInfoApi/GetFolders",
                defaults: new { controller = "FileCabinet", action = "GetFolders" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

            context.MapRoute(
             name: "download-file-filecabinet",
             url: AreaName + "/FileCabinetInfoApi/DownloadFile/{encryptedParameters}",
             defaults: new { controller = "FileCabinet", action = "GetFileByParameter" },
             constraints: new
             {
                 encryptedParameters = new EncryptedParametersRouteConstraint(),
                 httpMethod = new HttpMethodConstraint("Get")
             }
          );
            context.MapRoute(
              name: "Zipdownload-file-filecabinet",
              url: AreaName + "/FileCabinetFolderAccessInfoApi/ZipDownload/{encryptedParameters}",
              defaults: new { controller = "FileCabinet", action = "ZipDownload" },
              constraints: new
              {
                  encryptedParameters = new EncryptedParametersRouteConstraint(),
                  httpMethod = new HttpMethodConstraint("Get")
              }
           );

            context.MapRoute(
             name: "delete-file-filecabinet",
             url: AreaName + "/FileCabinetInfoApi/Delete/{encryptedParameters}",
             defaults: new { controller = "FileCabinet", action = "DeleteFile" },
             constraints: new
             {
                 encryptedParameters = new EncryptedParametersRouteConstraint(),
                 httpMethod = new HttpMethodConstraint("Post")
             }
          );

            context.MapRoute(
               name: "DeleteFolder-filecabinet",
               url: AreaName + "/FileCabinetFolderAccessInfoApi/FolderDelete/{encryptedParameters}",
               defaults: new { controller = "FileCabinet", action = "DeleteFolder" },
               constraints: new
               {
                   encryptedParameters = new EncryptedParametersRouteConstraint(),
                   httpMethod = new HttpMethodConstraint("Post")
               }
           );


            context.MapRoute(
              name: "GetFilesForFolder-filecabinet",
              url: AreaName + "/FileCabinetInfoApi/GetFilesForFolder/{encryptedParameters}",
              defaults: new { controller = "FileCabinet", action = "GetFilesForFolder" },
              constraints: new
              {
                  encryptedParameters = new EncryptedParametersRouteConstraint(),
                  httpMethod = new HttpMethodConstraint("Get")
              }
           );

            context.MapRoute(
                name: "catch-all-file-cabinet",
                url: AreaName + "/{*url}",
                defaults: new { controller = "FileCabinet", action = "Index" },
                constraints: new
                {
                    httpMethod = new HttpMethodConstraint("Get")
                }
            );

        }
    }
}