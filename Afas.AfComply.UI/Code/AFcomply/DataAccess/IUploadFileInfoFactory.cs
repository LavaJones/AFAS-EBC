using System;

namespace Afas.AfComply.UI.Code.AFcomply.DataAccess
{
    public interface IUploadFileInfoFactory
    {
        bool DeltereUploadedFileInfo(int UploadedFileInfoId, string userName);

        System.Collections.Generic.List<Afas.AfComply.Domain.POCO.UploadedFileInfo> GetAllUnprocessedUploadedFileInfo();
                
        System.Collections.Generic.List<int> GetAllEmployerIdsUnprocessedUploadedFileInfo();

        System.Collections.Generic.List<Afas.AfComply.Domain.POCO.UploadedFileInfo> GetAllFailedProcessingUploadedFileInfo();

        System.Collections.Generic.List<Afas.AfComply.Domain.POCO.UploadedFileInfo> GetAllUnprocessedUploadedFilesForEmployerId(int employerId);
        
        System.Collections.Generic.List<Afas.AfComply.Domain.POCO.UploadedFileInfo> GetAllUploadedFilesForEmployerId(int employerId);
        
        Afas.AfComply.Domain.POCO.UploadedFileInfo GetUploadedFileInfoById(int UploadedFileInfoId);
        
        int SaveUploadedFileInfo(string sourceFileName, int employerId, DateTime uploadTime, string uploadSourceDescription, string uploadTypeDescription, string fileTypeDescription, string userId);
        
        bool UpdateUploadedFileInfoEntityStatus(int UploadedFileInfoId, string userName, int newStatus);
        
        bool UpdateUploadedFileInfoProcessedStatus(int UploadedFileInfoId, string userName, bool Processed, bool Failed = false);

        bool UpdateUploadedFileInfoArchived(int UploadedFileInfoId, string userName, int archiveId);
    }
}
