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
    public class UpdatingController : ApiController
    {
        [HttpPut]
        public HttpResponseMessage UpDateEvent(EventForAdding eventForUpdating,string eventID)
        {
            CalendarConnection connection = new CalendarConnection();
            Event upDatingEvent = new Event();
            upDatingEvent.EventCopy(eventForUpdating);
            connection.GoogleCalendar.Events.Update(upDatingEvent, eventForUpdating.CalendarID, eventID).Execute();
            return (Request.CreateResponse(HttpStatusCode.OK,eventForUpdating));          
        }
    }
}
