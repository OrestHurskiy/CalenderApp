using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models;
using BookingRoom.Models.GoogleCalendar;
using BookingRoom.Models.GoogleConnection;
using Google.Apis.Calendar.v3.Data;
using log4net;

namespace BookingRoom.Controllers
{
    public class DisplayController : EventController
    {
        public DisplayController(IGoogleCalendarService connection,ILog logger)
            :base(connection,logger)
        {

        }

        [HttpGet]
        public HttpResponseMessage DisplayEvents(string calendarId)
        {
            IList<Event> events;
            try
            {
                events = _connection.GoogleCalendar.Events.List(calendarId).Execute().Items;
            }
            catch (Exception e)
            {
                log.Error("Exception -\n" + e);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }

            return Request.CreateResponse(HttpStatusCode.OK, events);
        }

    }
}
