using Afas.Domain;
using Afas.ImportConverter.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Application
{

    /// <summary>
    /// A Validator for a specific case. 
    /// </summary>
    public interface IDataValidator
    {

        /// <summary>
        /// Message if the data failed validation
        /// </summary>
        IList<ValidationFailure> DataValidationMessages { get; } 

        /// <summary>
        /// Validate the supplied Import Data for any active Validators
        /// </summary>
        /// <param name="importData">The Data to Validate</param>
        /// <returns>True if there were no validation errors, false if there were validation errors.</returns>
        bool IsDataValid(ImportData importData);

    }
}
