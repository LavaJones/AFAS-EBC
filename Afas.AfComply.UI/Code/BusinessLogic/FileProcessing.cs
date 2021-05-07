using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using log4net;
using Afas.AfComply.Reporting.Core.Models;
using System.IO.Compression;

/// <summary>
/// Summary description for FileProcessing
/// </summary>
public static class FileProcessing
{
    private static ILog Log = LogManager.GetLogger(typeof(FileProcessing));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_fu">File Upload Object</param>
    /// <param name="_savePath">C:\\folder\\folder\\</param>
    /// <param name="lblMessage">Label Object</param>
    public static void SaveFile(FileUpload _fu, string _savePath, Label lblMessage)
    {
        string fileName = _fu.FileName;

        string pathToCheck = _savePath + fileName;

        string tempfileName = "";

        if (System.IO.File.Exists(pathToCheck)) 
        {
          int counter = 2;
          while (System.IO.File.Exists(pathToCheck))
          {
            tempfileName = counter.ToString() + fileName;
            pathToCheck = _savePath + tempfileName;
            counter ++;
          }

          fileName = tempfileName;

          lblMessage.Text = "A file with the same name already exists." + 
              "<br />Your file was saved as " + fileName;
        }
        else
        {
          lblMessage.Text = "Your file was uploaded successfully.";
        }

        _savePath += fileName;

        _fu.SaveAs(_savePath);

        PIILogger.LogPII(string.Format("Uploaded File [{0}] to [{1}]",
            _fu.FileName, _savePath));

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_fu">File Upload Object</param>
    /// <param name="_savePath">C:\\folder\\folder\\</param>
    /// <param name="lblMessage">Label Object</param>
    public static void SaveFile(FileUpload _fu, String _savePath, Label lblMessage, String _fileName, out String savedFileName)
    {
        string fileName = _fileName + "_" + _fu.FileName;

        string pathToCheck = _savePath + fileName;

        string tempfileName = "";

        if (System.IO.File.Exists(pathToCheck))
        {
            int counter = 2;
            while (System.IO.File.Exists(pathToCheck))
            {
                tempfileName = Path.GetFileNameWithoutExtension(fileName) + '('+ counter.ToString() + ')' + Path.GetExtension(fileName);
                pathToCheck = _savePath + tempfileName;
                counter++;
            }

            fileName = tempfileName;

            lblMessage.Text = "A file with the same name already exists." +
                "<br />Your file was saved as " + fileName;
        }
        else
        {
            lblMessage.Text = "Your file was uploaded successfully.";
        }

        _savePath += fileName;

        _fu.SaveAs(_savePath);

        PIILogger.LogPII(
                String.Format("Uploaded File [{0}] to [{1}]", _fu.FileName, _savePath)
            );

        savedFileName = _savePath;

    }

    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> getFtpFiles(string _find)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps"));
        List<FileInfo> ftpFileList = directory.GetFiles().ToList<FileInfo>();
        List<FileInfo> filteredList = new List<FileInfo>();

        try
        {
            _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {
                string fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;
    }

    public static IList<FileInfo> GetAllFtpRawFiles()
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/rawdata"));
        return directory.GetFiles().ToList<FileInfo>();

    }



    public static IList<FileInfo> GetAllFilesByFolderName(string folderName)
    {

        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/"+ folderName));

        return directory.GetFiles().ToList<FileInfo>();

    }

    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> getFtpRawFiles(string _find)
    {

        List<FileInfo> ftpFileList = GetAllFtpRawFiles() as List<FileInfo>;
        List<FileInfo> filteredList = new List<FileInfo>();

        try
        {

                _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {
                string fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }
            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;
    }

    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> getFtpGpFiles(string _find)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/grosspay"));
        List<FileInfo> ftpFileList = directory.GetFiles().ToList<FileInfo>();
        List<FileInfo> filteredList = new List<FileInfo>();
        try
        {
            _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {
                string fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;
    }

    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> getFtpHrFiles(string _find)
    {
        List<FileInfo> filteredList = new List<FileInfo>();
        List<FileInfo> ftpFileList;
        try
        {
            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/hrstatus"));
            ftpFileList = directory.GetFiles().ToList<FileInfo>();
        }
        catch (Exception exception)
        {
            Log.Warn("Folder ~/ftps/hrstatus or it's files could not be accessed, or threw an exception: ", exception);
            return filteredList;
        }

        try
        {
            _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {
                string fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;
    }


    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> getFtpAckFiles(string _find)
    {
        List<FileInfo> filteredList = new List<FileInfo>();
        List<FileInfo> ftpFileList;
        try
        {
            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/submission_ack"));
            ftpFileList = directory.GetFiles().ToList<FileInfo>();
        }
        catch (Exception exception)
        {
            Log.Warn("Folder ~/ftps/submission_ack or it's files could not be accessed, or threw an exception: ", exception);
            return filteredList;
        }

        try
        {
            _find = _find.Trim().ToLower();
            foreach (FileInfo fi in ftpFileList)
            {
                string fname2 = fi.Name.Trim().ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;
    }

    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> GetFtpInsuranceChangeFiles(String _find)
    {

        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/InsuranceChangeEvent"));
        List<FileInfo> ftpFileList = directory.GetFiles().ToList<FileInfo>();
        List<FileInfo> filteredList = new List<FileInfo>();

        try
        {

            _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {

                String fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }

            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;

    }

    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> GetFtpInsuranceDiscrepancyFiles(String _find)
    {

        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/InsuranceDiscrepancy"));
        List<FileInfo> ftpFileList = directory.GetFiles().ToList<FileInfo>();
        List<FileInfo> filteredList = new List<FileInfo>();

        try
        {
        
            _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {
            
                String fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }

            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;
    
    }

    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> GetFtpInsuranceFiles(String _find)
    {

        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/insoffer"));
        List<FileInfo> ftpFileList = directory.GetFiles().ToList<FileInfo>();
        List<FileInfo> filteredList = new List<FileInfo>();

        try
        {

            _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {

                String fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }

            }

        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;

    }

    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> getFtpInsuranceCarrierFiles(string _find)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/inscarrier"));
        List<FileInfo> ftpFileList = directory.GetFiles().ToList<FileInfo>();
        List<FileInfo> filteredList = new List<FileInfo>();

        try
        {
            _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {
                string fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;
    }

    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> getFtpPayModFiles(string _find)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/paymod"));
        List<FileInfo> ftpFileList = directory.GetFiles().ToList<FileInfo>();
        List<FileInfo> filteredList = new List<FileInfo>();

        try
        {
            _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {
                string fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;
    }


    /// <summary>
    /// This function will return a list of FileInfo objects for a specific employer and file type. 
    /// </summary>
    /// <param name="_find">This is the DEM_xxxx or PAY_xxxx</param>
    /// <returns></returns>
    public static List<FileInfo> getFtpEcFiles(string _find)
    {
        DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/ftps/eclass"));
        List<FileInfo> ftpFileList = directory.GetFiles().ToList<FileInfo>();
        List<FileInfo> filteredList = new List<FileInfo>();

        try
        {
            _find = _find.ToLower();
            foreach (FileInfo fi in ftpFileList)
            {
                string fname2 = fi.Name.ToLower();

                if (fname2.Contains(_find))
                {
                    filteredList.Add(fi);
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
        }

        return filteredList;
    }

    public static MemoryStream ZipAllFilesInTheSpecifiedDirectoryPath(string Path, string ZippedFolderName)
    {
        var outputStream = new MemoryStream();
        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
        {

            zip.AddDirectory(Path);
            zip.Save(outputStream);
            return outputStream;
        }

    }


    /// <summary>
    /// Forcefully Replacing Existing Files during Extracting Files.
    /// </summary>
    /// <param name="archive"></param>
    /// <param name="destinationDirectoryName"></param>
    /// <param name="overwrite">If the file already exists in the destination folder then this is optional to overwrite the exiting file or ignore</param> 
    public static void ExtractToDirectory(ZipArchive archive, string destinationDirectoryName, bool overwrite)
    {
        if (!overwrite)
        {
            archive.ExtractToDirectory(destinationDirectoryName);
            return;
        }
        foreach (ZipArchiveEntry file in archive.Entries)
        {
            string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
            if (file.Name == "")
            {    
                Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                continue;
            }
            file.ExtractToFile(completeFileName, true);

        }
        archive.Dispose();
    }


}