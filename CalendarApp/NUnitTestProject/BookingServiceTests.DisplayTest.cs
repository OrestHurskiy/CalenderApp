using Google.Apis.Calendar.v3.Data;
using NUnit.Framework;
using BookingRoom.Helpers;
using Google.Apis.Calendar.v3;

namespace NUnitTestProject
{
    [TestFixture]
    public partial class BookingServiceTests
    {
        [Test]
        public void DisplayEvents_WithWrongCalendarId_ThrowsException()
        {
            var calendarId = string.Empty;//error

            Assert.Throws<Google.GoogleApiException>(
                 () => { _meetingBooking.GetEvents(calendarId); });
        }


        [Test]
        public void DisplayEvents_WithValidCalendarId_DisplaysEventsCorrectly()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var request = _calendarService.Events.List(calendarId);
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            var actualEvents = request.Execute().Items;
            var expectedEvents = _meetingBooking.GetEvents(calendarId);

            CollectionAssert.AllItemsAreNotNull(expectedEvents);
            CollectionAssert.AllItemsAreInstancesOfType(expectedEvents, typeof(Event));

            for (var index = 0; index < actualEvents.Count; index++)
            {
                var expectedEvent = expectedEvents[index];
                var actualEvent = actualEvents[index];

                Assert.AreEqual(expectedEvent.Id, actualEvent.Id);
                Assert.AreEqual(expectedEvent.Summary, actualEvent.Summary);
                Assert.AreEqual(expectedEvent.Description, actualEvent.Description);
            }
        }
    }
}
