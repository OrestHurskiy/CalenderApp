using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models;
using BookingRoom.Models.Displaying;
using BookingRoom.Models.GoogleConnection;

namespace BookingRoom.Controllers
{
    public class SearchController : EventController
    {
        public SearchController(ICalendarConnection connection)
            :base(connection)
        {

        }

        [HttpGet]
        public HttpResponseMessage SearchEventById(string id)
        {
            var calendars = _connection.GoogleCalendar.CalendarList.List().Execute().Items;

            foreach (var calendarEntry in calendars)
            {
                var events = _connection.GoogleCalendar.Events.List(calendarEntry.Id).Execute().Items;

                foreach (var eventEntry in events)
                {
                    if (eventEntry.Id == id)
                    {
                        var result = new EventDto
                        {
                            Id = eventEntry.Id,
                            Location = eventEntry.Location,
                            Summary = eventEntry.Summary,
                            Created = eventEntry.Created,
                            Description = eventEntry.Description,
                            Status = eventEntry.Status
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }

                }
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
    
}
