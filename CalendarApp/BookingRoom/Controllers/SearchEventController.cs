using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using BookingRoom.Filters;
using Google.Apis.Calendar.v3;
using GoogleCalendarService.Manager;

namespace BookingRoom.Controllers
{
    [GoogleExceptionFilter]
    public class SearchEventController : BaseEventController
    {
        public SearchEventController(IBookingService bookingService, IEventManager manager)
            :base(bookingService, manager)
        {

        }

        [HttpGet]
        public HttpResponseMessage SearchEventById(string eventId, string calendarId)
        {
            Event searchedEvent;
            searchedEvent = BookingService.SearchEventById(calendarId, eventId);
            return Request.CreateResponse(HttpStatusCode.OK,searchedEvent);
        }
    }
    
}
