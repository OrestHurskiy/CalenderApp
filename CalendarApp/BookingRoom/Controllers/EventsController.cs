using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingRoom.Models;

namespace BookingRoom.Controllers
{
    public class EventsController : ApiController
    {
        private readonly CalendarConnection _connection;

        public EventsController()
        {
            _connection = new CalendarConnection();
        }

        public HttpResponseMessage GetEventsList()
        {
            var calendars = _connection.GoogleCalendar.CalendarList.List().Execute().Items;
            var eventsList = new List<EventDto>();

            foreach (var calendarEntry in calendars)
            {
                var events = _connection.GoogleCalendar.Events.List(calendarEntry.Id).Execute().Items;

                eventsList.AddRange(events.Select(eventEntry => new EventDto
                {
                    Id = eventEntry.Id,
                    Location = eventEntry.Location,
                    Summary = eventEntry.Summary,
                    Created = eventEntry.Created,
                    Description = eventEntry.Description,
                    Status = eventEntry.Status
                }));
            }

            return Request.CreateResponse(HttpStatusCode.OK, eventsList);
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

        [HttpGet]
        public HttpResponseMessage SearchEventByLocation(string location)
        {
            var calendars = _connection.GoogleCalendar.CalendarList.List().Execute().Items;
           
            foreach (var calendarEntry in calendars)
            {
                var events = _connection.GoogleCalendar.Events.List(calendarEntry.Id).Execute().Items;

                foreach (var eventEntry in events)
                {
                    var locationToCompare = string.Empty;

                    if (eventEntry.Location != null)
                    {
                        locationToCompare = eventEntry.Location.Replace(" ", string.Empty);

                        if (locationToCompare == location)
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
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}