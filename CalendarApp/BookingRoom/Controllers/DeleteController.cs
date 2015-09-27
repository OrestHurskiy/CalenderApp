using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models;
using BookingRoom.Models.GoogleConnection;
using log4net;

namespace BookingRoom.Controllers
{
    public class DeleteController : EventController
    {
        public DeleteController(IGoogleCalendarService connection,ILog logger)
            :base(connection,logger)
        {

        }
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(string calendarId, string eventId)
        {
            try
            {
                _connection.GoogleCalendar.Events.Delete(calendarId, eventId).Execute();
            }
            catch (Exception e)
            {
                log.Error("Exception -\n" + e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
