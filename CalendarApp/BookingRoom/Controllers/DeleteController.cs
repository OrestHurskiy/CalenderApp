using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models;

namespace BookingRoom.Controllers
{
    public class DeleteController : ApiController
    {
        private readonly CalendarConnection _connection;
        public DeleteController()
        {
            _connection = new CalendarConnection("1022042832033-glqi5vrlgh0gtpcdg620nkrg4hs65835@developer.gserviceaccount.com");
        }
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(string calendarId, string eventId)
        {
            _connection.GoogleCalendar.Events.Delete(calendarId, eventId).Execute();
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
