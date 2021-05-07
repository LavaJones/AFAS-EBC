
namespace Afas.Application.FileAccess
{
    /// <summary>
    /// Interface for hiding direct File Access. It wraps Methods from System.IO. ex: Path, Directory, File 
    /// </summary>
    public interface IFileAccess
    {
        /// <summary>
        /// Checks if the file at the given path exists.
        /// </summary>
        /// <param name="path">The path of the file</param>
        /// <returns>True if the file Exists, fals if it doesn't</returns>
        bool FileExists(string path);

        /// <summary>
        /// Moves a file from the source to the destination and changing the name if specified.
        /// </summary>
        /// <param name="from">The path to the source file</param>
        /// <param name="to">The destination and file name.</param>
        void Move(string from, string to);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="path">The path of the file to be deleted.</param>
        void DeleteFile(string path);

        /// <summary>
        /// Checks if the direcotry at the given path exists.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>True if the directory Exists, fals if it doesn't</returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Creates all non existent Directories in the supplied path.
        /// </summary>
        /// <param name="path">The path of the directory to be created.</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Returns the filename and extension of the specified file.
        /// </summary>
        /// <param name="path">The path to extract the file name from.</param>
        /// <returns>The file name and extension.</returns>
        string GetFileName(string path);

        /// <summary>
        /// Gets the Files Extension only
        /// </summary>
        /// <param name="path">The File to get teh extension from.</param>
        /// <returns>The File Extension</returns>
        string GetExtension(string path);

        /// <summary>
        /// Gets the Directory path without the filename or extension.
        /// </summary>
        /// <param name="path">The path to extract the directory path from.</param>
        /// <returns>The Directory path without the filename or extension.</returns>
        string GetDirectoryName(string path);
    }
}
