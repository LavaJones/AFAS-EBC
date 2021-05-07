using Afas.AfComply.Application.Validators;
using Afas.AfComply.Domain;
using Afas.Application.Archiver;
using Afas.Application.FileAccess;
using Afas.Application.Services;
using Afas.ImportConverter.Application;
using Afas.ImportConverter.Application.Services;
using Afas.ImportConverter.Domain.POCO;
using Afc.Core.Application;
using log4net;
using System;
using System.Data;
using System.Linq;
public class AfComplyFileDataImporter : AFileDataImporter
{
    private readonly ILog Log = LogManager.GetLogger(typeof(AfComplyFileDataImporter));

    protected int EmployerId;
    protected int? PlanYearId;

    public AfComplyFileDataImporter(
            IFileArchiver FileArchiver,
            IFileAccess FileAccess,
            IArchiveFileInfoService FileArchiveService,
            ICsvFileDataTableBuilder CsvFileDataTableBuilder,
            IXlsxFileDataTableBuilder XlsxFileDataTableBuilder,
            IStagingImportService StagingImportFactory,
            IDataProcessor DataProcessor,
            IUploadedFileService UploadDataInfoFactory,
            IDataValidationManager DataValidator,
            ITransactionContext transactionContext) :
        base(FileArchiver, FileAccess, FileArchiveService, CsvFileDataTableBuilder, XlsxFileDataTableBuilder, StagingImportFactory, DataProcessor, UploadDataInfoFactory, DataValidator, transactionContext)
    {
    }

    public virtual void Setup(int employerId, string sourceFilePath, int? planYearId = null)
    {
        employer Employer = this.ValidateEmployerId(employerId);      
        this.ValidatePlanYearId(employerId, planYearId);      

        base.Setup(sourceFilePath, Employer.ResourceId, employerId);

        this.DataValidator.AddValidator(new FeinDataValidator(Employer.EMPLOYER_EIN));
        this.EmployerId = employerId;
        this.PlanYearId = planYearId;
        this.OwnerId = employerId;
    }

    protected virtual employer ValidateEmployerId(int employerId)
    {

        if (employerId <= 0)
        {
            this.Log.Error(string.Format("EmployerId: [{0}] is not a valid Id.", employerId));

            throw new ArgumentException("Employer Id is not valid.");
        }

        employer employer = employerController.getEmployer(employerId);
        if (null == employer)
        {
            this.Log.Error(string.Format("EmployerId: [{0}] did not return an employer.", employerId));

            throw new ArgumentException("Employer is not valid.");
        }

        if (employer.EMPLOYER_EIN == null || employer.EMPLOYER_EIN == string.Empty || false == employer.EMPLOYER_EIN.IsValidFedId())
        {
            this.Log.Error(string.Format("FEIN: [{0}] is not a valid FEIN.", employer.EMPLOYER_EIN));

            throw new ArgumentException("Employer FEIN is not valid.");
        }

        return employer;

    }

    protected virtual void ValidatePlanYearId(int employerId, int? planYearId)
    {

        if (planYearId == null)
        {
            return;
        }

        if (planYearId <= 0)
        {
            this.Log.Error(string.Format("PlanYearId: [{0}] is not a valid Id.", employerId));

            throw new ArgumentException("PlanYearId Id is not valid.");
        }

        PlanYear planYear = PlanYear_Controller.getEmployerPlanYear(employerId).Find(year => year.PLAN_YEAR_ID == planYearId);
        if (null == planYear)
        {
            this.Log.Error(string.Format("PlanYearId: [{0}] did not return a PlanYear.", planYearId));

            throw new ArgumentException("PlanYear is not valid.");
        }
    }

