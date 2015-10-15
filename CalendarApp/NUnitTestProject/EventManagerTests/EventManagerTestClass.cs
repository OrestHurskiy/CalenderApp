using BookingRoom.Helpers;
using Google.Apis.Calendar.v3;
using Microsoft.Practices.Unity;
using GoogleCalendarService;
using BookingRoom.Models;
using GoogleCalendarService.Manager;
using NUnit.Framework;

namespace NUnitTestProject
{
    public class EventManagerTestClass
    {
        protected CalendarService _calendarService;
        protected IBookingService _bookingService;
        protected EventFactory _eventFactory;
        protected IEventManager _eventManager;
        protected string _timezone;

        public EventManagerTestClass()
        {
            var _unityContainer = Dependency.UnityConfig.GetUnityContainer();

            _calendarService = _unityContainer.Resolve<CalendarService>();
            _bookingService = _unityContainer.Resolve<IBookingService>();
            _eventFactory = _unityContainer.Resolve<EventFactory>();
            _eventManager = _unityContainer.Resolve<IEventManager>();

            var calendars = _calendarService.CalendarList.List().Execute();
            _timezone = calendars.Items[0].TimeZone;
        }
    }
}
