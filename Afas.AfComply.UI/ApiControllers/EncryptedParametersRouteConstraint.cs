using Afc.Core.Logging;
using Afc.Core.Presentation.Web;
using Afc.Framework.Presentation.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Afas.AfComply.UI.ApiControllers
{

    /// <summary>
    /// Wrapper around the Afc.Framework version that was not injecting correctly.
    /// </summary>
    public class EncryptedParametersRouteConstraint : IRouteConstraint
    {

        public Boolean Match(
                HttpContextBase httpContext,
                Route route,
                String parameterName,
                RouteValueDictionary values,
                RouteDirection routeDirection
            )
        {

            if (values.ContainsKey(parameterName) == false)
            {
                return false;
            }

            if (routeDirection == RouteDirection.UrlGeneration)
            {
                return true;
            }

            String encryptedValue = values[parameterName].ToString();

            if (encryptedValue.Trim() == String.Empty)
            {
                return false;
            }

            IEncryptedParameters encryptedParameters = new EncryptedParameters();

            (encryptedParameters as ILoggingContextAware).Log =
                    Afc.Core.Logging.InternalLogManager.GetLogger(typeof(Afc.Framework.Presentation.Web.EncryptedParameters));

            return encryptedParameters.Load(encryptedValue);

        }

    }

}