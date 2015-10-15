using System.Collections.Generic;
using BookingRoom.Helpers;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService.Manager;
using NUnit.Framework;
using NUnitTestProject.Helpers;

namespace NUnitTestProject.EventManagerTests
{
    [TestFixture]
    public class MonthlyEventConflictsTest : EventManagerTestClass
    {
        [SetUp]
        public void SetUp()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
                new EventTime(2015, 4, 19, 10, 0, 0), new EventTime(2015, 4, 19, 12, 0, 0),
                "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var conflictedEvent2 = _eventFactory.CreateEvent(
                new EventTime(2015, 4, 19, 12, 0, 0), new EventTime(2015, 4, 19, 12, 0, 0),
                "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent2, _timezone), calendarId);

        }

        [TearDown]
        public void TearDown()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);
            var eventList = _bookingService.GetEvents(calendarId);
            foreach (var e in eventList)
            {
                _calendarService.Events.Delete(calendarId, e.Id).Execute();
            }
        }

        [Test]
        public void EventManager_MonthlyRecurrent_TwoDaysIntersection_ReturnsConflictedEvents()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 1, 19, 10, 0, 0), new EventTime(2015, 1, 19, 15, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] {RuleHelper.GetRuleString(FrequencyEnumaretion.MONTHLY, FrequencyType.COUNT, 10)};
            List<Event> conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 2);
        }
    }
}
