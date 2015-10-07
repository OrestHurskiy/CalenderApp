using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingRoom.Models.GoogleCalendar;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using NUnit.Core;
using NUnit.Framework;
using Event = Google.Apis.Calendar.v3.Data.Event;

namespace NUnitTestProject
{
    [TestFixture]
    public class DeleteEventTest : BaseIntegrationTest
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
            _googleService.Dispose();
        }

        [Test]
        public void Wrong_Input_Data_Must_Throw_Exception()
        {
            var eventId = "abqq3960anfn5cmi1242usco";
            var calendarId = string.Empty;//error

            Assert.Throws(typeof(Google.GoogleApiException),
                delegate { _meetingBooking.DeleteEvent(eventId, calendarId); });
        }

        [Test]
        public void Checking_Deleting_Event()
        {
            Init();
            _testEvent = _googleService.Events.List(_testCalendarId).Execute().Items.Last();
            _meetingBooking.DeleteEvent(_testCalendarId, _testEvent.Id);

            var deletedEvent =
                _googleService.Events.List(_testCalendarId)
                    .Execute()
                    .Items.SingleOrDefault(ev => ev.Id == _testEvent.Id);

            Assert.IsNull(deletedEvent);
            Dispose();
        }
    }
}
