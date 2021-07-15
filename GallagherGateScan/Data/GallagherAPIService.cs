using System;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.Net.Security;

namespace GallagherGateScan.Data
{
    public class GallagherAPIService
    {
        public string APIPath;
        public string APIKey;
        public string APICertThumbprint;
        public X509Certificate2 APICertificate;
        public void Initialize()
        {
            try
            {
                X509Certificate2Collection certs;
                certs = new X509Store(StoreName.My, StoreLocation.LocalMachine, OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly).Certificates.Find(X509FindType.FindByThumbprint, APICertThumbprint, false);
                if (certs.Count > 0) APICertificate = certs[0];
                certs = new X509Store(StoreName.My, StoreLocation.CurrentUser, OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly).Certificates.Find(X509FindType.FindByThumbprint, APICertThumbprint, false);
                if (certs.Count > 0) APICertificate = certs[0];
            }
            catch (Exception e)
            {
                throw new Exception("Could not locate API Certificate...", e);
            }
        }

        public async Task<string> Get()
        {
            HttpWebRequest wr = WebRequest.CreateHttp(APIPath + "");
            wr.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
            {
                return true;
            };
            wr.Method = "GET";
            wr.Headers.Add("Authorization: GGL-API-KEY " + APIKey);
            wr.ClientCertificates = new X509CertificateCollection()
            {
                APICertificate
            };
            WebResponse wresp;
            try
            {
                wresp = await wr.GetResponseAsync();
            }
            catch (WebException e)
            {
                wresp = e.Response;
            }
            StreamReader sr = new StreamReader(wresp.GetResponseStream());
            return await sr.ReadToEndAsync();
        }
    }
}
