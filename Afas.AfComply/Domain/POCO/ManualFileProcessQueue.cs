using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.POCO
{
    /// <summary>
    /// A table to keep track of tiles that need to be manually processed.
    /// </summary>
    public class ManualFileProcessQueue : BaseAfasModel
    {
        /// <summary>
        /// Database PK for this object
        /// </summary>
        public int ManualFileProcessQueueId { get; set; }

        /// <summary>
        /// The Id of the file upload that start
        /// </summary>
        public int FileInfoId { get; set; }

        /// <summary>
        /// The Id of the staging Import that is waiting in the Queue
        /// </summary>
        public int StagingImportId { get; set; }

        /// <summary>
        /// The name of the user that has started working on this, or null.
        /// </summary>
        public string WorkStartedBy { get; set; }
    }
}
