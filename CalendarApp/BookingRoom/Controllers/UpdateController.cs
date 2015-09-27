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
using BookingRoom.Models.GoogleConnection;
using BookingRoom.Models.GoogleEvent;
using log4net;

namespace BookingRoom.Controllers
{
    public class UpdateController : EventController
    {
        public UpdateController(IGoogleCalendarService connection,ILog logger)
            : base(connection,logger)
        {

        }
        [HttpPut]
        public HttpResponseMessage UpDateEvent(CalendarEvent eventForUpdate,string eventID)
        {
            Event updateEvent = ToEventConverter.ToEvent(eventForUpdate);        
            try
            {
                _connection.GoogleCalendar.Events.Update(updateEvent, eventForUpdate.CalendarID, eventID).Execute();
            }
            catch(Exception e)
            {
                log.Error("Exception -\n" + e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }                          
            return Request.CreateResponse(HttpStatusCode.OK,eventForUpdate);          
        }
    }
}