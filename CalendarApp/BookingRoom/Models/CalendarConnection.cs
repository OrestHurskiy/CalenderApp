using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace BookingRoom.Models
{
    /// <summary>
    /// Class for creating Google Caledar Service
    /// </summary>
    public class CalendarConnection
    {
        private string serviceEmail;
        private X509Certificate2 certificate;
        private ServiceAccountCredential credential;
        private BaseClientService.Initializer initializer;
        private CalendarService service;
        /// <summary>
        /// Constructor for connecting to Gmail Account
        /// </summary>
        public CalendarConnection()
        {
            serviceEmail = "1022042832033-glqi5vrlgh0gtpcdg620nkrg4hs65835@developer.gserviceaccount.com";
            var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Token.p12");
            certificate = new X509Certificate2(mappedPath, "notasecret", X509KeyStorageFlags.Exportable);

            credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceEmail)
            { Scopes = new[] { CalendarService.Scope.Calendar } }.FromCertificate(certificate));

            initializer = new BaseClientService.Initializer();
            initializer.HttpClientInitializer = credential;
            initializer.ApplicationName = "CalendarApp";
            service = new CalendarService(initializer);
        }
        /// <summary>
        /// Returns Calendar Service
        /// </summary>
        public CalendarService GoogleCalendar
        {
            get { return service; }
        }
    }
}