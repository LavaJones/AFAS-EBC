using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels
{
    /// <summary>
    /// Used in DropDown selections for the employer lists on admin side
    /// </summary>
    public class EmployerIdSelectViewModel
    {

        /// <summary>
        /// The Human Readable name of the employer. 
        /// </summary>
        public virtual string EmployerName { get; set; }

        /// <summary>
        /// The encrypted Id of the Employer.
        /// </summary>
        public virtual string EncryptedId { get; set; }

        public virtual string EIN { get; set; }

        public virtual string Address { get; set; }

        public virtual string City { get; set; }

        public virtual string State { get; set; }

        public virtual string Zip { get; set; }




    }

}