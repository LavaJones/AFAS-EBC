using System;

using log4net;
using Afas.Application.FileAccess;
using Afas.Application.Services;
using Afc.Core.Application;

namespace Afas.Application.Archiver
{

    /// <summary>
    /// This Class manages the functionality required to move a file into the archive.
    /// </summary>
    public class FileArchiver : IFileArchiver
    {
        /// <summary>
        /// Standard logger
        /// </summary>
        private ILog Log = LogManager.GetLogger(typeof(FileArchiver));


        /// <summary>
        /// The Path of the main Archive Folder
        /// </summary>
        private string archiveFolder;

        /// <summary>
        /// Wraps access to System.IO to enable our moving files about
        /// </summary>
        private IFileAccess fileAccess;

        /// <summary>
        /// Access to storage of the Archive File Info
        /// </summary>
        private IArchiveFileInfoAccess ArchiveFileInfoAccess;

        /// <summary>
        /// A Class to simplify the File Access required to archive files
        /// </summary>
        /// <param name="archiveFolder">The Base folder that the files are moved to</param>
        public FileArchiver(IArchiveLocationProvider folderProvider, IFileAccess fileAccess, IArchiveFileInfoAccess archiveFileInfoAccess, ITransactionContext transactionContext = null) 
        {
            //check that the folder path is not null or empty
            if (null == folderProvider || null == folderProvider.ArchiveFolderLocation || String.Empty == folderProvider.ArchiveFolderLocation) 
            {

                Log.Info("Dependency archiveFolder was not defined");
                throw new ArgumentNullException("folderProvider");
            }

            String archiveFolder = folderProvider.ArchiveFolderLocation;

            //checks to see if the archive folder path ends with a slash, if not it adds one
            if (false == archiveFolder.EndsWith("\\") && false == archiveFolder.EndsWith("/"))
            {
                //the folder path does not end with a slash, add one.
                archiveFolder += "\\";
            }
            this.archiveFolder = archiveFolder;

            //check that file access is actually an object
            if (null == fileAccess) 
            {
                throw new ArgumentNullException("fileAccess");
            }
            this.fileAccess = fileAccess;

            //check that archive File Info Access is actually an object
            if (null == archiveFileInfoAccess) 
            {
                throw new ArgumentNullException("archiveFileInfoAccess");
            }
            this.ArchiveFileInfoAccess = archiveFileInfoAccess;
            if (this.ArchiveFileInfoAccess is IArchiveFileInfoService && null != transactionContext)
            {
                ((IArchiveFileInfoService)this.ArchiveFileInfoAccess).Context = transactionContext;
            }            
        }

        /// <summary>
        /// Archive the specific file for the specific employer.
        /// </summary>
        /// <param name="filePath">The path to the File to be archived.</param>
        /// <param name="employerGuid">The Id of the employer that owns the file.</param>
        /// <param name="reason">Text Describing why the file was archived.</param>
        /// <returns>The Id of the Archive Info object</returns>
        public int ArchiveFile(String filePath, Guid employerGuid, string reason, int employerId)
        {
            //check for invalid arguments
            if(null == filePath || String.Empty == filePath)
            {
                Log.Info("ArchiveFile was passed an unset file path:"+filePath);
                throw new ArgumentNullException("filePath");
            }

            //GUID must be defined other than new(empty) GUID
            if(new Guid().Equals(employerGuid))
            {
                Log.Info("ArchiveFile was passed an uninitailized employer Guid:" + employerGuid);
                throw new ArgumentException("Employer Guid must be initailized.");
            }

            //If the source file must exist
            if (false == fileAccess.FileExists(filePath))
            {
                Log.Info("ArchiveFile was passed a File Path that did not eist:"+ filePath);
                throw new ArgumentException("File to move must exist.");
            }

            //create the path for the archived file using pattern : ArchiveFolder\Employer_GUID\newGUID.extension            
            String archivePath = String.Format("{0}{1}\\{2}{3}", archiveFolder, employerGuid, Guid.NewGuid(), fileAccess.GetExtension(filePath));

            //check that the file does not already exist for this archive destination
            if (fileAccess.FileExists(archivePath))
            {
                // This shouldn't happen (I hope)
                Log.Error("ArchiveFile Created an Archive File Path that conflicts with ant existing File:" + archivePath);
                throw new Exception("A file with that path already exists in the Archive: " + archivePath);
            }

            //if the directory does not exist yet, create it.
            if (false == fileAccess.DirectoryExists(fileAccess.GetDirectoryName(archivePath))) 
            {
                Log.Info(string.Format("Archive Folder doesnot exist for GUID {0}, creating Archive Path {1}", employerGuid, archivePath));
                fileAccess.CreateDirectory(fileAccess.GetDirectoryName(archivePath));
            }

            //Save the Archive Files information, if we cannot save it then don't move the file
            int saveResultId = this.ArchiveFileInfoAccess.SaveArchiveFileInfo(
                employerGuid,
                employerId,
                fileAccess.GetFileName(filePath),
                fileAccess.GetDirectoryName(filePath),
                archivePath,
                "Archiver",
                reason);

            if (saveResultId > 0)
            {
                //Move the file
                fileAccess.Move(filePath, archivePath);

                //if the new file exists, then delete the source file
                if (fileAccess.FileExists(archivePath))
                {
                    fileAccess.DeleteFile(filePath);
                }

                return saveResultId;
            }

            return 0;
        }
    }
}