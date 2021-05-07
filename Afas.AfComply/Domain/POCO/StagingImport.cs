using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.POCO
{
    /// <summary>
    /// This object is used to store import data as it is being proceesed and cleaned in multiple steps.
    /// </summary>
    public class StagingImport : BaseAfasModel
    {
        /// <summary>
        /// The DataBase PK
        /// </summary>
        public int StagingImportId { get; set; }

        /// <summary>
        /// A Data Table containing the orrigonal Data
        /// </summary>
        public DataTable Original { get; set; } 

        /// <summary>
        /// A Data Table containing the Modified Data
        /// </summary>
        public DataTable Modified { get; set; } 

        /// <summary>
        /// The Meta data of the File that this data came from 
        /// </summary>
        public UploadedFileInfo FileInfo { get; set; }

        /// <summary>
        /// The total number of rows in the current Data
        /// </summary>
        public int RowCount
        {
            get
            {
                if (null == Modified || Modified.TableName.IsNullOrEmpty())
                {
                    if (null == Original)
                    {
                        return 0;
                    }
                    else
                    {
                        return Original.Rows.Count;
                    }
                }
                else
                {
                    return Modified.Rows.Count;
                }
            }
        }
    }
}
