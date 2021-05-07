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
using Afas.ImportConverter.Domain.ImportFormatting.UploadedData;
using System.IO;

namespace Afas.ImportConverter.Domain.ImportFormatting.Staging
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
        /// This is used so that EF can treat the Data Table as XML
        /// </summary>
        internal virtual string OriginalXml
        {
            get
            {
                using (StringWriter writer = new StringWriter())
                {
                    Original.WriteXml(writer, XmlWriteMode.WriteSchema);
                    string text = writer.ToString();
                    return text.Trim();
                }
            }
            set
            {
                if (value != null)
                {
                    using (Stream stream = new MemoryStream())
                    {
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(value);
                        writer.Flush();
                        stream.Position = 0;
                        Original.ReadXml(stream);
                    }
                }
            }
        }

        /// <summary>
        /// A Data Table containing the Modified Data
        /// </summary>
        public DataTable Modified { get; set; }

        /// <summary>
        /// This is used so that EF can treat the Data Table as XML
        /// </summary>
        internal virtual string ModifiedXml
        {
            get
            {
                if (Modified == null)
                {
                    return null;
                }
                using (StringWriter writer = new StringWriter())
                {
                    Modified.WriteXml(writer, XmlWriteMode.WriteSchema);
                    string text = writer.ToString();
                    return text.Trim();
                }
            }
            set
            {
                if (value != null)
                {
                    using (Stream stream = new MemoryStream())
                    {
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(value);
                        writer.Flush();
                        stream.Position = 0;
                        Modified.ReadXml(stream);
                    }
                }
            }
        }

        /// <summary>
        /// The Meta data of the File that this data came from 
        /// </summary>
        public BaseUploadedDataInfo UploadInfo { get; set; }

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
