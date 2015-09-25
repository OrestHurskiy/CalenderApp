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

    public class PostController : ApiController
    {
        private readonly CalendarConnection _connection;
        public PostController()
        {
            _connection = new CalendarConnection("1022042832033-glqi5vrlgh0gtpcdg620nkrg4hs65835@developer.gserviceaccount.com");
        }
        [HttpPost]
        public HttpResponseMessage EventPost(CalendarEvent eventPost)
        {
            Event newEvent = eventPost.ToEvent();
            _connection.GoogleCalendar.Events.Insert(newEvent, eventPost.CalendarID).Execute();
            return Request.CreateResponse(HttpStatusCode.Created, newEvent);
        }
    }
}
