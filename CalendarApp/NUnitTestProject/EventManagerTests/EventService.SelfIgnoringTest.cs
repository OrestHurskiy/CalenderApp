using System.Collections.Generic;
using BookingRoom.Helpers;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3.Data;
using NUnit.Framework;


namespace NUnitTestProject.EventManagerTests
{
    [TestFixture]
    public class EventManagerSelfIgnoringTest : EventManagerTestClass
    {
        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);
            var eventList = _bookingService.GetEvents(calendarId);
            foreach (var e in eventList)
            {
                _calendarService.Events.Delete(calendarId, e.Id).Execute();
            }
        }

        [Test]
        public void EventManager_SelfIgnoringTest()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
                new EventTime(2015, 2, 17, 10, 0, 0), new EventTime(2015, 2, 17, 12, 0, 0),
                "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToUpdate = ToEventConverter.ToEvent(_eventFactory.CreateEvent(
                new EventTime(2015, 2, 17, 8, 0, 0), new EventTime(2015, 2, 17, 10, 0, 0),
                "Nunit", "NunitDecription"), _timezone);
            _bookingService.PostEvent(eventToUpdate, calendarId);

            IList<Event> eventList = _bookingService.GetEvents(calendarId);

            eventToUpdate = ToEventConverter.ToEvent(_eventFactory.CreateEvent(
                new EventTime(2015, 2, 17, 7, 0, 0), new EventTime(2015, 2, 17, 8, 30, 0),
                "Nunit", "NunitDecription"), _timezone);
            eventToUpdate.Id = eventList[eventList.Count - 1].Id;
            List<Event> conflictedList = _eventManager.CheckEvent(calendarId, eventToUpdate);
            Assert.AreEqual(conflictedList.Count, 0);
        }
    }
}
