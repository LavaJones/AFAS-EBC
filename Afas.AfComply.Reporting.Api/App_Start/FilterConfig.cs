using System.Web;
using System.Web.Mvc;

namespace Afas.AfComply.Reporting.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

