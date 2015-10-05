using System;
using NUnit.Framework;
using NUnitTestProject.Common;
using Microsoft.Practices.Unity;
using GoogleCalendarService;
using System.Collections.Generic;
using Google.Apis.Calendar.v3.Data;
using BookingRoom.Models.GoogleCalendar;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3;
using System.Linq;

namespace NUnitTestProject
{
    [TestFixture]
    public class PostEventTest : BaseIntegrationTest
    {
        [Test]
        public void Wrong_Inserted_Data_Should_Throw_Exception()
        {
            EventTime start = new EventTime(2015, 10, 3, 15, 20, 0);
            EventTime end = new EventTime(2015, 10, 3, 16, 20, 0);
            CalendarEvent eventForAdd = CreateEvent(start, end, "NewSummary", "NewDescription");
            eventForAdd.CalendarID = string.Empty;//Error
                     
            Assert.Throws(typeof(Google.GoogleApiException),
                delegate { _meetingBooking.PostEvent(ToEventConverter.ToEvent(eventForAdd), eventForAdd.CalendarID); });
        }

        [Test]
        public void Check_Posting_Event()
        {
            CalendarService googleService = _serviceLocator.Get<CalendarService>();

            CalendarEvent eventForAdd = CreateEvent(
                new EventTime(2015, 10, 3, 15, 20, 0), new EventTime(2015, 10, 3, 16, 20, 0),
                "Nunit","NunitDecription");
            eventForAdd.CalendarID = System.Configuration.ConfigurationManager.AppSettings["TestCalendar"];
            IList<Event> beforeAddList = googleService.Events.List(eventForAdd.CalendarID).Execute().Items;

            _meetingBooking.PostEvent(ToEventConverter.ToEvent(eventForAdd), eventForAdd.CalendarID);

            IList<Event> afterAddList = googleService.Events.List(eventForAdd.CalendarID).Execute().Items;
            var addedEvent = from gEvent in afterAddList
                             where (gEvent.Summary == eventForAdd.Summary) &&
                             (gEvent.Start == ToEventConverter.ToEvent(eventForAdd).Start) &&
                             (gEvent.End == ToEventConverter.ToEvent(eventForAdd).End)
                             select gEvent;

            Assert.AreEqual(beforeAddList.Count + 1, afterAddList.Count);
            Assert.NotNull(addedEvent);
        }
    }
}
