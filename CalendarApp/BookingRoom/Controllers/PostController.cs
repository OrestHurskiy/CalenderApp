using BookingRoom.Models;
using BookingRoom.Models.GoogleConnection;
using BookingRoom.Models.GoogleCalendar;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models.GoogleEvent;

namespace BookingRoom.Controllers
{

    public class PostController : EventController
    {
        public PostController(ICalendarConnection connection)
            :base(connection)
        {

        }
        [HttpPost]
        public HttpResponseMessage EventPost(CalendarEvent eventPost)
        {
            Event newEvent = ToEventConverter.ToEvent(eventPost);
            try
            {
                _connection.GoogleCalendar.Events.Insert(newEvent, eventPost.CalendarID).Execute();
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return Request.CreateResponse(HttpStatusCode.Created, newEvent);
        }
    }
}
