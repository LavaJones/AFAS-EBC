using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Afc.Web.Reporting.Application
{
    public class QlikIframeHelper
    {
        public static int ApiPort { get; set; }
        public static int WebPort { get; set; }

        static QlikIframeHelper()
        {
            QlikIframeHelper.ApiPort = 4243;
            QlikIframeHelper.WebPort = 443;
        }

        private string _virtualProxy;
        private string _host;

        private IKeyProvider _provider;

        private Func<object,
                    System.Security.Cryptography.X509Certificates.X509Certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain,
                    System.Net.Security.SslPolicyErrors,
                    bool> _certificateValidationCallback;

        public QlikIframeHelper(string host, string virtualProxy, IKeyProvider provider, Func<object,
                    System.Security.Cryptography.X509Certificates.X509Certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain,
                    System.Net.Security.SslPolicyErrors,
                    bool> certificateValidationCallback)
            : this(host, virtualProxy, provider)
        {
            _certificateValidationCallback = certificateValidationCallback;
        }

        public QlikIframeHelper(string host, string virtualProxy, IKeyProvider provider)
        {
            _host = host;
            _virtualProxy = virtualProxy;
            _provider = provider;
        }

        public string GetIframeUrl(string userId, string userDirectory, string qlikSenseApplication = null, IEnumerable<Dictionary<string, string>> attributes = null, string targetUrl = null)
        {
            var xrfToken = GenerateXrfToken(16);

            var requestUri = $"https://{_host}:{ApiPort}/qps/{_virtualProxy}/ticket?xrfkey={xrfToken}";

            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            if (_certificateValidationCallback != null)
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                {
                    return _certificateValidationCallback(sender, certificate, chain, sslPolicyErrors);
                };

            var targetId = GetTargetIdFromQlik();

            request.ClientCertificates.Add(_provider.Certificate);

            request.Method = "POST";
            request.Accept = "application/json; charset=UTF-8";
            request.ContentType = "application/json";

            request.Headers.Add("X-Qlik-Xrfkey", xrfToken);

            string requestJson = string.Empty;

            if (attributes != null)
            {
                requestJson = JsonConvert.SerializeObject(new
                {
                    UserId = userId,
                    UserDirectory = userDirectory,
                    Attributes = attributes == null ? new List<Dictionary<string, string>>() : attributes,
                    TargetId = targetId,
                });
            }
            else
            {
                requestJson = JsonConvert.SerializeObject(new
                {
                    UserId = userId,
                    UserDirectory = userDirectory,
                    TargetId = targetId,
                });
            }

            request.GetRequestStream().Write(Encoding.ASCII.GetBytes(requestJson), 0, Encoding.ASCII.GetBytes(requestJson).Length);
            request.GetRequestStream().Close();

            string data = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
                reader.Close();
            }

            dynamic result = JObject.Parse(data);

            var ticket = result.Ticket;
            string targetUri;

            if (string.IsNullOrEmpty(qlikSenseApplication))
            {
                targetUri = result.TargetUri;
            }
            else
            {
                targetUri = $"https://{_host}/{_virtualProxy}/sense/app/{qlikSenseApplication}";
            }

            if (string.IsNullOrEmpty(targetUrl))
            {
                return $"{targetUri}?qlikTicket={ticket}";
            }

            return $"{targetUrl}?qlikTicket={ticket}";
        }

        private string GetTargetIdFromQlik()
        {
            var request = WebRequest.CreateHttp($"https://{_host}:{WebPort}/{_virtualProxy}/hub/");
            request.AllowAutoRedirect = false;

            Uri targetUri = null;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                targetUri = new Uri(response.Headers["Location"]);
            }

            return HttpUtility.ParseQueryString(targetUri.Query).Get("targetId");
        }

        private string GenerateXrfToken(int length)
        {
            var characters = "abcdefghijklmnopqrstuwxyzABCDEFGHIJKLMNOPQRSTUWXYZ0123456789";

            var random = new Random();

            var result = new StringBuilder();

            for (int i = 0; i < length; i++)
                result.Append(characters.Substring(random.Next(characters.Length), 1));

            return result.ToString();
        }
    }


    public interface IKeyProvider
    {
        X509Certificate2 Certificate { get; }

    }
    public class LocalUserCertificateManager : IKeyProvider
    {
        public X509Certificate2 Certificate { get; set; }

        public LocalUserCertificateManager(string thumbprint, StoreLocation location)
        {
            X509Store certStore = new X509Store(StoreName.My, location);

            certStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = certStore.Certificates.Find(
                X509FindType.FindByThumbprint,
                thumbprint,
                false);

            if (certCollection.Count > 0)
            {
                Certificate = certCollection[0];
            }
            else
            {
                throw new KeyNotFoundException($"Unable to find certificate for {thumbprint}.");
            }

            certStore.Close();
        }


    }
}