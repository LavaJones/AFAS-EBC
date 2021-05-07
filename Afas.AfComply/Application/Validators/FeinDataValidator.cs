using Afas.ImportConverter.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.Domain;
using Afas.ImportConverter.Domain.POCO;
using Afas.AfComply.Domain;
using log4net;

namespace Afas.AfComply.Application.Validators
{
    public class FeinDataValidator : IDataValidator
    {
        public string EmployerFein { get; set; }

        public FeinDataValidator(string Fein)
        {
            this.EmployerFein = Fein;
            this.DataValidationMessages = new List<ValidationFailure>();
        }

        public IList<ValidationFailure> DataValidationMessages
        {
            get;
            protected set;
        }

        bool IDataValidator.IsDataValid(ImportData importData)
        {
            try
            {
                if (false == importData.Data.VerifyContainsOnlyThisFederalIdentificationNumber(EmployerFein))
                {
                    ValidationFailure failure = new ValidationFailure();
                    failure.ValidationType = "FEIN";
                    failure.ValidationSeverity = ValidationSeverity.Dataset;
                    failure.ValidationMessage = "Could not Find (Co)FEIN Column.";
                    failure.Identifier = null;

                    DataValidationMessages.Add(failure);

                    return false;
                }
            }
            catch (Exception ex)
            {
                ValidationFailure failure = new ValidationFailure();
                failure.ValidationType = "FEIN";
                failure.ValidationSeverity = ValidationSeverity.Dataset;
                failure.ValidationMessage = ex.Message;
                failure.Identifier = null;

                DataValidationMessages.Add(failure);
                
                return false;
            }

            return true;
        }
    }
}
