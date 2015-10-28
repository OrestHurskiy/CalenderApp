﻿using Google.Apis.Calendar.v3.Data;
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
                 () => { _bookingService.GetEvents(calendarId); });
        }


        [Test]
        public void DisplayEvents_WithValidCalendarId_DisplaysEventsCorrectly()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var actualEvents = _calendarService.Events.List(calendarId).Execute().Items;
            var expectedEvents = _bookingService.GetEvents(calendarId);

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
