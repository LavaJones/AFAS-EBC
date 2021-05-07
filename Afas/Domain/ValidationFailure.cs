using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Domain
{
    /// <summary>
    /// Defines different levels of Validation failures.
    /// </summary>
    public enum ValidationSeverity
    {
        /// <summary>
        /// A validation failure at the Dataset Level means that the entire Datset is invalid because of this issue
        /// </summary>
        Dataset = 1,
        /// <summary>
        /// A validation failure at the Item Level means that the whole Item is invalid because of this issue
        /// </summary>
        Item = 2,
        /// <summary>
        /// A validation failure at the Value Level means that only the one Value is invalid because of this issue
        /// </summary>
        Value = 3
    }
    
    /// <summary>
    /// This data describes a validation failure. It is intended to be used to resolve the issue.
    /// </summary>
    public class ValidationFailure
    {
        /// <summary>
        /// The type of Validation that failed. (ex. FEIN)
        /// </summary>
        public string ValidationType { get; set; }

        /// <summary>
        /// Message explaining the validation failure.
        /// </summary>
        public string ValidationMessage { get; set; }

        /// <summary>
        /// Identifier of the item failing Validation or null.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The severity of the validation failure.
        /// </summary>
        public ValidationSeverity ValidationSeverity { get; set; }

    }
}
