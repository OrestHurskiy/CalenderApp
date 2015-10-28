using System;
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
    public class WeeklyEventConflictsTest : EventManagerTestClass
    {
        [SetUp]
        public void SetUp()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
                new EventTime(2015, 4, 19, 10, 0, 0), new EventTime(2015, 4, 19, 12, 0, 0),
                "Nunit", "NunitDecription");
            conflictedEvent.Recurrence = new[] {RuleHelper.GetRuleString(FrequencyEnumaretion.WEEKLY, FrequencyType.COUNT, 6, new DateTime(), DayOfWeek.Sunday, DayOfWeek.Friday)};
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);
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
        public void EventManager_WeaklyRecurrent_InterSectionAllDay_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 4, 17, 10, 0, 0), new EventTime(2015, 4, 17, 15, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.WEEKLY, FrequencyType.COUNT, 5, new DateTime(), DayOfWeek.Friday, DayOfWeek.Sunday)};
            List<Event> conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }
        [Test]
        public void EventManager_WeaklyRecurrent_OneDayIntersection_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 4, 16, 10, 0, 0), new EventTime(2015, 4, 16, 15, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.WEEKLY, FrequencyType.COUNT, 3, new DateTime(), DayOfWeek.Thursday)};
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
        [Test]
        public void EventManager_WeaklyRecurrent_TwoEventsIntersection_RetunsConflictEvents()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
                new EventTime(2015, 4, 20, 12, 0, 0), new EventTime(2015, 4, 20, 13, 0, 0),
                "Nunit", "NunitDecription");
            conflictedEvent.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.WEEKLY, FrequencyType.COUNT, 6, new DateTime(), DayOfWeek.Monday, DayOfWeek.Friday)};
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 4, 17, 10, 0, 0), new EventTime(2015, 4, 17, 15, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.WEEKLY, FrequencyType.COUNT, 6, new DateTime(), DayOfWeek.Friday)};
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 2);
        }
    }
}
