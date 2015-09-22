using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarService.GoogleConnection
{
    public class CustomInitializer : BaseClientService.Initializer
    {
        public CustomInitializer(ServiceAccountCredential credential, string appName) : base()
        {
            this.HttpClientInitializer = credential;
            this.ApplicationName = appName;
        }
    }
}
