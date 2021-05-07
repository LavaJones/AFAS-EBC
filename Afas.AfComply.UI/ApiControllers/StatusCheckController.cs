using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

using System.Web.Http;

using log4net;

namespace Afas.AfComply.UI.ApiControllers
{

    /// <summary>
    /// Is the API Services available and working.
    /// </summary>
    public class StatusCheckController : BaseWebApiController
    {

        public StatusCheckController() : base(LogManager.GetLogger(typeof(StatusCheckController))) { }

        [HttpGet]
        public HttpResponseMessage Available(Guid? authorizationToken)
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (authorizationToken.HasValue == false)
            {

                this.Log.Warn(String.Format("Received unauthorized request. AuthorizationToken is missing."));

                return Request.CreateResponse(HttpStatusCode.NotFound, "Resource not found.");

            }

            if (StatusCheckController.AccessFailureGuid.Equals(authorizationToken))
            {

                this.Log.Warn(String.Format("Short circuit, placebo testing GUID detected."));

                this.Log.Error("Short circuit, placebo testing GUID detected.", new InvalidOperationException());

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Testing placebo detected!");

            }

            if (StatusCheckController.AccessGuid.Equals(authorizationToken) == false)
            {

                this.Log.Warn(String.Format("Received unauthorized request from authorizationToken: {0}.", authorizationToken));

                return Request.CreateResponse(HttpStatusCode.NotFound, "Resource not found.");

            }

            watch.Stop();

            // We want to log some additional information if the system is slow
            if(watch.ElapsedMilliseconds > Feature.ShortTimeStatusCheck)
            {
                // If status check took more than a second to finish, send an error email
                if (watch.ElapsedMilliseconds > Feature.LongTimeStatusCheck)
                {
                    Log.Error("StatusCheckController was very slow. "+ watch.ElapsedMilliseconds);
                }
                else
                {
                    Log.Warn("StatusCheckController was slow. " + watch.ElapsedMilliseconds);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "OK!");

        }

        public static readonly Guid AccessGuid = Guid.Parse("AFD0A415-44AE-4833-B3B2-F918B92505A2");

        public static readonly Guid AccessFailureGuid = Guid.Parse("DEADBEEF-44AE-4833-B3B2-F918B92505A2");

    }

}