using System.Collections.Generic;
using NUnit.Framework;
using Google.Apis.Calendar.v3.Data;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3;
using System.Linq;
using BookingRoom.Helpers;

namespace NUnitTestProject
{
    [TestFixture]
    public partial class BookingServiceTests 
    {
        [Test]
        public void PostEvent_WithWrongCalendarId_ThrowsException()
        {
            var start = new EventTime(2015, 10, 3, 15, 20, 0);
            var end = new EventTime(2015, 10, 3, 16, 20, 0);
            var eventToAdd = _eventFactory.CreateEvent(start, end, "NewSummary", "NewDescription");
            var calendarId = string.Empty;//Error

            Assert.Throws<Google.GoogleApiException>(
                () => { _bookingService.PostEvent(ToEventConverter.ToEvent(eventToAdd, _timezone), calendarId); });
        }

        [Test]
        public void PostEvent_WithWrongEventData_ThrowsException()
        {
            Event eventToAdd = null;
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            Assert.Throws<Google.GoogleApiException>(
                () => { _bookingService.PostEvent(eventToAdd, calendarId); });
        }

        [Test]
        public void PostEvent_WithValidInputData_PostsEventCorrectly()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
                new EventTime(2015, 10, 3, 15, 20, 0), new EventTime(2015, 10, 3, 16, 20, 0),
                "Nunit", "NunitDecription");

            _bookingService.PostEvent(ToEventConverter.ToEvent(eventToAdd, _timezone), calendarId);

            var testEvent = _bookingService.GetEvents(calendarId).Last();
           
            Assert.AreEqual(testEvent.Summary, eventToAdd.Summary);
            Assert.AreEqual(testEvent.Description, eventToAdd.Description);
            Assert.DoesNotThrow( () => _calendarService.Events.Delete(calendarId, testEvent.Id).Execute());
        }
    }
}