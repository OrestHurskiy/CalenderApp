using System.Collections.Generic;
using NUnit.Framework;
using Google.Apis.Calendar.v3.Data;
using System.Linq;
using BookingRoom.Helpers;
using BookingRoom.Models.GoogleEvent;

namespace NUnitTestProject
{
    [TestFixture]
    public partial class BookingServiceTests 
    {
        [Test]
        public void UpdateEvent_WithValidInputData_UpdatesEventCorrectly()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 6, 15, 20, 0), new EventTime(2015, 10, 6, 16, 20, 0),
               "Nunit", "NunitDecription");

            var testEvent = ToEventConverter.ToEvent(eventToAdd, _timezone);
            Assert.DoesNotThrow(() => _calendarService.Events.Insert(testEvent, calendarId).Execute());

            testEvent = _bookingService.GetEvents(calendarId).Last();
            testEvent.Description = $"Changed{testEvent.Description}";
            testEvent.Summary = $"Changed{testEvent.Summary}";

            _bookingService.UpdateEvent(testEvent, calendarId, testEvent.Id);

            var updatedEvent = _bookingService.GetEvents(calendarId).Last();
            Assert.AreEqual(updatedEvent.Description, testEvent.Description);
            Assert.AreEqual(updatedEvent.Summary, testEvent.Summary);
            Assert.DoesNotThrow(() => _calendarService.Events.Delete(calendarId, updatedEvent.Id).Execute());
        }

        [Test]
        public void UpdateEvent_WithWrongCalendarId_ThrowsException()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);
            var someEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 10, 0, 0), new EventTime(2015, 10, 17, 12, 0, 0),
               "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(someEvent, _timezone), calendarId);
            var beforeUpdateEventList = _bookingService.GetEvents(calendarId);

            var testEvent = beforeUpdateEventList[0];//take any event

            Assert.Throws<Google.GoogleApiException>(
                () => { _bookingService.UpdateEvent(testEvent, string.Empty, testEvent.Id); });
        }

        [Test]
        public void UpdateEvent_WithWrongEventId_ThrowsException()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var someEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 10, 0, 0), new EventTime(2015, 10, 17, 12, 0, 0),
               "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(someEvent, _timezone), calendarId);
            var beforeUpdateEventList = _bookingService.GetEvents(calendarId);

            var testEvent = beforeUpdateEventList[0];//take any event

            Assert.Throws<Google.GoogleApiException>(
                () => { _bookingService.UpdateEvent(testEvent, calendarId, string.Empty); });

        }

        [Test]
        public void UpdateEvent_WithWrongEventData_ThrowsException()
        {
            Event testEvent = null;//error
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);
            var eventId = "svvhrie7p05vrt5vi4f8j4ivlg";

            Assert.Throws<Google.GoogleApiException>(
                () => { _bookingService.UpdateEvent(testEvent, calendarId, eventId); });
        }
    }
}
