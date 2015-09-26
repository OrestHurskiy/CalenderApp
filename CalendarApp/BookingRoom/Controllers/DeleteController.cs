using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models;
using BookingRoom.Models.GoogleConnection;

namespace BookingRoom.Controllers
{
    public class DeleteController : EventController
    {
        public DeleteController(ICalendarConnection connection)
            :base(connection)
        {

        }
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(string calendarId, string eventId)
        {
            _connection.GoogleCalendar.Events.Delete(calendarId, eventId).Execute();
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
