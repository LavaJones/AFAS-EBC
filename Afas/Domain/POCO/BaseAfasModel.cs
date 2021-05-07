using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Domain
{

    public abstract class BaseAfasModel : BaseDomainModel
    {
        public BaseAfasModel()
            : base()
        {
        }

        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = new List<ValidationMessage>();

                SharedUtilities.ValidateString(this.CreatedBy, "CreatedBy", validationMessages);

                SharedUtilities.ValidateEntityStatus(this.EntityStatus, "EntityStatus", validationMessages);

                SharedUtilities.ValidateString(this.ModifiedBy, "ModifiedBy", validationMessages);

                SharedUtilities.ValidateDate(this.ModifiedDate, "ModifiedDate", validationMessages);

                SharedUtilities.ValidateGuid(this.ResourceId, "ResourceId", validationMessages);

                return validationMessages;

            }

        }

    }

}
