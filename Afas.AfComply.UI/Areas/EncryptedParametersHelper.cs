using Afc.Core.Presentation.Web;
using Afc.Framework.Presentation.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas
{
    public static class EncryptedParametersHelper
    {
        private static IEncryptedParameters encryptedParametersValues = new EncryptedParameters();

        public static string AsMvcParameter(string key, string value)
        {
            lock (encryptedParametersValues)
            { 
                encryptedParametersValues.Reset();
                encryptedParametersValues["idle"] = DateTime.Now.ToString();

                encryptedParametersValues[key] = value;           

                return encryptedParametersValues.AsMvcUrlParameter;
            }
        }

        public static string AsMvcParameter(Dictionary<string, string> values)
        {
            lock (encryptedParametersValues)
            {
                encryptedParametersValues.Reset();
                encryptedParametersValues["idle"] = DateTime.Now.ToString();

                foreach (var key in values.Keys)
                {
                    encryptedParametersValues[key] = values[key];
                }

                return encryptedParametersValues.AsMvcUrlParameter;
            }
        }
    }
}