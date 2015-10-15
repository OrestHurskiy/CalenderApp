using System.Linq;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3;
using NUnit.Core;
using NUnit.Framework;
using BookingRoom.Helpers;

namespace NUnitTestProject
{
    [TestFixture]
    public partial class BookingServiceTests 
    {
        
        [Test]
        public void SearchEvent_WithWrongCalendarId_ThrowsException()
        {
            var eventId = "abqq3960anfn5cmi1242usco";
            var calendarId = string.Empty;//error

            Assert.Throws<Google.GoogleApiException>(
                () => { _bookingService.SearchEventById(calendarId, eventId); });
        }

        [Test]
        public void SearchEvent_WithValidInputData_SearchsEventCorrectly()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 6, 15, 20, 0), new EventTime(2015, 10, 6, 16, 20, 0),
               "Nunit", "NunitDecription");

            var testEvent = ToEventConverter.ToEvent(eventToAdd, _timezone);
            Assert.DoesNotThrow(() => _calendarService.Events.Insert(testEvent, calendarId).Execute());

            testEvent = _bookingService.GetEvents(calendarId).Last();
            var searchedEvent = _bookingService.SearchEventById(calendarId, testEvent.Id);
            Assert.IsNotNull(searchedEvent);
            Assert.AreEqual(searchedEvent.Id, testEvent.Id);
            Assert.AreEqual(searchedEvent.Summary, testEvent.Summary);
            Assert.AreEqual(searchedEvent.Description, testEvent.Description);
            Assert.DoesNotThrow(() => _calendarService.Events.Delete(calendarId, testEvent.Id).Execute());
        }

    }
}
