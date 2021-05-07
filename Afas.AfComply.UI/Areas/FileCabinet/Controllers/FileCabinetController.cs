using Afas.AfComply.Reporting.Core.Models;
using Afas.AfComply.Reporting.Core.Request;
using Afas.AfComply.Reporting.Core.Response;
using Afas.AfComply.UI.Areas.Administration.Controllers;
using Afas.AfComply.UI.Areas.ViewModels.FileCabinet;
using Afas.Application.Archiver;
using Afas.Application.Services;
using Afas.Domain.POCO;
using Afc.Core.Application;
using Afc.Core.Logging;
using Afc.Core.Presentation.Web;
using Afc.Marketing;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;



namespace Afas.AfComply.UI.Areas.FileCabinet.Controllers
{

    [CookieTokenAuthCheckAttribute]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class FileCabinetController : BaseReadOnlyController<FileCabinetInfoModel, FileCabinetInfoViewModel>
    {
        private string SaveFileCabinetInfo { get; set; }

        private string DeleteFileCabinetInfo { get; set; }

        private string DeleteFolderFileCabinetInfo { get; set; }

        private string GetFilesForParticularFolder { get; set; }

        private string GetByOwnerGuid { get; set; }

        private string GetSingleInfo { get; set; }

        private readonly IFileArchiver Archiver;

        private readonly IArchiveFileInfoService ArchiveService;

        public FileCabinetController(
            ILogger logger,
            IEncryptedParameters encryptedParameters,
            IFileArchiver archiver,
            IArchiveFileInfoService archiveService,
            IApiHelper apiHelper,
             ITransactionContext transactionContext) :
            base(logger, encryptedParameters, apiHelper)
        {
            this.SaveFileCabinetInfo = "SaveFileCabinetInfo-FileCabinetInfo";
            this.DeleteFileCabinetInfo = "DeleteFileCabinetInfo-FileCabinetInfo";
            this.GetByOwnerGuid = "GetByOwnerGuid-FileCabinetAccess";
            this.GetSingleInfo = "GetSingle-FileCabinetInfo";
            this.GetFilesForParticularFolder = "GetFilesForParticularFolder-FileCabinetInfo";
            this.DeleteFolderFileCabinetInfo = "DeleteFolderFileCabinetInfo-FileCabinetInfo";


            if (null == archiver)
            {
                throw new ArgumentNullException("Archiver");
            }
            this.Archiver = archiver;

            if (null == archiveService)
            {
                throw new ArgumentNullException("ArchiveService");
            }
            this.ArchiveService = archiveService;

            this.ArchiveService.Context = transactionContext;
        }


        public ActionResult Index()
        {
            return this.View();
        }


        [HttpPost]
        public ActionResult uploadFile(HttpPostedFileBase postedFiles, string fileName, string fileDescription, string FolderEncryptedValues)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            if (postedFiles == null || postedFiles.ContentLength <= 0)
            {
                if (this.HttpContext.Request.Files.AllKeys.Any())
                {
                    postedFiles = this.HttpContext.Request.Files[0];
                }
            }

            if (postedFiles == null || postedFiles.ContentLength <= 0)
            {
                return null;
            }
            if (postedFiles.ContentLength > 0)
            {

                base.Load(FolderEncryptedValues);
                Guid FolderresourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());

                string FileExtension = Path.GetExtension(postedFiles.FileName);
                int employerId = int.Parse(CookieTokenAuthCheckAttribute.GetEmployerId(this.HttpContext));
                string path = this.Server.MapPath("..\\..\\..\\ftps\\filecabinet\\uploadedfiles\\");
                string saveTo = path + employerId + "\\";
                string filepath = Path.GetFullPath(saveTo);
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                saveTo += fileName;
                saveTo += FileExtension;
                postedFiles.SaveAs(saveTo);

                FileInfo temp = new System.IO.FileInfo(saveTo);
                string extension = temp.Extension;
                int CurrentApplicationId = 1;
                int archiveId = this.Archiver.ArchiveFile(saveTo, this.employerResourceId, "File Sent to File Cabinet", employerId);
                ArchiveFileInfo archive = this.ArchiveService.GetById(archiveId);
                FileCabinetInfoModel model = new FileCabinetInfoModel()
                {
                    Filename = fileName,
                    FileDescription = fileDescription,
                    FileType = extension,
                    OwnerResourceId = Guid.Parse((this.employerResourceId).ToString()),
                    ApplicationId = CurrentApplicationId,
                    ArchiveFileInfo = archive,
                    OtherResourceId = null,
                    FileCabinetFolderInfo = null

                };
                NewChildItemRequest<FileCabinetInfoModel> request = new NewChildItemRequest<FileCabinetInfoModel>
                {
                    model = model,
                    ResourceId = FolderresourceId,
                    Requester = this.requester
                };
                ModelListResponse<FileCabinetInfoModel> message = this.ApiHelper.Send<ModelListResponse<FileCabinetInfoModel>, NewChildItemRequest<FileCabinetInfoModel>>(this.SaveFileCabinetInfo, request);
            }

