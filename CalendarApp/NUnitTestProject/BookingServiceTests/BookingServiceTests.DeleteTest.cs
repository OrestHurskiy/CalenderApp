using System.Linq;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3;
using NUnit.Framework;
using BookingRoom.Helpers;
using Microsoft.Practices.Unity;
using GoogleCalendarService;
using BookingRoom.Models;
using GoogleCalendarService.Manager;

namespace NUnitTestProject
{
    [TestFixture]
    public partial class BookingServiceTests
    {
        private CalendarService _calendarService;
        private IBookingService _bookingService;
        private EventFactory _eventFactory;
        private string _timezone;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var _unityContainer = Dependency.UnityConfig.GetUnityContainer();

            _calendarService = _unityContainer.Resolve<CalendarService>();
            _bookingService = _unityContainer.Resolve<IBookingService>();
            _eventFactory = _unityContainer.Resolve<EventFactory>();

            var calendars = _calendarService.CalendarList.List().Execute();
            _timezone = calendars.Items[0].TimeZone;
        }
        [SetUp]
        public void SetUp()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
                new EventTime(2014, 6, 17, 10, 0, 0), new EventTime(2014, 6, 17, 12, 0, 0),
                "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);
        }

        [TearDown]
        public void TearDown()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);
            var eventList = _bookingService.GetEvents(calendarId);
            foreach (var e in eventList)
            {
                _calendarService.Events.Delete(calendarId, e.Id).Execute();
            }
        }
        [Test]
        public void DeleteEvent_WithWrongCalendarId_ThrowsException()
        {
            var eventId = "abqq3960anfn5cmi1242usco";
            var calendarId = string.Empty;//error

            Assert.Throws<Google.GoogleApiException>(
                () => { _bookingService.DeleteEvent(calendarId, eventId); });
        }

        [Test]
        public void DeleteEvent_WithWrongEventId_ThrowsException()
        {
            var eventId = string.Empty;//error
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            Assert.Throws<Google.GoogleApiException>(
                () => { _bookingService.DeleteEvent(calendarId, eventId); });
        }

        [Test]
        public void DeleteEvent_WithValidInputData_DeletesEventCorrectly()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 6, 15, 20, 0), new EventTime(2015, 10, 6, 16, 20, 0),
               "Nunit", "NunitDecription");

            var testEvent = ToEventConverter.ToEvent(eventToAdd, _timezone);
            Assert.DoesNotThrow( () => _calendarService.Events.Insert(testEvent, calendarId).Execute());
               
            testEvent = _bookingService.GetEvents(calendarId).Last();
            _bookingService.DeleteEvent(calendarId, testEvent.Id);

            var deletedEvent =
                _calendarService.Events.List(calendarId)
                    .Execute()
                    .Items.SingleOrDefault(ev => ev.Id == testEvent.Id);

            Assert.IsNull(deletedEvent);
        }
    }
}
