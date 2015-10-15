using System.Web.Http;
using GoogleCalendarService;
using GoogleCalendarService.Manager;

namespace BookingRoom.Controllers
{
    public class BaseEventController : ApiController
    {
        protected IBookingService BookingService;
        protected IEventManager EventManager;

        public BaseEventController(IBookingService bookingService, IEventManager eventManager)
        {
            BookingService = bookingService;
            EventManager = eventManager;
        }
    }
}
