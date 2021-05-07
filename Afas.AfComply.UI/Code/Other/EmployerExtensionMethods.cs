using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public static class EmployerExtensionMethods
    {
        public static bool HasBreaksInService(this employer value)
        {
            return value.EMPLOYER_TYPE_ID <= 2;
        }
    }
