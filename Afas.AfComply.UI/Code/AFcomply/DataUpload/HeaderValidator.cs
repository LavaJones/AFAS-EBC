using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Afas.AfComply.Domain;
using log4net;
using Afas.Domain;
using Afas.ImportConverter.Application;

namespace Afas.AfComply.UI.Code.AFcomply.DataUpload
{
    /// <summary>
    /// This class validates header values
    /// </summary>
    public class HeaderValidator : IHeaderValidator
    {
        private ILog Log = LogManager.GetLogger(typeof(HeaderValidator));

        /// <summary>
        /// Check if the header row looks like a real header or contains actual data
        /// </summary>
        /// <param name="headers">The Row To Check</param>
        /// <returns>True if the row looks liek a header, false if it looks like data</returns>
        public bool ValidateHeaders(IEnumerable<string> headers)
        {
            int blankHeaderCount = 0;
            foreach (string header in headers)
            {
                if (header.IsValidFedId()
                    || header.IsValidSsn()
                    || header.IsValidZipCode()
                    || header.IsValidPhoneNumber()
                    || header.IsValidDate()
                    || header.IsValidEmail()
                    )
                {
                    return false;
                }

                if (header.IsNullOrEmpty())
                {
                    blankHeaderCount++;
                }
            }

            if (blankHeaderCount >= headers.Count())
            {
                return false;
            }

            return true;
        }
    }
}