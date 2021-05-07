using Afas.AfComply.Reporting.Domain.FileCabinet;
using Afc.Core.Domain;
using System;
using System.Linq;


namespace Afas.AfComply.Reporting.Domain.Approvals.FileCabinet
{
    public static class FileCabinetInfoQuerableExtensionMethods
    {
        public static IQueryable<FileCabinetInfo> FilterForCreatedBy(this IQueryable<FileCabinetInfo> FileCabinetInfos, string Name)
        {
            return (
                from FileCabinetInformation in FileCabinetInfos
                where FileCabinetInformation.CreatedBy == Name
                select FileCabinetInformation
                );
        }
        public static IQueryable<FileCabinetInfo> FilterForFile(this IQueryable<FileCabinetInfo> FileCabinetInfos, string FileName)
        {
            return (
                from FileCabinetInformation in FileCabinetInfos
                where FileCabinetInformation.Filename == FileName
                select FileCabinetInformation
                  );
        }

        public static IQueryable<FileCabinetInfo> FilterForFolder(this IQueryable<FileCabinetInfo> FileCabinetInfos, int FolderId)
        {
            return (
                from FileCabinetInformation in FileCabinetInfos
                where FileCabinetInformation.ID == FolderId
                select FileCabinetInformation
                );

        }

        public static IQueryable<FileCabinetInfo> FilterForEmployeeResourceId(this IQueryable<FileCabinetInfo> FileCabinetInfos, Guid otherResourceId, long FileCabinetFolderInfo_ID)
        {
            return (
                from FileCabinetInformation in FileCabinetInfos
                where FileCabinetInformation.OtherResourceId == otherResourceId && FileCabinetInformation.FileCabinetFolderInfo.ID == FileCabinetFolderInfo_ID
                select FileCabinetInformation
                );
        }

        public static IQueryable<FileCabinetInfo> FilterForFolderResourceID(this IQueryable<FileCabinetInfo> FileCabinetInfos, Guid ResourceID, Guid OwnerResourceId)
        {
            return (
                from FileCabinetInformation in FileCabinetInfos
                where FileCabinetInformation.FileCabinetFolderInfo.ResourceId == ResourceID && FileCabinetInformation.OwnerResourceId == OwnerResourceId
                orderby FileCabinetInformation.Filename
                select FileCabinetInformation);
        }
    }
}
