using BookingRoom.Models;
using BookingRoom.Models.GoogleCalendar;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models.GoogleEvent;
using log4net;
using Google.Apis.Calendar.v3;
using GoogleCalendarService;

namespace BookingRoom.Controllers
{

    public class PostEventController : EventController
    {
        public PostEventController(MeetingBooking connection)
            : base(connection)
        {

        }
        [HttpPost]
        public HttpResponseMessage EventPost(CalendarEvent eventPost)
        {
            try
            {
                _connection.PostEvent(ToEventConverter.ToEvent(eventPost), eventPost.CalendarID);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

            return Request.CreateResponse(HttpStatusCode.Created, eventPost);
        }
    }
}
