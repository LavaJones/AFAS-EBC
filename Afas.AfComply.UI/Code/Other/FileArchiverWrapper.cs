using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using log4net;

using Afas.AfComply.Application;

using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Application.Archiver;
using Afas.Application.FileAccess;
using Afas.AfComply.UI.Plumbing;

/// <summary>
/// This Class manages the functionality required to move a file into the archive.
/// </summary>
[Obsolete("This was created as a Temporary fix to simplify conversion to DI")]
    public class FileArchiverWrapper : IFileArchiver
    {
        /// <summary>
        /// Standard logger
        /// </summary>
        private ILog Log;

        /// <summary>
        /// Wraps THe DI version of Archiver
        /// </summary>
        private FileArchiver archiver;

        /// <summary>
        /// A Clas to simplify the File Access required to archive files
        /// </summary>
        /// <param name="archiveFolder">The Base folder that the files are moved to</param>
        public FileArchiverWrapper() 
        {
            Log = LogManager.GetLogger(typeof(FileArchiverWrapper));
            FileAccess fileAccess = new FileAccess();                    
            IArchiveFileInfoAccess archiveFileInfoAccess = new ArchiveFileInfoFactory();
            archiver = new FileArchiver( new MachineConfigProvider(), fileAccess, archiveFileInfoAccess);
        }

        [Obsolete("This was created as a Temporary fix to simplify conversion to archiving")]
        public int ArchiveFile(string filePath, int employerId, string reason)
        {
            PIILogger.LogPII(string.Format("Archiving File for employer [{0}] from File Path [{1}]", employerId, filePath));

   
            Log.Info("Using Obsolete method ArchiveFile, plan on updating this code.");
            return ArchiveFile(filePath, employerController.getEmployer(employerId).ResourceId, reason, employerId);
        }

        /// <summary>
        /// Moves a File to the Archive
        /// </summary>
        /// <param name="filePath">The path to the File to move.</param>
        /// <param name="employerGuid">The Employer that this file belongs to.</param>
        /// <param name="reason">Text that describes why the file was deleted</param>
        public int ArchiveFile(string filePath, Guid employerGuid, string reason, int employerId)
        {
            return archiver.ArchiveFile(filePath, employerGuid, reason, employerId);
        }
    }
