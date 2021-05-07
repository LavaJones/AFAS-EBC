using System;
using System.Linq;


namespace Afas.AfComply.Reporting.Domain.FileCabinet
{
    public static class FileCabinetFolderInfoQuerableExtensionMethod
    {
        public static IQueryable<FileCabinetFolderInfo> FilterForFolderResourceID(this IQueryable<FileCabinetFolderInfo> FileCabinetFolderInfos, Guid ResourceID)
        {
            return (
                from FileCabinetFolderInformation in FileCabinetFolderInfos
                where FileCabinetFolderInformation.ResourceId == ResourceID
                select FileCabinetFolderInformation
                );
        }

        public static IQueryable<FileCabinetFolderInfo> FilterFor1095TaxYear(this IQueryable<FileCabinetFolderInfo> FileCabinetFolderInfos, int TaxYear)
        {
            string TaxYearText = TaxYear.ToString();

            return (
                from 
                    FileCabinetFolderInformation in FileCabinetFolderInfos
                where 
                    FileCabinetFolderInformation.FolderName == "1095" 
                select 
                    FileCabinetFolderInformation
                        .children
                        .Where(Folder=> Folder.FolderName == TaxYearText)
                        .FirstOrDefault()
                );
        }
    }
}
