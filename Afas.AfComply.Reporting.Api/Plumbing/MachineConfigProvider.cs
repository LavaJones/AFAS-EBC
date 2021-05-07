using Afas.Application.Archiver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Afas.AfComply.Reporting.Api.Plumbing
{
    /// <summary>
    /// This class is to provide specific config values to other modules
    /// </summary>
    public class MachineConfigProvider : IArchiveLocationProvider
    {
        string IArchiveLocationProvider.ArchiveFolderLocation
        {
            get
            {
                return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Archive.ArchiveFolder"]);
            }
        }
    }
}