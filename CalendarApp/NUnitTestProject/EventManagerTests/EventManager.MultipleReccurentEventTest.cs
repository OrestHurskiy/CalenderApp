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
    public class MultipleEventConflictsTest : EventManagerTestClass
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent1 = _eventFactory.CreateEvent(
             new EventTime(2015, 6, 17, 10, 0, 0), new EventTime(2015, 6, 17, 12, 0, 0),
             "Nunit", "NunitDecription");
            conflictedEvent1.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent1, _timezone), calendarId);

            var conflictedEvent2 = _eventFactory.CreateEvent(
                new EventTime(2015, 6, 18, 15, 0, 0), new EventTime(2015, 6, 18, 19, 0, 0),
                "Nunit", "NunitDecription");
            conflictedEvent2.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent2, _timezone), calendarId);
        }
        [Test]
        public void EventManager_MultipleRecurrent_TouchedToTheLastEvent_ReturnsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 6, 17, 19, 0, 0), new EventTime(2015, 6, 17, 20, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] {RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5)};
            List<Event> conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
        [Test]
        public void EventManager_MultipleRecurrent_TouchedBeetwenTwoEvnts_ReturnsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 6, 17, 12, 0, 0), new EventTime(2015, 6, 17, 15, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
        [Test]
        public void EventManager_MultipleRecurrent_IntersectFirstTouchedSecon_ReturnsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 6, 15, 10, 0, 0), new EventTime(2015, 6, 15, 15, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 3) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }
        [Test]
        public void EventManager_MultipleRecurrent_OuterIntersection_ReturnsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 6, 15, 10, 0, 0), new EventTime(2015, 6, 15, 19, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 2);
        }
    }
}
