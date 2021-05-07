using Afas.Domain;
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
    public class ImportFormatCommandScope : BaseImportConverterModel
    {
        /// <summary>
        /// This scope covers all imports that share the same headers
        /// </summary>
        public virtual bool Headers { get; set; }

        /// <summary>
        /// This scope covers all imports that share the same input source
        /// </summary>
        public virtual bool InputSource { get; set; }

        /// <summary>
        /// This scope covers all imports that share the same upload type
        /// </summary>
        public virtual bool UploadType { get; set; }

        /// <summary>
        /// This scope covers all imports
        /// </summary>
        public virtual bool Global { get; set; }

        /// <summary>
        /// There is no scope defined
        /// </summary>
        public virtual bool None 
        { 
            get
            {
                return Headers || InputSource || UploadType || Global;
            }
            set 
            {
                if(value == true)
                {
                    Headers = false;
                    InputSource = false;
                    UploadType = false;
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
        public virtual int CompareFlags(ImportFormatCommandScope other)
        {
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
    
            if (true == this.Global && false == other.Global)
            { return -1; }
            if (false == this.Global && true == other.Global)
            { return 1; }

            return 0;
        }

    }

}
