using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

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
