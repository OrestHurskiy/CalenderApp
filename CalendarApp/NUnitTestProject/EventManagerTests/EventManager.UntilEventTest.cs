using System;
using BookingRoom.Helpers;
using BookingRoom.Models.GoogleEvent;
using GoogleCalendarService.Manager;
using NUnit.Framework;
using NUnitTestProject.Helpers;

namespace NUnitTestProject.EventManagerTests
{
    [TestFixture]
    public class EventManagerUntilEventTest : EventManagerTestClass
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
             new EventTime(2014, 6, 17, 10, 0, 0), new EventTime(2014, 6, 17, 12, 0, 0),
             "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);
        }

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
        public void EventManager_UntilEvent_IntersectOneDay_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2014, 6, 10, 10, 0, 0), new EventTime(2014, 6, 10, 12, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[]
            {
                RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.UNTIL, 0,
                    new DateTime(2014, 6, 17, 12, 0, 0))
            };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_UntilEvent_IntersecTwoDay_RetunsConflictEvents()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
             new EventTime(2014, 6, 15, 10, 0, 0), new EventTime(2014, 6, 15, 12, 0, 0),
             "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2014, 6, 10, 10, 0, 0), new EventTime(2014, 6, 10, 12, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[]
            {
                RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.UNTIL, 0,
                    new DateTime(2014, 6, 17, 12, 0, 0))
            };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 2);
        }

        [Test]
        public void EventManager_UntilEvent_IntersecOnTimeLine_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2014, 6, 10, 10, 0, 0), new EventTime(2014, 6, 10, 12, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[]
            {
                RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.UNTIL, 0,
                    new DateTime(2014, 6, 17, 9, 0, 0))
            };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
    }
}
