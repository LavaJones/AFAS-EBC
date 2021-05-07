using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.Domain;
using Afas.ImportConverter.Domain.POCO;

namespace Afas.ImportConverter.Application.Implementation
{
    public class DataValidationManager : IDataValidationManager
    {

        private IList<IDataValidator> Validators { get; set; }

        public IList<ValidationFailure> DataValidationMessages
        {
            get;
            protected set;
        }

        public DataValidationManager()
        {
            // default the Lists 
            this.Validators = new List<IDataValidator>();
            this.DataValidationMessages = new List<ValidationFailure>();
        }

        void IDataValidationManager.AddValidator(IDataValidator validator)
        {
            // Add the validator if it is not null and is not already added
            if (false == this.Validators.Contains(validator) && null != validator)
            {
                this.Validators.Add(validator);
            }
        }

        bool IDataValidator.IsDataValid(ImportData importData)
        {
            bool valid = true;
            // Check all Validators for validity
            foreach (IDataValidator validator in this.Validators)
            {
                // If any validator failed then return false, and gather all failed validation messages.
                if (false == validator.IsDataValid(importData))
                {
                    valid = false;
                    this.DataValidationMessages = this.DataValidationMessages.Concat(validator.DataValidationMessages).ToList();
                }
            }
            return valid;
        }
    }
}
