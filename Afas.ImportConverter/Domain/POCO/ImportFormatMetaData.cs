using Afas.Domain;
using Afas.Domain.POCO;
using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.POCO
{

    /// <summary>
    /// Important Meta Data for the Import Format
    /// </summary>
    public class ImportMetaData : BaseImportConverterModel
    {
        /// <summary>
        /// Default constructor that sets the list
        /// </summary>
        public ImportMetaData():base()
        {
            ColumnHeaders = new List<string>();
        }

        /// <summary>
        /// List of each column header, this lets us identify files with the same headers
        /// </summary>
        public virtual IList<string> ColumnHeaders { get; set; }

        /// <summary>
        /// Text denoting how upload was done, ex: Bulk Import, Agent, Client, Correction, etc.
        /// </summary>
        public string UploadSourceDescription { get; set; }

        /// <summary>
        /// Text denoting what type of upload was done, ex: Demographics, Payroll, Coverage, etc. 
        /// </summary>
        public string UploadTypeDescription { get; set; }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateObject(this.ColumnHeaders, "ColumnHeaders", validationMessages);

                SharedUtilities.ValidateString(this.UploadSourceDescription, "UploadSourceDescription", validationMessages);

                SharedUtilities.ValidateString(this.UploadTypeDescription, "UploadTypeDescription", validationMessages);

                return validationMessages;

            }

        }

        /// <summary>
        /// Checks if the supplied if this meta data is applicable to the other metadata based on the provided scope
        /// </summary>
        /// <param name="other">The other MetaData to check against.</param>
        /// <param name="scope">The scope for which to compare the applicability</param>
        /// <returns>True if it is applicable, False if not.</returns>
        public virtual bool IsApplicable(ImportMetaData other, ImportFormatCommandScope scope) 
        {
            if (scope.None)
            {
                return false;
            }

            if (scope.Headers)
            {
                foreach (string header in this.ColumnHeaders)
                {
                    if (false == other.ColumnHeaders.Contains(header))
                    {
                        return false;
                    }
                }
            }

            if (scope.InputSource)
            {
                if (other.UploadSourceDescription != this.UploadSourceDescription)
                {
                    return false;
                }
            }

            if (scope.UploadType)
            {
                if (other.UploadTypeDescription != this.UploadTypeDescription)
                {
                    return false;
                }
            }

            return true;
        }

    }

}
