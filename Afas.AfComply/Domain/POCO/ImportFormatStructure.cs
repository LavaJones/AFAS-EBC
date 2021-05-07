using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.POCO
{
    /// <summary>
    /// List the actions that can be taken once the Data has been formatted
    /// </summary>
    public enum PostFormattingActions 
    {
        /// <summary>
        /// Once the Formatting is complete, add the data to the Queue for Human Inspection.
        /// </summary>
        QueueData,

        /// <summary>
        /// Once the formatting is complete, rerun the process to format the resulting data.
        /// </summary>
        ReprocessFormatting,
        
        /// <summary>
        /// Once the Formatting is complete, attempt to import the data into the system.
        /// </summary>
        ImportData,

        /// <summary>
        /// Once the Formatting is complete, check for valid reprocssing, if none found then attmept the import
        /// </summary>
        ReprocessOrImport,

        /// <summary>
        /// Once the Formatting is complete, attempt to import but reprocess formatting on import failure
        /// </summary>
        ImportOrReprocess,
    }

    /// <summary>
    /// This class contains the structure unique to a specific File upload from an employer
    /// </summary>
    public class ImportFormatStructure : BaseAfasModel
    {


        /// <summary>
        /// Database PK for this object
        /// </summary>
        public int ImportFormatStructureId { get; set; }

        /// <summary>
        /// The Id of the Employer that this structure is associated with
        /// </summary>
        public int EmployerId { get; set; }

        /// <summary>
        /// The Guid of the employer associated with this structure 
        /// </summary>
        public Guid EmployerGuid { get; set; }

        /// <summary>
        /// Text denoting how upload was done, ex: Bulk Import, Agent, Client, Correction, etc.
        /// </summary>
        public string UploadSourceDescription { get; set; }

        /// <summary>
        /// Text denoting what type of upload was done, ex: Demographics, Payroll, Coverage, etc. 
        /// </summary>
        public string UploadTypeDescription { get; set; }

        /// <summary>
        /// Text denoting what type of file was uploaded, ex: CSV, TSV, excel, etc.
        /// </summary>
        public string FileTypeDescription { get; set; }

        /// <summary>
        /// The Id to the payroll provider or null if not payroll
        /// </summary>
        public int? PayrollProviderId{ get; set; }

        /// <summary>
        /// List of each column header, this lets us identify files with the same headers
        /// </summary>
        public List<string> RequiredColumnHeaders { get; set; }

        /// <summary>
        /// Single string version of all the headers for this import
        /// </summary>
        public string RequiredCommaSeperatedHeaders 
        {
            get 
            {
                string commaSeSeperated = "";
                foreach (string header in RequiredColumnHeaders.OrderBy(text => text))
                {
                    commaSeSeperated += header.Trim() + ", ";
                }

                return commaSeSeperated.Trim().Trim(',').Trim();
            }

            set 
            {
                List<string> headers = new List<string>();
                string[] heardersArray = value.Trim().Trim(',').Trim().Split(',');
                foreach (string header in heardersArray) 
                {
                    headers.Add(header.Trim());
                }
                RequiredColumnHeaders = headers;
            }
        }

        /// <summary>
        /// The list of commands to format this structure
        /// </summary>
        public List<ImputFormatCommand> Commands { get; set; }

        /// <summary>
        /// The list of commands in order of execution
        /// </summary>
        public IEnumerable<ImputFormatCommand> OrderedCommands { get { return Commands.OrderBy(command => command.Ordinal); } }

        /// <summary>
        /// The action to do once this formatting is completed 
        /// </summary>
        public PostFormattingActions CompletedAction { get; set; }
    }
}
