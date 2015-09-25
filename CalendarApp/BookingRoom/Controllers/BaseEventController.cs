using System.Web.Http;
using GoogleCalendarService;
namespace BookingRoom.Controllers
{
    public class BaseEventController : ApiController
    {
        protected BookingService BookingService;

        public BaseEventController(BookingService bookingService)
        {
            BookingService = bookingService;
        }
    }
}