    protected override void SaveUploadInfo()
    {

        if (this.uploadInfo == null)
        {
            Afas.ImportConverter.Domain.ImportFormatting.UploadedData.UploadedFileInfo uploadLoad = new Afas.ImportConverter.Domain.ImportFormatting.UploadedData.UploadedFileInfo
            {
                ArchiveFileInfo = null,
                FileTypeDescription = ((ImportFileMetaData)this.importData.MetaData).FileTypeDescription,
                FileName = ((ImportFileMetaData)this.importData.MetaData).SourceFilePath
            };
            this.uploadInfo = uploadLoad;
            this.uploadInfo.UploadOwnerId = this.EmployerId;
        }
        this.uploadInfo.UploadedByUser = this.UserId;
        this.uploadInfo.UploadSourceDescription = this.importData.MetaData.UploadSourceDescription;
        this.uploadInfo.UploadTime = DateTime.Now;
        this.uploadInfo.UploadTypeDescription = this.importData.MetaData.UploadTypeDescription;

        this.uploadInfo = this.UploadDataInfoService.AddNewEntity(this.uploadInfo, this.UserId);

        if (null == this.uploadInfo || null == this.uploadInfo.ResourceId || this.uploadInfo.ResourceId == new Guid())
        {
            this.Log.Error("Failed to Save Upload Data Info.");

            throw new ApplicationException("Database Access Issue, Please contact IT.");
        }

    }

    protected override DataTable ImportProcessedData()
    {
        DataTable RemainingData = null;

        switch (this.importData.MetaData.UploadTypeDescription)
        {
            case "Demographics":
                Afas.AfComply.UI.Code.AFcomply.DataUpload.DemographicDataTableImporter demImporter = Afas.AfComply.UI.Code.AFcomply.DataUpload.DependencyInjection.GetDemoImporter();
                RemainingData = demImporter.ImportProcessedData(this.importData.Data, this.EmployerId, this.UserId);
                this.DataValidationMessages = this.DataValidationMessages.Concat(demImporter.DataValidationMessages).ToList();

                break;
            case "Payroll":
                throw new NotImplementedException();
            case "Coverage":
                throw new NotImplementedException();
            case "Offer":
                Afas.AfComply.UI.Code.AFcomply.DataUpload.OfferDataTableImporter offImporter = Afas.AfComply.UI.Code.AFcomply.DataUpload.DependencyInjection.GetOfferImporter();

                if (this.PlanYearId == null)
                {
                    Afas.Domain.ValidationFailure failure = new Afas.Domain.ValidationFailure
                    {
                        Identifier = "Plan Year Id",
                        ValidationSeverity = Afas.Domain.ValidationSeverity.Dataset,
                        ValidationType = "PlanYear",
                        ValidationMessage = "A Plan Year was not provided, cannot import Offer file without a plan year."
                    };
                    this.DataValidationMessages.Add(failure);

                    RemainingData = this.importData.Data;
                    break;
                }

                RemainingData = offImporter.ImportProcessedData(this.importData.Data, this.EmployerId, (int)this.PlanYearId, this.UserId);
                this.DataValidationMessages = this.DataValidationMessages.Concat(offImporter.DataValidationMessages).ToList();

                break;

            case "Edits1095":

                Afas.AfComply.UI.Code.AFcomply.DataUpload.Edit1095TableImporter editImporter = Afas.AfComply.UI.Code.AFcomply.DataUpload.DependencyInjection.GetEdit1095Importer();

                if (this.PlanYearId == null)
                {
                    Afas.Domain.ValidationFailure failure = new Afas.Domain.ValidationFailure
                    {
                        Identifier = "Tax Year Id",
                        ValidationSeverity = Afas.Domain.ValidationSeverity.Dataset,
                        ValidationType = "TaxYear",
                        ValidationMessage = "A Tax Year was not provided, cannot import Edits file without a tax year."
                    };
                    this.DataValidationMessages.Add(failure);

                    RemainingData = this.importData.Data;
                    break;
                }

                RemainingData = editImporter.ImportProcessedData(this.importData.Data, this.EmployerId, (int)this.PlanYearId, this.UserId, this.TransactionContext);

                this.DataValidationMessages = this.DataValidationMessages.Concat(editImporter.DataValidationMessages).ToList();

                break;

        }

        return RemainingData;
    }

}