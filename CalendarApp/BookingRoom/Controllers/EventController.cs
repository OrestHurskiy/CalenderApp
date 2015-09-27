using BookingRoom.Models.GoogleConnection;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookingRoom.Controllers
{
    public class EventController : ApiController
    {
        protected readonly IGoogleCalendarService _connection;
        protected readonly ILog log;
        public EventController(IGoogleCalendarService connection,ILog logger)
        {
            _connection = connection;
            log = logger;
        }
    }
}
