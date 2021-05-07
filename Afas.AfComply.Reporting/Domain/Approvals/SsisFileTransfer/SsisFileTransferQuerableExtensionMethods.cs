using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer;

namespace Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer
{
  public static  class SsisFileTransferQuerableExtensionMethods
 {

        public static IQueryable<SsisFileTransfer> FilterForFileName(this IQueryable<SsisFileTransfer> SsisFileTransfers, string FileName)
        {
            return (
                   from SsisFileTransfer in SsisFileTransfers
                   where SsisFileTransfer.FileName == FileName
                   select SsisFileTransfer
                  );

        }

        public static IQueryable<SsisFileTransfer> FilterForFEIN(this IQueryable<SsisFileTransfer> SsisFileTransfers, String FEIN)
        {
            return (
                   from SsisFileTransfer in SsisFileTransfers
                   where SsisFileTransfer.FEIN == FEIN
                   select SsisFileTransfer
                   );
        }
        public static IQueryable<SsisFileTransfer> FilterForStartRunTime(this IQueryable<SsisFileTransfer> SsisFileTransfers, DateTime RunTime)
        {
            return (
                 from SsisFileTransfer in SsisFileTransfers
                 where SsisFileTransfer.RunTime >= RunTime
                 select SsisFileTransfer
                   );
        }

        public static IQueryable<SsisFileTransfer> FilterForEndRunTime(this IQueryable<SsisFileTransfer> SsisFileTransfers, DateTime RunTime)
        {
            return (
                 from SsisFileTransfer in SsisFileTransfers
                 where SsisFileTransfer.RunTime <= RunTime
                 select SsisFileTransfer
                   );
        }


    }
}
