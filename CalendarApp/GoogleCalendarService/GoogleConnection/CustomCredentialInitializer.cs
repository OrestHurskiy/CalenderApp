using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarService.GoogleConnection
{
    public class CustomCredentialInitializer : ServiceAccountCredential.Initializer
    {
        public CustomCredentialInitializer(string serviceEmail, IEnumerable<string> scopes, X509Certificate2 certificate) : base(serviceEmail)
        {
            this.Scopes = scopes;
            this.FromCertificate(certificate);
        }
    }
}
