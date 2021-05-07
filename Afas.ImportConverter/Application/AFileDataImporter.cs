using Afas.ImportConverter.Domain.POCO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Afas.Domain;
using Afas.Application.FileAccess;
using Afas.Application.Archiver;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Application;
using Afas.ImportConverter.Application.Services;
using Afas.ImportConverter.Domain.ImportFormatting.UploadedData;
using Afas.Domain.POCO;
using Afas.Application.Services;
using Afc.Core.Application;
using System.Data.Entity;
using System.Data.OleDb;
using ClosedXML.Excel;
using System.Linq;

namespace Afas.ImportConverter.Application
{
    public abstract class AFileDataImporter : ADataImporter
    {

        private ILog Log = LogManager.GetLogger(typeof(AFileDataImporter));

        protected Guid OwnerGuid;

        protected int OwnerId;

        protected IFileArchiver FileArchiver { get; set; }

        protected IArchiveFileInfoService FileArchiveService { get; set; }

        protected IFileAccess FileAccess { get; set; }

        protected ICsvFileDataTableBuilder CsvFileDataTableBuilder { get; set; }

        protected IXlsxFileDataTableBuilder XlsxFileDataTableBuilder { get; set; }




        public AFileDataImporter
            (IFileArchiver FileArchiver,
            IFileAccess FileAccess,
            IArchiveFileInfoService FileArchiveService,
            ICsvFileDataTableBuilder CsvFileDataTableBuilder,
            IXlsxFileDataTableBuilder XlsxFileDataTableBuilder,
            IStagingImportService StagingImportFactory,
            IDataProcessor DataProcessor,
            IUploadedFileService UploadDataInfoFactory,
            IDataValidationManager DataValidator,
            ITransactionContext transactionContext)
            : base(StagingImportFactory, DataProcessor, UploadDataInfoFactory, DataValidator, transactionContext)
        {

            if (null == FileArchiver)
            {
                throw new ArgumentNullException("FileArchiver");
            }
            this.FileArchiver = FileArchiver;

            if (null == FileAccess)
            {
                throw new ArgumentNullException("FileAccess");
            }
            this.FileAccess = FileAccess;

            if (null == FileArchiveService)
            {
                throw new ArgumentNullException("FileArchiveService");
            }
            this.FileArchiveService = FileArchiveService;
            this.FileArchiveService.Context = transactionContext;

            if (null == CsvFileDataTableBuilder)
            {
                throw new ArgumentNullException("CsvFileDataTableBuilder");
            }
            this.CsvFileDataTableBuilder = CsvFileDataTableBuilder;

            if (null == XlsxFileDataTableBuilder)
            {
                throw new ArgumentNullException("XlsxFileDataTableBuilder");
            }
            this.XlsxFileDataTableBuilder = XlsxFileDataTableBuilder;


        }

        public void Setup(string sourceFilePath, Guid ownerGuid, int ownerId)
        { 

            CheckFileExists(sourceFilePath);

            ImportFileMetaData meta = new ImportFileMetaData();
            meta.SourceFilePath = sourceFilePath;
            meta.FileTypeDescription = FileAccess.GetExtension(sourceFilePath).Trim().Trim('.').ToLower();

            if (meta.FileTypeDescription.IsNullOrEmpty())
            {
                throw new ArgumentException("Could not determine Uploaded File Type.");
            }

            this.importData.MetaData = meta;

            this.OwnerGuid = ownerGuid;
            this.OwnerId = ownerId;

        }

        protected void CheckFileExists(string sourceFilePath)
        {

            if (null == sourceFilePath || false == FileAccess.FileExists(sourceFilePath))
            {
                Log.Error("Tried to upload nonexistent File at path: " + sourceFilePath);

                throw new ArgumentException("Uploaded File Doesn't exist.");
            }

        }

        protected override void SaveUploadInfo()
        {
            if (uploadInfo == null)
            {
                UploadedFileInfo uploadLoad = new UploadedFileInfo();
                uploadLoad.ArchiveFileInfo = null;
                uploadLoad.FileTypeDescription = ((ImportFileMetaData)this.importData.MetaData).FileTypeDescription;
                uploadLoad.FileName = ((ImportFileMetaData)this.importData.MetaData).SourceFilePath;
                uploadInfo = uploadLoad;
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

        protected override void ParseToDataTable()
        {

            this.importData.Data = null;

            ImportFileMetaData meta = ((ImportFileMetaData)this.importData.MetaData);

            switch (meta.FileTypeDescription)
            {
                case "csv":
                    this.importData.Data = CsvFileDataTableBuilder.CreateDataTableFromCsvFile(meta.SourceFilePath);
                    break;

                case "xls":
                    throw new NotImplementedException();

                case "xltm":                    
                case "xltx":
                case "xlsm":
                case "xlsx":

                    this.importData.Data = XlsxFileDataTableBuilder.CreateDataTableFromXlsxFile(meta.SourceFilePath);
                    break;

                case "tsv":

                    throw new NotImplementedException();

                case "txt":
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }

            string SourceFilePath = ((ImportFileMetaData)this.importData.MetaData).SourceFilePath;

            if (null == this.importData.Data)
            {

                Log.Error(String.Format("Data Table Parsing Failed for File at [{0}]", SourceFilePath));

                throw new ApplicationException("Failed to parse File Data.");

            }
            else if (this.importData.Data.Columns.Count <= 0 || this.importData.Data.Rows.Count <= 0)
            {

                Log.Error(String.Format(
                    "Data Table Parsing returned empty Table. Columns Count: [{0}], Rows Count: [{1}], User Id: [{2}] File source: [{3}]",
                    this.importData.Data.Columns.Count,
                    this.importData.Data.Rows.Count,
                    UserId,
                    SourceFilePath));

                throw new ApplicationException("Parsed File Data was Empty.");

            }

            /// Set the title (nessisary to create xml)
            this.importData.Data.TableName = "Import";
        }

        protected override void CleanUpDataSource()
        {
            ArchiveFile();
        }
    
        protected void ArchiveFile()
        {

            string SourceFilePath = ((ImportFileMetaData)this.importData.MetaData).SourceFilePath;

            int ArchiveId = FileArchiver.ArchiveFile(
                SourceFilePath,
                OwnerGuid,
                "Archive By New Importer",
                OwnerId);

            if (ArchiveId <= 0)
            {

                Log.Error(String.Format(
                    "Failed to ArchiveFile. ArchiveId: [{0}], User: [{1}]",
                    ArchiveId,
                    UserId));

                throw new ApplicationException("Archival Issue, Please contact IT.");

            }

            ArchiveFileInfo archive = this.FileArchiveService.GetById(ArchiveId);

            archive.ResourceId = Guid.NewGuid();

            ((UploadedFileInfo)uploadInfo).ArchiveFileInfo = archive;

            if (null == UploadDataInfoService.UpdateEntity(uploadInfo, UserId))
            {

                Log.Error(String.Format(
                    "Failed to update Upload Info. UploadId: [{0}], ArchiveId: [{1}], User: [{2}]",
                    uploadInfo.ResourceId,
                    ArchiveId,
                    UserId));

                throw new ApplicationException("DataBase Access Issue, Please contact IT.");

            }
        }
    }
}