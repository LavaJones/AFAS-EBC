using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.Domain;

namespace Afas.AfComply.Domain
{
    public static class BooleanExtensionMethods
    {
        public static string ToValueString(this bool value)
        {
            return value ? "X".GetCsvEscaped() : "".GetCsvEscaped();
        }
    }
}
