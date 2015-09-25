using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models;
using BookingRoom.Models.Displaying;

namespace BookingRoom.Controllers
{
    public class DisplayController : ApiController
    {
        private readonly CalendarConnection _connection;

        public DisplayController()
        {
            _connection = new CalendarConnection("1022042832033-glqi5vrlgh0gtpcdg620nkrg4hs65835@developer.gserviceaccount.com");
        }

        [HttpGet]
        public HttpResponseMessage DisplayEvents()
        {
            var calendars = _connection.GoogleCalendar.CalendarList.List().Execute().Items;
            var eventsToDisplay = new List<EventDto>();

            foreach (var calendarEntry in calendars)
            {
                var events = _connection.GoogleCalendar.Events.List(calendarEntry.Id).Execute().Items;
                eventsToDisplay.AddRange(events.Select(eventEntry => new EventDto
                {
                    Id = eventEntry.Id,
                    Location = eventEntry.Location,
                    Summary = eventEntry.Summary,
                    Created = eventEntry.Created,
                    Description = eventEntry.Description,
                    Status = eventEntry.Status
                }));
            }

            return Request.CreateResponse(HttpStatusCode.OK, eventsToDisplay);
        }

    }
}
