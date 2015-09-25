using BookingRoom.Models;
using BookingRoom.Models.Posting;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookingRoom.Controllers
{
    public class PostingController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage EventPost(EventForAdding eventPost)
        {
            Event newEvent = new Event();
            newEvent.EventCopy(eventPost);
            CalendarConnection connection = new CalendarConnection();
            connection.GoogleCalendar.Events.Insert(newEvent, eventPost.CalendarID).Execute();
            return Request.CreateResponse(HttpStatusCode.Created, newEvent);
        }
    }
}
