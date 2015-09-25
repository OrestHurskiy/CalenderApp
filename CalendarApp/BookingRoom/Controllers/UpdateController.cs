using BookingRoom.Models;
using BookingRoom.Models.Posting;
using Google.Apis.Requests;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace BookingRoom.Controllers
{
    public class UpdateController : ApiController
    {
        private readonly CalendarConnection _connection;
        public UpdateController()
        {
            _connection = new CalendarConnection("1022042832033-glqi5vrlgh0gtpcdg620nkrg4hs65835@developer.gserviceaccount.com");
        }
        [HttpPut]
        public HttpResponseMessage UpDateEvent(CalendarEvent eventForUpdate,string eventID)
        {
            Event updateEvent = eventForUpdate.ToEvent();
            /*_connection.GoogleCalendar.Events.Update(updateEvent, eventForUpdate.CalendarID, eventID).Execute();*/
            ClientServiceRequest<Event> request = _connection.GoogleCalendar.Events.Update(updateEvent, eventForUpdate.CalendarID, eventID);
            try
            {
                request.Execute();
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,eventForUpdate);
            }                          
            return Request.CreateResponse(HttpStatusCode.OK,eventForUpdate);          
        }
    }
}