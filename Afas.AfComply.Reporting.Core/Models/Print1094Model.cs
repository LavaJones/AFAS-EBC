using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class Print1094Model : Model
    {
        public Print1094Model() : base() { }

        /// <summary>
        /// The approval of the Print
        /// </summary>
        [Required]
        public virtual Approved1094FinalPart1Model Approved1094 { get; set; }

        /// <summary>
        /// The print batch
        /// </summary>
        [Required]
        public virtual PrintBatchModel PrintBatch { get; set; }

        /// <summary>
        /// The actual file that printed
        /// </summary>
        [Required]
        public virtual string OutputFilePath { get; set; }
    }
}
