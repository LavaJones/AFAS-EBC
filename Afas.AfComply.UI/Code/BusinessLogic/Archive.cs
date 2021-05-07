using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

// Missing namespace is intentional.

/// <summary>
/// Helper class to handle all Archive deployment variables.
/// </summary>
public static class Archive
{
    /// <summary>
    /// The folder where all archived files are moved to.
    /// </summary>
    public static String ArchiveFolder
    {
        get
        {
            return ConfigurationManager.AppSettings["Archive.ArchiveFolder"];
        }
    }

}
