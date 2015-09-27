using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models;
using BookingRoom.Models.GoogleConnection;
using Google.Apis.Calendar.v3.Data;
using log4net;

namespace BookingRoom.Controllers
{
    public class SearchController : EventController
    {
        public SearchController(IGoogleCalendarService connection,ILog logger)
            :base(connection,logger)
        {

        }

        [HttpGet]
        public HttpResponseMessage SearchEventById(string eventId,string calendarId)
        {
            Event searchedEvent;
            try
            {
                var events = _connection.GoogleCalendar.Events.List(calendarId).Execute().Items;
                searchedEvent = events.SingleOrDefault(ev => ev.Id == eventId);
            }
            catch (Exception e)
            {
                log.Error("Exception -\n" + e);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }

            return Request.CreateResponse(HttpStatusCode.OK, searchedEvent);
        }
    }
    
}
