using System;

namespace Afas.Application.Archiver
{

    public interface IArchiveFileInfoAccess
    {
        /// <summary>
        /// Save a new Archived File Info Object to Storage
        /// </summary>
        /// <param name="employerGuid">The Guid Used as a Folder</param>
        /// <param name="FileName">The Filename portion of the orrigonal file path.</param>
        /// <param name="SourceFilePath">The Source Files Path minus the File Name.</param>
        /// <param name="ArchiveFilePath">The Resulting Archive Path.</param>
        /// <param name="userId">The Id of the entity that archived the files.</param>
        /// <param name="reason">Text Describing why the File was archived</param>
        /// <returns>0 if the Save Failed, > 0 if it succeded in saving.</returns>
        int SaveArchiveFileInfo(
            Guid employerGuid,
            int employerId,
            string FileName,
            string SourceFilePath,
            string ArchiveFilePath,
            string userId, 
            string reason);
        
    }

}
