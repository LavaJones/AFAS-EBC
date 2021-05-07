using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using Afas.AfComply.Reporting.Application.Services;
using System.Web.Http;

namespace Afas.AfComply.UI.Code.Controllers
{
    public class qlikIFrameController 
    {
        public static int ApiPort { get; set; }

        public static int WebPort { get; set; }

        static qlikIFrameController()
        {
            qlikIFrameController.ApiPort = 4243;
            qlikIFrameController.WebPort = 443;
        }

        private string _virtualProxy;
        private string _host;

        private IKeyProvider _provider;

        private Func<object,
                    System.Security.Cryptography.X509Certificates.X509Certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain,
                    System.Net.Security.SslPolicyErrors,
                    bool> _certificateValidationCallback;
        

        public qlikIFrameController(string host, string virtualProxy, IKeyProvider provider, Func<object,
                    System.Security.Cryptography.X509Certificates.X509Certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain,
                    System.Net.Security.SslPolicyErrors,
                    bool> certificateValidationCallback)
            : this(host, virtualProxy, provider)
        {
            _certificateValidationCallback = certificateValidationCallback;
        }
        
        public qlikIFrameController(string host, string virtualProxy, IKeyProvider provider)
        {
            _host = host;
            _virtualProxy = virtualProxy;
            _provider = provider;
        }

        public string GetIframeUrl(string userId, string userDirectory, IEnumerable<KeyValuePair<string, string>> attributes = null)
        {
            var xrfToken = GenerateXrfToken(16);

            var requestUri = $"https://{_host}:{ApiPort}/gps/{_virtualProxy}/ticket?xrfkey={xrfToken}";

            var request = (HttpWebRequest)WebRequest.Create(requestUri);


            if (_certificateValidationCallback != null)
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErros) =>
                    {
                        return _certificateValidationCallback(sender, certificate, chain, sslPolicyErros);
                    };
            var targetId = GetTargetIdFromQlik();

            request.Method = "POST";
            request.Accept = "application/json; charset=UTF-8";
            request.ContentType = "application/json";

            request.Headers.Add("X-Qlik-Xrfkey", xrfToken);

            var requestJson = JsonConvert.SerializeObject(new
            {
                UserId = userId,
                UserDirectory = userDirectory,
                Attributes = attributes == null ? new List<KeyValuePair<string, string>>() : attributes,
                TargetId = targetId,
            });

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
            var targetUri = result.TargetUri;

            return $"{targetUri}?qlikTicket={ticket}";
        }

        private string GetTargetIdFromQlik()
        {
            var request = WebRequest.CreateHttp($"https://{_host}:{WebPort}/{_virtualProxy}/hub/");
            request.AllowAutoRedirect = false;

            Uri targetUri;

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
}