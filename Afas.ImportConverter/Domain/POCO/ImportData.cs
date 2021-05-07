using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.Domain;
using Afc.Core;
using Afc.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Afas.ImportConverter.Domain.POCO
{

    /// <summary>
    /// A specific set of Metadata that is tied to a specific set of data (like a file).
    /// </summary>
    public class ImportData : BaseImportConverterModel
    {

        /// <summary>
        /// MetatDat describing this import 
        /// </summary>
        public virtual ImportMetaData MetaData { get; set; }                

        /// <summary>
        /// If this MetaData is associated with a Data Table
        /// </summary>
        
        public virtual DataTable Data { get; set; }

        /// <summary>
        /// This is used so that EF can treat the Data Table as XML
        /// </summary>
        internal virtual string DataXml
        {
            get
            {
                using (Stream stream = new MemoryStream())
                {
                    Data.WriteXml(stream, XmlWriteMode.WriteSchema);
                    StreamReader reader = new StreamReader(stream);
                    return reader.ReadToEnd();
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
                        Data.ReadXml(stream);
                    }
                }                
            }
        }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateObject(this.MetaData, "MetaData", validationMessages);

                return validationMessages;

            }

        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImportData() 
        {
            Data = new DataTable();
        }
    }
}
