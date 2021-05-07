using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Afas.AfComply.UI.Areas
{
    public static class UrlHelperHelper
    {

        public static UrlHelper GetUrlHelper()
        {

            if (false == System.Web.HttpContext.Current.Items.Contains("UrlHelper"))
            {
                HttpContextWrapper httpContextWrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
                UrlHelper url = new UrlHelper(new RequestContext(httpContextWrapper, RouteTable.Routes.GetRouteData(httpContextWrapper)));
                System.Web.HttpContext.Current.Items.Add("UrlHelper", url);
            }            

            return (UrlHelper)System.Web.HttpContext.Current.Items["UrlHelper"];
        }

    }
}