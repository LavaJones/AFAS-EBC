/*
using Afas.AfComply.Domain;
using Afas.AfComply.Domain.POCO;
using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.UI.Code.AFcomply.DataUpload;
using Afas.Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

public class DataTableImportMethod
{
    private ILog Log = LogManager.GetLogger(typeof(DataTableImportMethod));

    private readonly int EmployerId;
    private readonly int? PlanYearId;
    private readonly string SourceFilePath;
    private readonly string UserId;
    private readonly string UploadSource;
    private readonly string UploadDataType;
    private readonly employer Employer;
    private readonly string FileType;
    private DataTable ParsedData;
    private int? UploadId;
    private int? StagingId;

    public DataTableImportMethod(int employerId, string sourceFilePath, string userId, string uploadSource, string uploadDataType, int? planYearId = null)
    {
        CheckFileExists(sourceFilePath);

        FileType = DependencyInjection.GetFileAccess().GetExtension(sourceFilePath).Trim().Trim('.').ToLower();
        if (FileType.IsNullOrEmpty())
        {
            throw new ArgumentException("Could not determine Uploaded File Type.");
        }

        Employer = ValidateEmployerId(employerId);      
        ValidatePlanYearId(employerId, planYearId);      

        if (userId.IsNullOrEmpty())
        {
            throw new ArgumentNullException("userId");
        }
        if (uploadSource.IsNullOrEmpty())
        {
            throw new ArgumentNullException("uploadSource");
        }
        if (uploadDataType.IsNullOrEmpty())
        {
            throw new ArgumentNullException("uploadDataType");
        }
         
        this.EmployerId = employerId;
        this.PlanYearId = planYearId;
        this.UserId = userId;
        this.UploadSource = uploadSource;
        this.UploadDataType = uploadDataType;
        this.SourceFilePath = sourceFilePath;
        this.ParsedData = null;
    }

    private void CheckFileExists(string sourceFilePath)
    {
        if (null == sourceFilePath || false == DependencyInjection.GetFileAccess().FileExists(sourceFilePath))
        {
            Log.Error("Tried to upload nonexistent File at path: " + sourceFilePath);

            throw new ArgumentException("Uploaded File Doesn't exist.");
        }
    }

    private employer ValidateEmployerId(int employerId)
    {
        if (employerId <= 0)
        {
            Log.Error(String.Format("EmployerId: [{0}] is not a valid Id.", employerId));

            throw new ArgumentException("Employer Id is not valid.");
        }

        employer employer = employerController.getEmployer(employerId);
        if (null == employer)
        {
            Log.Error(String.Format("EmployerId: [{0}] did not return an employer.", employerId));

            throw new ArgumentException("Employer is not valid.");
        }

        if (employer.EMPLOYER_EIN == null || employer.EMPLOYER_EIN == string.Empty || false == employer.EMPLOYER_EIN.IsValidFedId())
        {
            Log.Error(String.Format("FEIN: [{0}] is not a valid FEIN.", employer.EMPLOYER_EIN));

            throw new ArgumentException("Employer FEIN is not valid.");
        }

        return employer;
    }

    private void ValidatePlanYearId(int employerId, int? planYearId)
    {
        if (planYearId == null)
        {
            return;
        }

        if (planYearId <= 0)
        {
            Log.Error(String.Format("PlanYearId: [{0}] is not a valid Id.", employerId));

            throw new ArgumentException("PlanYearId Id is not valid.");

        }

        PlanYear planYear = PlanYear_Controller.getEmployerPlanYear(employerId).Find(year => year.PLAN_YEAR_ID == planYearId);
        if (null == planYear)
        {
            Log.Error(String.Format("PlanYearId: [{0}] did not return a PlanYear.", planYearId));

            throw new ArgumentException("PlanYear is not valid.");

        }        
    }

    public bool ImportFile()
    {
        CheckFileExists(SourceFilePath);

        SaveUploadInfo();

        try
        {
            ParseFile();

            if (false == FeinCheck())
            {
                return false;
            }

            SaveDataToStaging();

            ArchiveFile();

            CleanupAndPreProcessing();

            DataTable RemainingData = ImportProcessedData();
            RemainingData.TableName = "RemainingData";

            return UpdateDatabase(RemainingData);
        }
        catch (Exception ex)
        {
            Log.Error(
                string.Format(
                    "There was a problem importing file at [{0}], for Employer Id: [{1}], by User: [{2}], of Upload Source: [{3}] and Data Type: [{4}]",
                    SourceFilePath,
                    EmployerId,
                    UserId,
                    UploadSource,
                    UploadDataType),
                ex);

            UploadFileInfoFactory.UpdateUploadedFileInfoProcessedStatus((int)UploadId, UserId, false, true);

            return false;
        }
    }
    
    private void SaveUploadInfo() 
    {
        UploadId = UploadFileInfoFactory.SaveUploadedFileInfo(SourceFilePath, EmployerId, DateTime.Now, UploadSource, UploadDataType, FileType, UserId);
        if (null == UploadId || UploadId <= 0)
        {
            Log.Error("Failed to Save Upload File Info for File at path: " + SourceFilePath);

            throw new ApplicationException("Data Base Access Issue, Please contact IT.");
        }
    }

    private void ParseFile()
    {
        ParsedData = null;

        switch (FileType)
        {
            case "csv":
                ParsedData = DependencyInjection.GetCsvFileDataTableBuilder().CreateDataTableFromCsvFile(SourceFilePath);
                break;

            case "xls":
            case "xlsx":
                throw new NotImplementedException();
            case "tsv":
                throw new NotImplementedException();
            case "txt":
                throw new NotImplementedException();
            default:
                throw new NotImplementedException();
        }

        if (null == ParsedData)
        {
            Log.Error(String.Format("Data Table Parsing Failed for File at [{0}]", SourceFilePath));

            throw new ApplicationException("Failed to parse File Data.");
        }
        else if (ParsedData.Columns.Count <= 0 || ParsedData.Rows.Count <= 0)
        {
            Log.Error(String.Format(
                "Data Table Parsing returned empty Table. Columns Count: [{0}], Rows Count: [{1}], User Id: [{2}] File source: [{3}]",
                ParsedData.Columns.Count,
                ParsedData.Rows.Count,
                UserId,
                SourceFilePath));

            throw new ApplicationException("Parsed File Data was Empty.");
        }

        /// Set the title (nessisary to create xml)
        ParsedData.TableName = "Import";
    }

    private bool FeinCheck()
    {
        try
        {
            if (false == ParsedData.VerifyContainsOnlyThisFederalIdentificationNumber(Employer.EMPLOYER_EIN))
            {
                Log.Error(String.Format(
                    "Uploaded File did not contain (Co)FEIN Column. UploadId: [{0}], User: [{1}]",
                    UploadId,
                    UserId));

                UploadFileInfoFactory.UpdateUploadedFileInfoProcessedStatus((int)UploadId, UserId, false, true);

                return false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(String.Format(
                    "Uploaded File contains multiple or wrong (Co)FEIN. Expected EIN: [{0}] UploadId: [{1}] User: [{2}]",
                    Employer.EMPLOYER_EIN,
                    UploadId,
                    UserId), ex);

            UploadFileInfoFactory.UpdateUploadedFileInfoProcessedStatus((int)UploadId, UserId, false, true);

            return false;
        }

        return true;
    }

    private void SaveDataToStaging() 
    {
        StagingId = StagingImportFactory.SaveDataTableToStagingImport(ParsedData, null, UserId, (int)UploadId);
        if (null == StagingId || StagingId <= 0)
        {
            Log.Error(String.Format(
                "Failed to Stage DataTable. StagingId: [{0}], UploadId: [{1}], User: [{2}]",
                StagingId,
                UploadId,
                UserId));

            throw new ApplicationException("Failed to Save Data to Staging.");
        }
    }

    private void ArchiveFile()
    {
        int ArchiveId = new FileArchiverWrapper().ArchiveFile(SourceFilePath, Employer.ResourceId, "Archive By New Importer", );
        if (ArchiveId <= 0)
        {
            Log.Error(String.Format(
                "Failed to ArchiveFile. ArchiveId: [{0}], User: [{1}]",
                ArchiveId,
                UserId));

            throw new ApplicationException("Archival Issue, Please contact IT.");
        }

        if (false == UploadFileInfoFactory.UpdateUploadedFileInfoArchived((int)UploadId, UserId, ArchiveId))
        {
            Log.Error(String.Format(
                "Failed to update Upload Info. UploadId: [{0}], ArchiveId: [{1}], User: [{2}]",
                UploadId,
                ArchiveId,
                UserId));

            throw new ApplicationException("Data Base Access Issue, Please contact IT.");
        }
    }
    
    private void CleanupAndPreProcessing()
    {
        ParsedData.CleanDataTable();

        StagingImportFactory.UpdateModifiedStagingImport((int)StagingId, UserId, ParsedData);













        




        StagingId = StagingImportFactory.UpdateStagingImportAfterReprocessing((int)StagingId, UserId, ParsedData);
    }

    private DataTable ImportProcessedData() 
    {
        DataTable RemainingData = null;

        switch (UploadDataType)
        {
            case "Demographics":
                RemainingData = DependencyInjection.GetDemoImporter().ImportProcessedData(ParsedData, EmployerId, UserId);
                break;
            case "Payroll":
                throw new NotImplementedException();
            case "Coverage":
                throw new NotImplementedException();
            case "Offer":
                RemainingData = DependencyInjection.GetOfferImporter().ImportProcessedData(ParsedData, EmployerId, (int)PlanYearId, UserId);
                break;

        }

        return RemainingData;
    }

    private bool UpdateDatabase(DataTable RemainingData)
    {
        if (null != RemainingData && null != RemainingData.Rows)
        {
            if (RemainingData.Rows.Count <= 0)
            {
                StagingImportFactory.UpdateStagingImportEntityStatus((int)StagingId, UserId, 2);

                UploadFileInfoFactory.UpdateUploadedFileInfoProcessedStatus((int)UploadId, UserId, true);

                return true;
            }
            else
            {
                Log.Warn("File not completly uploaded, there were [" + RemainingData.Rows.Count + "] unprocessed rows.");

                StagingId = StagingImportFactory.UpdateStagingImportAfterReprocessing((int)StagingId, UserId, RemainingData);

                UploadFileInfoFactory.UpdateUploadedFileInfoProcessedStatus((int)UploadId, UserId, false, true);

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
*/
