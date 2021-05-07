using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Http;
using System.Web.Http.Routing;

namespace Afas.AfComply.UI.ApiControllers
{

    /// <summary>
    /// Route contstraint to keep people from poking around the API.
    /// </summary>
    public class GuidRouteConstraint : IHttpRouteConstraint
    {
        
        Boolean IHttpRouteConstraint.Match(
                HttpRequestMessage request, 
                IHttpRoute route, 
                String parameterName, 
                IDictionary<String, Object> values, 
                HttpRouteDirection routeDirection
            )
        {
            
            Object value;

            if (values.TryGetValue(parameterName, out value) == false)
            {
                return false;
            }

            String input = Convert.ToString(value);

            Guid ignoredGuid;

            return Guid.TryParse(input, out ignoredGuid);

        }

    }

}