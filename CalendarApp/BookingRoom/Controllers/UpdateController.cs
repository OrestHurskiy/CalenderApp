using BookingRoom.Models;
using BookingRoom.Models.GoogleCalendar;
using Google.Apis.Requests;
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
    public class UpdateController : EventController
    {
        public UpdateController(MeetingBooking connection)
            : base(connection)
        {

        }
        [HttpPut]
        public HttpResponseMessage UpDateEvent(CalendarEvent eventForUpdate, string eventID)
        {
            try
            {
                _connection.UpdateEvent(ToEventConverter.ToEvent(eventForUpdate), eventForUpdate.CalendarID, eventID);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            return Request.CreateResponse(HttpStatusCode.Created, eventForUpdate);
        }
    }
}