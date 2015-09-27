using BookingRoom.Models.GoogleConnection;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using ServiceCredential = BookingRoom.Models.GoogleConnection.ServiceCredential;

namespace BookingRoom.Models
{
    /// <summary>
    /// Class for creating Google Caledar Service
    /// </summary>
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly ClientInitializer _initializer;
        private readonly ServiceCredential _credential;
        private BaseClientService.Initializer initializer;
        private readonly CalendarService service;
        /// <summary>
        /// Constructor for connecting to Gmail Account
        /// </summary>
        public GoogleCalendarService(string applicationName, ServiceCredential credential, ClientInitializer initializer)
        {
            _initializer = initializer;
            _credential = credential;
            _initializer.ClientService.HttpClientInitializer = _credential.Credential;
            _initializer.ClientService.ApplicationName = applicationName;
            service = new CalendarService(_initializer.ClientService);
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