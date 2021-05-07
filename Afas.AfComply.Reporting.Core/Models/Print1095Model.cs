using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Marketing.Models;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class Print1095Model : Model
    {

        public Print1095Model() : base() { }

        /// <summary>
        /// The approval of the Print
        /// </summary>
        [Required]
        public virtual Approved1095FinalModel Approved1095 { get; set; }

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
