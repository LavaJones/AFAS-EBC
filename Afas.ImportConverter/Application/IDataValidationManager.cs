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
    /// Manages multiple Validators that all apply
    /// </summary>
    public interface IDataValidationManager : IDataValidator
    {

        /// <summary>
        /// Adds a Validator that Applies to this process
        /// </summary>
        /// <param name="validator">The Validator to add</param>
        void AddValidator(IDataValidator validator);

    }
}
