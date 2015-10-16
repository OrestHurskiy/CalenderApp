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
            var eventForAdd = _eventFactory.CreateEvent(start, end, "NewSummary", "NewDescription");
            var calendarId = string.Empty;//Error

            Assert.Throws<Google.GoogleApiException>(
                () => { _meetingBooking.PostEvent(ToEventConverter.ToEvent(eventForAdd), calendarId); });
        }

        [Test]
        public void PostEvent_WithWrongEventData_ThrowsException()
        {
            Event eventForAdd = null;
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            Assert.Throws<Google.GoogleApiException>(
                () => { _meetingBooking.PostEvent(eventForAdd, calendarId); });
        }

        [Test]
        public void PostEvent_WithValidInputData_PostsEventCorrectly()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventForAdd = _eventFactory.CreateEvent(
                new EventTime(2015, 10, 3, 15, 20, 0), new EventTime(2015, 10, 3, 16, 20, 0),
                "Nunit", "NunitDecription");

            _meetingBooking.PostEvent(ToEventConverter.ToEvent(eventForAdd), calendarId);

            var request = _calendarService.Events.List(calendarId);
            request.SingleEvents = true;
            var testEvent = request.Execute().Items.Last();

            Assert.AreEqual(testEvent.Summary, eventForAdd.Summary);
            Assert.AreEqual(testEvent.Description, eventForAdd.Description);
            Assert.DoesNotThrow(() => _calendarService.Events.Delete(calendarId, testEvent.Id).Execute());
        }
    }
}
