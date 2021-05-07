using Afas.ImportConverter.Domain.POCO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Afas.Domain;
using Afas.Application.Archiver;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.Domain.POCO;
using Afas.ImportConverter.Application;
using System.Linq;
using Afc.Core.Domain;
using Afas.ImportConverter.Domain.ImportFormatting.Staging;
using Afas.ImportConverter.Application.Services;
using Afas.ImportConverter.Domain.ImportFormatting.UploadedData;
using Afc.Core.Application;

namespace Afas.ImportConverter.Application
{

    public abstract class ADataImporter
    {

        private ILog Log = LogManager.GetLogger(typeof(ADataImporter));

        protected readonly ImportData importData; 

        protected string UserId { get; private set; }

        protected StagingImport stagingImport { get; set; }

        protected BaseUploadedDataInfo uploadInfo { get; set; }
        
        protected IStagingImportService StagingImportService { get; set; }

        protected IDataProcessor DataProcessor { get; set; }

        protected IUploadedDataInfoService UploadDataInfoService { get; set; }

        protected IDataValidationManager DataValidator { get; set; }

        public IList<ValidationFailure> DataValidationMessages { get; protected set; }

        protected ITransactionContext TransactionContext { get; set; }

        public ADataImporter(
            IStagingImportService StagingImportService,
            IDataProcessor DataProcessor,
            IUploadedDataInfoService UploadDataInfoService,
            IDataValidationManager DataValidator,
            ITransactionContext transactionContext)
        {

            if (null == transactionContext)
            {
                throw new ArgumentNullException("TransactionContext");
            }
            this.TransactionContext = transactionContext;

            if (null == StagingImportService)
            {
                throw new ArgumentNullException("StagingImportFactory");
            }
            this.StagingImportService = StagingImportService;
            this.StagingImportService.Context = transactionContext;

            if (null == DataProcessor)
            {
                throw new ArgumentNullException("DataProcessor");
            }       
            this.DataProcessor = DataProcessor;

            if(null == UploadDataInfoService)
            {
                throw new ArgumentNullException("UploadDataInfoFactory");
            }            
            this.UploadDataInfoService = UploadDataInfoService;
            this.UploadDataInfoService.Context = transactionContext;

            if (null == DataValidator)
            {
                throw new ArgumentNullException("DataValidator");
            }
            this.DataValidator = DataValidator;
            
            this.importData = new ImportData();
            this.importData.Data = null;

            this.DataValidationMessages = new List<ValidationFailure>();
        }

        public virtual bool ImportData(string userId, string uploadSource, string uploadDataType)
        {
            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentNullException("userId");
            }
            this.UserId = userId;

            if (uploadSource.IsNullOrEmpty())
            {
                throw new ArgumentNullException("uploadSource");
            }
            this.importData.MetaData.UploadSourceDescription = uploadSource;

            if (uploadDataType.IsNullOrEmpty())
            {
                throw new ArgumentNullException("uploadDataType");
            }
            this.importData.MetaData.UploadTypeDescription = uploadDataType;

            DataValidationMessages = new List<ValidationFailure>();

            SaveUploadInfo();
            try
            {

                ParseToDataTable();

                PrevalidateData();

                SaveDataToStaging();

                CleanUpDataSource();

                CleanupAndPreProcessing();

                DataTable RemainingData = ImportProcessedData();
                RemainingData.TableName = "RemainingData";

                return UpdateDatabase(RemainingData);

            }
            catch (Exception ex)
            {
                Log.Error(
                        "There was a problem while importing data.",
                        ex);

                uploadInfo.ProcessingFailed = true;

                if (null == UploadDataInfoService.UpdateEntity(uploadInfo, UserId))
                {
                    Log.Error("Failed to Save Upload Data Info Failed, resourceId: "+ uploadInfo.ResourceId);

                    throw new ApplicationException("Database Access Issue, Please contact IT.");
                }

                return false;
            }
        }

        protected virtual void SaveUploadInfo()
        {
            if(uploadInfo == null)
            {
                uploadInfo = new UploadedDataInfo();
            }
            uploadInfo.UploadedByUser = UserId;
            uploadInfo.UploadSourceDescription = this.importData.MetaData.UploadSourceDescription;
            uploadInfo.UploadTime = DateTime.Now;
            uploadInfo.UploadTypeDescription = this.importData.MetaData.UploadTypeDescription;

            uploadInfo = UploadDataInfoService.AddNewEntity(uploadInfo, UserId);

            if (null == uploadInfo || null == uploadInfo.ResourceId || uploadInfo.ResourceId == new Guid())
            {
                Log.Error("Failed to Save Upload Data Info.");

                throw new ApplicationException("Database Access Issue, Please contact IT.");
            }
        }



