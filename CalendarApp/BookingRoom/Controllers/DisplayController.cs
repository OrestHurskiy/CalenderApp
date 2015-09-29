using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Apis.Calendar.v3.Data;
using log4net;
using GoogleCalendarService;

namespace BookingRoom.Controllers
{
    public class DisplayController : EventController
    {
        public DisplayController(MeetingBooking connection)
            :base(connection)
        {

        }

        [HttpGet]
        public HttpResponseMessage DisplayEvents(string calendarId)
        {
            IList<Event> events;
            try
            {
                events = _connection.GetEvents(calendarId);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }

            return Request.CreateResponse(HttpStatusCode.OK, events);
        }

    }
}
