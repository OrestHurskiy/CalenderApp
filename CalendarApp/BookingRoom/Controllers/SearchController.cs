using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models;
using Google.Apis.Calendar.v3.Data;
using log4net;
using Google.Apis.Calendar.v3;
using GoogleCalendarService;

namespace BookingRoom.Controllers
{
    public class SearchController : EventController
    {
        public SearchController(MeetingBooking connection)
            :base(connection)
        {

        }

        [HttpGet]
        public HttpResponseMessage SearchEventById(string eventId, string calendarId)
        {
            Event searchedEvent;
            try
            {
                 searchedEvent = _connection.SearchEventById(calendarId, eventId);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }

            return Request.CreateResponse(HttpStatusCode.OK,searchedEvent);
        }
    }
    
}
