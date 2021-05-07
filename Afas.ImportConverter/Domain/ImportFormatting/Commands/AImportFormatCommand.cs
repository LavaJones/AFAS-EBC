using Afas.Domain;
using Afas.Domain.POCO;
using Afas.ImportConverter.Domain.POCO;
using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting
{

    /// <summary>
    /// A generic Formatting Command that can be strung together to format the input data
    /// </summary>
    public abstract class AImportFormatCommand : BaseImportConverterModel, IComparable<IImportFormatCommand>, IImportFormatCommand
    {

        /// <summary>
        /// The Metadata defining what this command applies to.
        /// </summary>
        public ImportMetaData MetaData { get; set; }

        /// <summary>
        /// The scope that this command is applicable to
        /// </summary>
        public ImportFormatCommandScope Scope { get; set; } 

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public abstract ImportFormatCommandTypes CommandType { get; }

        /// <summary>
        /// Key value Pair for parameters of the command, key is the parameter name, value is the string value.
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }
        
        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public virtual IList<string> RequiredParameters { get { return new List<string>(); } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public virtual IList<string> OptionalParameters { get { return new List<string>(); } }


        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                SharedUtilities.ValidateObject(this.MetaData, "MetaData", validationMessages);

                SharedUtilities.ValidateObject(this.Scope, "Scope", validationMessages);

                SharedUtilities.ValidateObject(this.Parameters, "Parameters", validationMessages);

                SharedUtilities.ValidateObject(this.RequiredParameters, "RequiredParameters", validationMessages);

                SharedUtilities.ValidateObject(this.OptionalParameters, "OptionalParameters", validationMessages);

                foreach(string required in this.RequiredParameters)
                {
                    if (false == this.Parameters.ContainsKey(required))
                    {
                        validationMessages.Add(new ValidationMessage("Parameters", "Required Parameter ["+ required + "] was not provided.", ValidationMessageSeverity.Error));
                    }
                }

                return validationMessages;

            }

        }

        /// <summary>
        /// Default Constructor with no args
        /// </summary>
        public AImportFormatCommand()
        {

            this.Parameters = new Dictionary<string, string>();
            this.Scope = new ImportFormatCommandScope() { None = true };

        }

        /// <summary>
        /// Checks to see if this Command applys to an bject with this metadata
        /// </summary>
        /// <param name="other">The meta data to check.</param>
        /// <returns>True it applies, false it does not apply.</returns>
        public virtual bool AppliesTo(ImportMetaData other)
        {
            
            return this.MetaData.IsApplicable(other, this.Scope);

        }

        /// <summary>
        /// Apply this Command to format this data
        /// </summary>
        /// <param name="metaData">The metaData, including the data to be formatted.</param>
        /// <returns>True if the format was applied correctly, false if there was a failure.</returns>
        public abstract bool ApplyTo(ImportData metaData);

        /// <summary>
        /// Implements the compare to Interface to compare and sort ImportFormatCommands
        /// </summary>
        /// <param name="other">The ImportFormatCommand to compare to.</param>
        /// <returns>
        /// Less than zero: This instance precedes obj in the sort order.
        /// Zero: This instance occurs in the same position in the sort order as obj.
        /// Greater than zero: This instance follows obj in the sort order.
        /// </returns>
        public virtual int CompareTo(IImportFormatCommand other)
        {

            return this.Scope.CompareFlags(other.Scope);

        }
    }
}
