using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;

namespace BookingRoom.Models.GoogleConnection
{
    public class ServiceCredential
    {
        private readonly ServiceAccountCredential _credential;
        private readonly TokenManager _tokenManager;

        public ServiceCredential(string serviceEmail, TokenManager tokenManager)
        {
            _tokenManager = tokenManager;

            _credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceEmail)
            {
                Scopes = new[] { CalendarService.Scope.Calendar }
            }.FromCertificate(_tokenManager.Certificate));
        }

        public ServiceAccountCredential Credential
        {
            get { return _credential; }
        }
    }
}
