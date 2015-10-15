using System.Linq;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3;
using NUnit.Core;
using NUnit.Framework;
using BookingRoom.Helpers;
using Microsoft.Practices.Unity;
using GoogleCalendarService;
using BookingRoom.Models;

namespace NUnitTestProject
{
    [TestFixture]
    public partial class BookingServiceTests
    {
        private CalendarService _calendarService;
        private BookingService _meetingBooking;
        private EventFactory _eventFactory;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var _unityContainer = Dependency.UnityConfig.GetUnityContainer();
            _calendarService = _unityContainer.Resolve<CalendarService>();
            _meetingBooking = _unityContainer.Resolve<BookingService>();
            _eventFactory = _unityContainer.Resolve<EventFactory>();
        }

        [Test]
        public void DeleteEvent_WithWrongCalendarId_ThrowsException()
        {
            var eventId = "abqq3960anfn5cmi1242usco";
            var calendarId = string.Empty;//error

            Assert.Throws<Google.GoogleApiException>(
                () => { _meetingBooking.DeleteEvent(calendarId, eventId); });
        }

        [Test]
        public void DeleteEvent_WithWrongEventId_ThrowsException()
        {
            var eventId = string.Empty;//error
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            Assert.Throws<Google.GoogleApiException>(
                () => { _meetingBooking.DeleteEvent(calendarId, eventId); });
        }

        [Test]
        public void DeleteEvent_WithValidInputData_DeletesEventCorrectly()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventForAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 6, 15, 20, 0), new EventTime(2015, 10, 6, 16, 20, 0),
               "Nunit", "NunitDecription");

            var testEvent = ToEventConverter.ToEvent(eventForAdd);
            Assert.DoesNotThrow( () => _calendarService.Events.Insert(testEvent, calendarId).Execute());
               
            testEvent = _calendarService.Events.List(calendarId).Execute().Items.Last();
            _meetingBooking.DeleteEvent(calendarId, testEvent.Id);

            var deletedEvent =
                _calendarService.Events.List(calendarId)
                    .Execute()
                    .Items.SingleOrDefault(ev => ev.Id == testEvent.Id);

            Assert.IsNull(deletedEvent);

        }
    }
}
