using System;

namespace Afas.AfComply.UI.Code.AFcomply.DataAccess
{
    public interface IStagingImportFactory
    {
        bool DeleteStagingImport(int stagingId, string userName);

        System.Collections.Generic.List<Afas.AfComply.Domain.POCO.StagingImport> GetAllActiveStagingImport();

        System.Collections.Generic.List<Afas.AfComply.Domain.POCO.StagingImport> GetAllActiveStagingImportForUpload(int employerId);
        
        System.Collections.Generic.List<Afas.AfComply.Domain.POCO.StagingImport> GetAllStagingImportForUpload(int employerId);

        Afas.AfComply.Domain.POCO.StagingImport GetStagingImportById(int StagingImportId);

        int SaveDataTableToStagingImport(System.Data.DataTable originalDataTable, System.Data.DataTable modifiedDataTable, string userId, int uploadedFileInfoId);

        bool UpdateModifiedStagingImport(int stagingId, string userName, System.Data.DataTable modifiedDataTable);

        int UpdateStagingImportAfterReprocessing(int stagingId, string userName, System.Data.DataTable ReprocessedDataTable);

        bool UpdateStagingImportEntityStatus(int stagingId, string userName, int newStatus);
    }
}
