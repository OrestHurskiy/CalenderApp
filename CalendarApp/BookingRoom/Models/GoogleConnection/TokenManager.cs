using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BookingRoom.Models.GoogleConnection
{
    public class TokenManager
    {
        private readonly X509Certificate2 _certificate;

        public TokenManager(string password)
        {
            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Token.p12");
            _certificate = new X509Certificate2(mappedPath, password, X509KeyStorageFlags.Exportable);
        }

        public X509Certificate2 Certificate
        {
            get { return _certificate; }
        }
    }
}
