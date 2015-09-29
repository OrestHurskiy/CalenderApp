using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using GoogleCalendarService;

namespace BookingRoom.Controllers
{
    public class DeleteController : EventController
    {
        public DeleteController(MeetingBooking connection)
            :base(connection)
        {

        }
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(string calendarId, string eventId)
        {
            try
            {
                _connection.DeleteEvent(calendarId, eventId);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }       
               
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