            return null;
        }

        [HttpGet]
        public FileStreamResult GetFileByParameter(string encryptedParameters)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            base.Load(encryptedParameters);
            string user = CookieTokenAuthCheckAttribute.GetUserId(this.HttpContext);
            Guid resourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());
            GetByGuidRequest request = new GetByGuidRequest
            {
                ResourceId = resourceId,
                Requester = user
            };

            ModelItemResponse<FileCabinetInfoModel> message = this.ApiHelper.Send<ModelItemResponse<FileCabinetInfoModel>, GetByGuidRequest>(this.GetSingleInfo, request);

            string filePath = message.Model.ArchiveFileInfo.ArchiveFilePath;
            string fileName = message.Model.Filename + message.Model.FileType;
            Guid ArchiveResourceId = message.Model.ArchiveFileInfo.ResourceId;

            if (false == message.Model.OwnerResourceId.Equals(this.employerResourceId))
            {
                string emailMessage = string.Format("File Cabinet Error: User [{0}] logged into Employer [{1}] tried to download a File [{2}] belonging to Employer [{3}]. \r\n\r\n"
                    + "This may have been an accedental error; only if the User is a CSR, or works on multiple accounts (of which both Employers are correct). \r\n\r\n ",
                    user,
                    this.employerResourceId,
                    resourceId,
                    message.Model.OwnerResourceId);

                this.Log.Error(emailMessage);

                Email em = new Email();

                em.SendEmail(SystemSettings.IrsProcessingAddress, "File Cabinet Download Error", emailMessage, false);

                return null;
            }

            if (false == System.IO.File.Exists(filePath))
            {
                return null;
            }

            PIILogger.LogPII(string.Format("File[{0}] downloaded From the File Cabinet by a user[{1}] where path of the file is [{2}] and Resource id in Archive table is[{3}]", resourceId, user, filePath, ArchiveResourceId));

            FileStream fs = new FileStream(filePath, FileMode.Open);

            stopwatch.Stop();
            this.Log.Info(string.Format("GetFileByParameter Returned the archive in [{0}]ms.", stopwatch.ElapsedMilliseconds));

            return this.File(fs, "application/octet-stream", fileName);

        }

        [HttpGet]
        public FileResult ZipDownload(string encryptedParameters)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            base.Load(encryptedParameters);
            Guid FolderresourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());
            Guid employerResourceId = Guid.Parse(CookieTokenAuthCheckAttribute.GetEmployerResourceId(this.HttpContext));
            FileCabinetFilesRequests request = new FileCabinetFilesRequests
            {
                ResourceId = FolderresourceId,
                Requester = this.requester,
                OwnerResourceId = employerResourceId
            };

            ModelListResponse<FileCabinetInfoModel> message = this.ApiHelper.Send<ModelListResponse<FileCabinetInfoModel>, FileCabinetFilesRequests>(this.GetFilesForParticularFolder, request);

            string FolderName = message.Models[0].FileCabinetFolderInfo.FolderName;

            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (FileCabinetInfoModel file in message.Models)
                    {
                        string filepath = file.ArchiveFileInfo.ArchiveFilePath;
                        string fileName = file.Filename + file.FileType;
                        byte[] bytes = System.IO.File.ReadAllBytes(filepath);
                        ZipArchiveEntry zipEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);

                        using (Stream zipStream = zipEntry.Open())
                        {
                            zipStream.Write(bytes, 0, bytes.Length);
                            PIILogger.LogPII(string.Format("File[{0}]  added to a zip for downloading, and the path of the file is [{1}]", fileName, filepath));

                        }
                    }
                }

                PIILogger.LogPII(string.Format("User[{0}] Downloaded a Files in form of Zip From a folder [{1}] and ResourceId of Folder is[{2}]", this.User, FolderName, FolderresourceId));
                stopwatch.Stop();
                this.Log.Info(string.Format("ZipDownload Returned the archive in [{0}]ms.", stopwatch.ElapsedMilliseconds));
                return this.File(ms.ToArray(), "application/Zip", FolderName + "AllFiles.zip");

            }

        }

        [HttpGet]
        public ActionResult GetFolders(string FolderEncryptedValues)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            FileCabinetFilesRequests request = new FileCabinetFilesRequests
            {
                Requester = this.requester,
                ResourceId = this.employerResourceId,
                CurrentApplicationId = 1
            };
            ModelListResponse<FileCabinetAccessModel> message = this.ApiHelper.Send<ModelListResponse<FileCabinetAccessModel>, FileCabinetFilesRequests>(this.GetByOwnerGuid, request);
            IList<FileCabinetFolderAccessInfoViewModel> viewModels = Mapper.Map<IList<FileCabinetAccessModel>, IList<FileCabinetFolderAccessInfoViewModel>>(message.Models);
            string jsonOut = "";
            JsonSerializer ser = JsonSerializer.CreateDefault();
            using (StringWriter str = new StringWriter())
            {
                ser.Serialize(str, viewModels);
                jsonOut = str.ToString();
            }
            ContentResult result = this.Content(jsonOut, "application/json");

            stopwatch.Stop();
            this.Log.Info(string.Format("Get-Many Returned [{0}] view models in [{1}]ms.", viewModels.Count, stopwatch.ElapsedMilliseconds));
            return result;
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        [CompressFilter]
        [HttpGet]
        public ActionResult GetFilesForFolder(string encryptedParameters)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            base.Load(encryptedParameters);
            Guid FolderresourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());
            Guid employerResourceId = Guid.Parse(CookieTokenAuthCheckAttribute.GetEmployerResourceId(this.HttpContext));

            FileCabinetFilesRequests request = new FileCabinetFilesRequests
            {
                ResourceId = FolderresourceId,
                Requester = this.requester,
                OwnerResourceId = employerResourceId
            };
            ModelListResponse<FileCabinetInfoModel> message = this.ApiHelper.Send<ModelListResponse<FileCabinetInfoModel>, FileCabinetFilesRequests>(this.GetFilesForParticularFolder, request);
            IList<FileCabinetInfoViewModel> viewModels = Mapper.Map<IList<FileCabinetInfoModel>, IList<FileCabinetInfoViewModel>>(message.Models);

            string jsonOut = "";
            JsonSerializer ser = JsonSerializer.CreateDefault();

            using (StringWriter str = new StringWriter())
            {
                ser.Serialize(str, viewModels);
                jsonOut = str.ToString();
            }
            ContentResult result = this.Content(jsonOut, "application/json");
            stopwatch.Stop();
            this.Log.Info(string.Format("Get-Many Returned [{0}] view models in [{1}]ms.", viewModels.Count, stopwatch.ElapsedMilliseconds));

            return result;
        }


        [HttpPost]
        public virtual ActionResult DeleteFile(string encryptedParameters)
        {
            base.Load(encryptedParameters);
            DeleteItemRequest<FileCabinetInfoModel> request = new DeleteItemRequest<FileCabinetInfoModel>();

            Guid ResourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());
            request.ResourceId = ResourceId;

            request.Requester = this.requester;

            ModelItemResponse<FileCabinetInfoModel> message = this.ApiHelper.Send<ModelItemResponse<FileCabinetInfoModel>, DeleteItemRequest<FileCabinetInfoModel>>(
                this.DeleteFileCabinetInfo,
                request);

            FileCabinetInfoModel model = message.Model;
            FileCabinetInfoViewModel viewModel = Mapper.Map<FileCabinetInfoModel, FileCabinetInfoViewModel>(model);

            return this.Json(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteFolder(string encryptedParameters)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            base.Load(encryptedParameters);
            Guid FolderresourceId = Guid.Parse(this.EncryptedParameters["ResourceId"].ToString());
            Guid employerResourceId = Guid.Parse(CookieTokenAuthCheckAttribute.GetEmployerResourceId(this.HttpContext));

            FileCabinetFilesRequests request = new FileCabinetFilesRequests
            {
                ResourceId = FolderresourceId,
                Requester = this.requester,
                OwnerResourceId = employerResourceId
            };
            ModelListResponse<FileCabinetInfoModel> message = this.ApiHelper.Send<ModelListResponse<FileCabinetInfoModel>, FileCabinetFilesRequests>(this.DeleteFolderFileCabinetInfo, request);

            stopwatch.Stop();

            return null;
        }
    }
}