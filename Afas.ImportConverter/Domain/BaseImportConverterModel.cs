using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Afas.ImportConverter.Domain
{
    public abstract class BaseImportConverterModel : BaseAfasModel
    {

        /// <summary>
        /// The tables PK
        /// </summary>
        [Key]
        public virtual long ID { get; set; }

    }
}
