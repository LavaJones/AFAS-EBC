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
public class ReportingFileDataImporter : AfComplyFileDataImporter
{
    private ILog Log = LogManager.GetLogger(typeof(AfComplyFileDataImporter));

    public ReportingFileDataImporter(
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

    public override void Setup(int employerId, string sourceFilePath, int? taxYearId = null)
    {
        employer Employer = ValidateEmployerId(employerId);      
        ValidateTaxYearId(employerId, taxYearId);      

        base.Setup(sourceFilePath, Employer.ResourceId, employerId);
        
        DataValidator.AddValidator(new FeinDataValidator(Employer.EMPLOYER_EIN));        
        this.EmployerId = employerId;
        this.PlanYearId = taxYearId;         
        this.OwnerId = employerId;
    }
    

    private void ValidateTaxYearId(int employerId, int? taxYearId)
    {

        if (taxYearId == null)
        {
            throw new ArgumentException("TaxYearId Id is required.");
        }

        if (taxYearId <= 0)
        {
            Log.Error(String.Format("TaxYearId: [{0}] is not a valid Id.", employerId));

            throw new ArgumentException("TaxYearId Id is not valid.");
        }

        if (taxYearId < 2015 || taxYearId > 2025)
        {
            Log.Error(String.Format("TaxYearId: [{0}] was not reasonable.", taxYearId));

            throw new ArgumentException("TaxYearId is not valid.");
        }
    }    
        
}