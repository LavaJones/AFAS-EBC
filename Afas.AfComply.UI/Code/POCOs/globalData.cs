using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for globalData
/// </summary>
public static class globalData
{

    public static string getVersion()
    {
        return System.Configuration.ConfigurationManager.AppSettings["version"];
    }
}