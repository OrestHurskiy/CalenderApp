using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3;
using NUnit.Core;
using NUnit.Framework;
using Event = Google.Apis.Calendar.v3.Data.Event;

namespace NUnitTestProject
{
    [TestFixture]
    public class SearchEventTest : BaseIntegrationTest
    {
        private Event _testEvent;
        private string _testCalendarId;
        private CalendarService _googleService;

        private void Init()
        {
            _googleService = _serviceLocator.Get<CalendarService>();
            _testCalendarId = System.Configuration.ConfigurationManager.AppSettings["TestCalendar"];

            var eventForAdd = CreateEvent(
               new EventTime(2015, 10, 6, 15, 20, 0), new EventTime(2015, 10, 6, 16, 20, 0),
               "Nunit", "NunitDecription");

            _testEvent = ToEventConverter.ToEvent(eventForAdd);
            _googleService.Events.Insert(_testEvent, _testCalendarId).Execute();
        }

        private void Dispose()
        {
            _googleService.Events.Delete(_testCalendarId, _testEvent.Id).Execute();
            _googleService.Dispose();
        }

        [Test]
        public void Wrong_Input_Data_Must_Throw_Exception()
        {
            var eventId = "abqq3960anfn5cmi1242usco";
            var calendarId = string.Empty;//error

            Assert.Throws(typeof(Google.GoogleApiException),
                delegate { _meetingBooking.SearchEventById(calendarId, eventId); });
        }

        [Test]
        public void Check_Searching_Event()
        {
            Init();
            _testEvent = _googleService.Events.List(_testCalendarId).Execute().Items.Last();
            var searchedEvent = _meetingBooking.SearchEventById(_testCalendarId, _testEvent.Id);
            Assert.IsNotNull(searchedEvent);
            Assert.AreEqual(searchedEvent.Id, _testEvent.Id);
            Assert.AreEqual(searchedEvent.Summary, _testEvent.Summary);
            Assert.AreEqual(searchedEvent.Description, _testEvent.Description);
            Dispose();
        }

    }
}
