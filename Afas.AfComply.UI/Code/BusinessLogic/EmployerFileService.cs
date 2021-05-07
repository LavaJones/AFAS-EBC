using System.Collections.Generic;
using System.Linq;
using System.IO;
using Afas.AfComply.UI.Code.POCOs;


public class EmployerFileService
{

    public IList<EmployerFile> ResolveEmployerNames(IList<employer> employers, IList<FileInfo> files)
    {
        return (from file in files
            from employer in employers
            let empFileTypeList = CreateEmpFileTypeList(employer)
            where empFileTypeList.Contains(file.Name.Split('_')[0] + "_")
            select new EmployerFile
            {
                CreationTimeUtc = file.CreationTimeUtc,
                EmployerId = employer.EMPLOYER_ID,
                EmployerName = employer.EMPLOYER_NAME,
                FileFullPath = file.FullName,
                Length = file.Length,
                FileName = file.Name
            }).ToList();
    }

    public List<string> CreateEmpFileTypeList(employer empr)
    {
        var list = new List<string>
        {
            empr.EMPLOYER_IMPORT_EC,
            empr.EMPLOYER_IMPORT_GP,
            empr.EMPLOYER_IMPORT_HR,
            empr.EMPLOYER_IMPORT_IC,
            empr.EMPLOYER_IMPORT_IO,
            empr.EMPLOYER_IMPORT_PAY_MOD,
            empr.EMPLOYER_IMPORT_PAYROLL,
            empr.EMPLOYER_IMPORT_EMPLOYEE
        };

        return list;
    }

}
