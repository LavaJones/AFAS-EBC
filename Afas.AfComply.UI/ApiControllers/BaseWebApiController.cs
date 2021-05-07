using System;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Net.Http;

using System.Web;
using System.Web.Http;

using log4net;

namespace Afas.AfComply.UI.ApiControllers
{

    /// <summary>
    /// Abstract base class for all WebApi controllers. Seems like this should be in Afc.Wgm.Core somewhere.
    /// </summary>
    public abstract class BaseWebApiController : ApiController
    {

        public BaseWebApiController(ILog log) : base()
        {

            this.Log = log;

        }

        protected ILog Log { get; private set; }

    }

}
