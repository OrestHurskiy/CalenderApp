using log4net;
using System;
using System.Web.Http;
using GoogleCalendarService;
namespace BookingRoom.Controllers
{
    public class EventController : ApiController
    {
        protected MeetingBooking _connection;
        public EventController(MeetingBooking connection)
        {
            _connection = connection;
        }
    }
}
