using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using BookingRoom.Filters;

namespace BookingRoom.Controllers
{
    [GoogleExceptionFilter]
    public class SearchEventController : BaseEventController
    {
        public SearchEventController(BookingService bookingService)
            :base(bookingService)
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
