using System.IO;

namespace Afas.Application.FileAccess
{
    /// <summary>
    /// Wrapper class for System.IO features
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]

    public class FileAccess : IFileAccess
    {
        /// <summary>
        /// Checks if the file at the given path exists.
        /// </summary>
        /// <param name="path">The Full Path of the file</param>
        /// <returns>True if the file Exists, fals if it doesn't</returns>
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Moves a file from the source to the destination and changing the name if specified.
        /// </summary>
        /// <param name="from">The path to the source file</param>
        /// <param name="to">The destination and file name.</param>
        public void Move(string from, string to)
        {
            File.Move(from, to);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="path">The path of the file to be deleted.</param>
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// Checks if the direcotry at the given path exists.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>True if the directory Exists, fals if it doesn't</returns>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Creates all non existent Directories in the supplied path.
        /// </summary>
        /// <param name="path">The path of the directory to be created.</param>
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Returns the filename and extension of the specified file.
        /// </summary>
        /// <param name="path">The path to extract hte file name from.</param>
        /// <returns>The file name and extension.</returns>
        public string GetFileName(string path) 
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Gets the Files Extension only
        /// </summary>
        /// <param name="path">The File to get teh extension from.</param>
        /// <returns>The File Extension</returns>
        public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        /// <summary>
        /// Gets the Directory path without the filename or extension.
        /// </summary>
        /// <param name="path">The path to extract the directory path from.</param>
        /// <returns>The Directory path without the filename or extension.</returns>
        public string GetDirectoryName(string path) 
        {
            return Path.GetDirectoryName(path);
        }
    }
}