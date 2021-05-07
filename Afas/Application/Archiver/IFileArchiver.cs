using System;

namespace Afas.Application.Archiver
{

    /// <summary>
    /// This interface defines the interaction of a file archiver
    /// </summary>
    public interface IFileArchiver
    {
        /// <summary>
        /// Archive the specific file for the specific employer.
        /// </summary>
        /// <param name="filePath">The path to the File to be archived.</param>
        /// <param name="employerGuid">The Id of the employer that owns the file.</param>
        /// <param name="reason">Text Describing why the file was archived.</param>
        /// <returns>The Id of the Archive Info object</returns>
        int ArchiveFile(string filePath, Guid employerGuid, string reason, int employerId);
    }

}
