using Afas.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.POCO
{
    /// <summary>
    /// This Class defines the scopes of Import Formattting Commands so that they are limited in their application within the scope.
    /// </summary>
    public class ImportFormatFileCommandScope : ImportFormatCommandScope
    {
        /// <summary>
        /// This scope covers all imports that share the same file type (ie. .CSV, .TSV, .XLS)
        /// </summary>
        public virtual bool FileType { get; set; }

        /// <summary>
        /// There is no scope defined
        /// </summary>
        public override bool None 
        { 
            get
            {
                return Headers || InputSource || UploadType || Global || FileType;
            }
            set 
            {
                if(value == true)
                {
                    Headers = false;
                    InputSource = false;
                    UploadType = false;
                    FileType = false;
                    Global = false;
                }
            }
        }

        /// <summary>
        /// Compares the flags of this vs those of the other provided and returns the 1, -1, or zero
        /// </summary>
        /// <param name="other">The flags to compare to.</param>
        /// <returns>
        /// Less than zero: This instance precedes other in the sort order.
        /// Zero: This instance occurs in the same position in the sort order as other.
        /// Greater than zero: This instance follows other in the sort order.
        /// </returns>
        public override int CompareFlags(ImportFormatCommandScope otherOne)
        {
            if (otherOne is ImportFormatFileCommandScope)
            {
                ImportFormatFileCommandScope other = (ImportFormatFileCommandScope)otherOne;
                if (true == this.Headers && false == other.Headers)
                { return -1; }
                if (false == this.Headers && true == other.Headers)
                { return 1; }

                if (true == this.InputSource && false == other.InputSource)
                { return -1; }
                if (false == this.InputSource && true == other.InputSource)
                { return 1; }

                if (true == this.UploadType && false == other.UploadType)
                { return -1; }
                if (false == this.UploadType && true == other.UploadType)
                { return 1; }

                if (true == this.FileType && false == other.FileType)
                { return -1; }
                if (false == this.FileType && true == other.FileType)
                { return 1; }

                if (true == this.Global && false == other.Global)
                { return -1; }
                if (false == this.Global && true == other.Global)
                { return 1; }

                return 0;
            }
            else 
            {
                return base.CompareFlags(otherOne);
            }
        }
    }
}