        protected abstract void ParseToDataTable();

        protected virtual void PrevalidateData() 
        {
            if (false == DataValidator.IsDataValid(this.importData))
            {
                Log.Error(String.Format(
                    "Uploaded Data failed Validation for UploadId: [{0}], User: [{1}]",
                    uploadInfo.ResourceId,
                    UserId));

                this.DataValidationMessages = this.DataValidationMessages.Concat(DataValidator.DataValidationMessages).ToList();

                uploadInfo.Processed = false;
                uploadInfo.ProcessingFailed = true;
                UploadDataInfoService.UpdateEntity(uploadInfo, UserId);

                throw new ApplicationException("Data failed to validate.");
            }

        }

        protected virtual void SaveDataToStaging()
        {
            if(stagingImport == null)
            {
                stagingImport = new StagingImport();
            }
            stagingImport.Original = this.importData.Data;
            stagingImport.UploadInfo = uploadInfo;
            
            stagingImport = StagingImportService.AddNewEntity(stagingImport, UserId);
            
            if (null == stagingImport || stagingImport.ResourceId == null || stagingImport.ResourceId == new Guid())
            {
                Log.Error(String.Format(
                    "Failed to Stage DataTable. Staging: [{0}], UploadResourceId: [{1}], User: [{2}]",
                    stagingImport,
                    uploadInfo.ResourceId,
                    UserId));

                throw new ApplicationException("Database Access Issue, Please contact IT.");
            }
        }

        protected abstract void CleanUpDataSource();

        protected virtual void CleanupAndPreProcessing()
        {
            this.importData.Data.CleanDataTable();

            stagingImport.Modified = this.importData.Data;

            if (null == StagingImportService.UpdateEntity(stagingImport, UserId))
            {
                Log.Error(String.Format(
                        "Failed to update Staging Modified. UploadResourceId: [{0}], StagingResourceId: [{1}], User: [{2}]",
                        uploadInfo.ResourceId,
                        stagingImport.ResourceId,
                        UserId));

                throw new ApplicationException("Database Access Issue, Please contact IT.");
            }

            if (false == DataProcessor.ProcessImportDataTable(this.importData))
            {
                Log.Error("Issue while processing Data.");



            }

            stagingImport.Modified = this.importData.Data;

            stagingImport = StagingImportService.UpdateAfterProcessing(stagingImport, UserId);

            if (null == stagingImport)
            {
                Log.Error("Failed to update Staging After Reprocess.");

                throw new ApplicationException("DataBase Access Issue, Please contact IT.");
            }
        }

        protected abstract DataTable ImportProcessedData();
        
        protected virtual bool UpdateDatabase(DataTable RemainingData)
        {
            if (null != RemainingData && null != RemainingData.Rows)
            {
                if (RemainingData.Rows.Count <= 0)
                {

                    if (null == StagingImportService.DeactivateEntity(stagingImport.ResourceId, UserId))
                    {
                        Log.Error("Failed to Update Staging Entity status, ResourceId: " + stagingImport.ResourceId);

                        throw new ApplicationException("DataBase Access Issue, Please contact IT.");
                    }

                    uploadInfo.Processed = true;

                    if (null == UploadDataInfoService.UpdateEntity(uploadInfo, UserId)) 
                    {
                        Log.Error("Failed to Update Upload Info Processed Status, ResourceId: " + uploadInfo.ResourceId);

                        throw new ApplicationException("DataBase Access Issue, Please contact IT.");
                    }

                    return true;
                }
                else
                {
                    stagingImport.Modified = RemainingData;

                    stagingImport = StagingImportService.UpdateAfterProcessing(stagingImport, UserId);
                    
                    if (null == stagingImport || null == stagingImport.Original || stagingImport.Original.Rows.Count != RemainingData.Rows.Count)
                    {
                        Log.Error("Failed to Update Remaining Data on stagingImport: " + stagingImport);
                    }

                    uploadInfo.ProcessingFailed = true;

                    if (null == UploadDataInfoService.UpdateEntity(uploadInfo, UserId))
                    {
                        Log.Error("Failed to Update Upload Info Failed Status, ResourceId: " + uploadInfo.ResourceId);

                        throw new ApplicationException("DataBase Access Issue, Please contact IT.");
                    }                    

                    return false;
                }
            }
            else
            {
                Log.Error("Remaining Data was null or empty.");

                throw new ApplicationException("Unimported Data returned was null.");
            }
        }
    }
}