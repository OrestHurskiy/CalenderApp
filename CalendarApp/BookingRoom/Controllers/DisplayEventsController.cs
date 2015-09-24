using BookingRoom.Models;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookingRoom.Controllers
{
    public class DisplayEventsController : ApiController
    {
        private readonly CalendarConnection _connection;

        public DisplayEventsController()
        {
            _connection = new CalendarConnection();
        }

        public IList<EventDto> GetList()
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

            return eventsToDisplay;
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
            EventDto result = new EventDto();

            foreach (var calendarEntry in calendars)
            {
                var events = _connection.GoogleCalendar.Events.List(calendarEntry.Id).Execute().Items;
                foreach (var eventEntry in events)
                {
                    string locationToCompare = string.Empty;

                    if (eventEntry.Location != null)
                    {
                        locationToCompare = eventEntry.Location.Replace(" ", string.Empty);

                        if (locationToCompare == location)
                        {
                            result.Id = eventEntry.Id;
                            result.Location = eventEntry.Location;
                            result.Summary = eventEntry.Summary;
                            result.Created = eventEntry.Created;
                            result.Status = eventEntry.Status;

                            return Request.CreateResponse(HttpStatusCode.OK, result);
                        }
                    }
                   

                }

            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        
    }
}
