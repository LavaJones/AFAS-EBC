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
    /// Important Meta Data for the Import File
    /// </summary>
    public class ImportFileMetaData : ImportMetaData
    {

        /// <summary>
        /// The File path that this was origonally saved as. 
        /// </summary>
        public string SourceFilePath { get; set; }

        /// <summary>
        /// Text denoting what type of file was uploaded, ex: CSV, TSV, excel, etc.
        /// </summary>
        public string FileTypeDescription { get; set; }


        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateString(this.SourceFilePath, "SourceFilePath", validationMessages);

                SharedUtilities.ValidateString(this.FileTypeDescription, "FileTypeDescription", validationMessages);

                return validationMessages;

            }

        }

        /// <summary>
        /// Checks if the supplied if this meta data is applicable to the other metadata based on the provided scope
        /// </summary>
        /// <param name="other">The other MetaData to check against.</param>
        /// <param name="scope">The scope for which to compare the applicability.</param>
        /// <returns>True if it is applicable, False if not.</returns>
        public override bool IsApplicable(ImportMetaData other, ImportFormatCommandScope scope) 
        {

            if (scope is ImportFormatFileCommandScope)
            {
                if (((ImportFormatFileCommandScope)scope).FileType)
                {
                    if (other is ImportFileMetaData)
                    {
                        if (((ImportFileMetaData)other).FileTypeDescription != this.FileTypeDescription)
                        {
                            return false;
                        }                        
                    }
                    else 
                    {
                        return false; 
                    }
                }
            }

            return base.IsApplicable(other, scope);
        }
    }
}
