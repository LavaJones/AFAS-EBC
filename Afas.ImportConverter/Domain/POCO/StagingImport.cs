using Afas.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.Domain;
using Afc.Core.Domain;
using Afc.Core;

namespace Afas.ImportConverter.Domain.POCO
{
    /// <summary>
    /// This object is used to store import data as it is being proceesed and cleaned in multiple steps.
    /// </summary>
    public class StagingImport : BaseImportConverterModel
    {

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
        public UploadedDataInfo UploadInfo { get; set; }

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



        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateObject(this.Original, "Original", validationMessages);

                SharedUtilities.ValidateObject(this.UploadInfo, "UploadInfo", validationMessages);

                return validationMessages;

            }

        }
    }
}
