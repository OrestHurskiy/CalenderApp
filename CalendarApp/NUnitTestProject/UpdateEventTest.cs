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
    class UpdateEventTest : BaseIntegrationTest
    {
        [Test]
        public void CheckingUpdating()
        {
            CalendarService googleService = _serviceLocator.Get<CalendarService>();
            string testCalendarId = System.Configuration.ConfigurationManager.AppSettings["TestCalendar"];
            IList<Event> beforeUpdateEventList = googleService.Events.List(testCalendarId).Execute().Items;

            Event testedEvent = (from gEvent in beforeUpdateEventList
                                 where gEvent.Id != null //couse we can post Event without id,for the present;
                                 select gEvent).First();

            if (testedEvent == null)
                throw new Exception("Cant find event for update"); //we need to know that there are no event for updating;

            string currentSummary = testedEvent.Summary; //remember current state
            string currentDesctiption = testedEvent.Description;

            testedEvent.Summary = "ChangedSummary"; //chenage event
            testedEvent.Description = "ChengedDescription";


            _meetingBooking.UpdateEvent(testedEvent,testCalendarId,testedEvent.Id);

            IList<Event> ufterApdateList = googleService.Events.List(testCalendarId).Execute().Items;

            Event testedAfterUpdateEvent = (from gEvent in ufterApdateList
                                            where gEvent.Id == testedEvent.Id
                                            select gEvent).First();

            if (testedAfterUpdateEvent == null)
                throw new Exception("Cant Find updated Event");

            Assert.AreEqual(testedEvent.Summary, testedAfterUpdateEvent.Summary); //we cant just Assert.AreEqual(two this object) couse of "updated" time field will be different anyway
            Assert.AreEqual(testedEvent.Description, testedAfterUpdateEvent.Description);

            testedAfterUpdateEvent.Summary = currentSummary;//push event back to past state
            testedAfterUpdateEvent.Description = currentDesctiption;

            _meetingBooking.UpdateEvent(testedAfterUpdateEvent, testCalendarId, testedAfterUpdateEvent.Id);

            Event checkEvent = (from gEvent in beforeUpdateEventList
                                where gEvent.Id == testedEvent.Id
                                select gEvent).Single();

            Assert.AreEqual(checkEvent.Summary,testedEvent.Summary);
            Assert.AreEqual(checkEvent.Description, testedEvent.Description);
        }

        [Test]
        public void Wrong_Input_Data_Must_Throw_Exception()
        {
            CalendarEvent eventForAdd = new CalendarEvent();
            CalendarService googleService = _serviceLocator.Get<CalendarService>();
            string testCalendarId = System.Configuration.ConfigurationManager.AppSettings["TestCalendar"];

            IList<Event> beforeUpdateEventList = googleService.Events.List(testCalendarId).Execute().Items;

            Event testedEvent = (from gEvent in beforeUpdateEventList
                                 where gEvent.Id != null //couse we can post Event without id,for the present;
                                 select gEvent).First();

            Assert.Throws(typeof(Google.GoogleApiException),
                delegate { _meetingBooking.UpdateEvent(testedEvent, string.Empty, testedEvent.Id); });
            Assert.Throws(typeof(Google.GoogleApiException),
                delegate { _meetingBooking.UpdateEvent(testedEvent, testCalendarId, string.Empty); });
        }
    }
}
