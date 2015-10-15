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
    class SignleEventConflictsTest : EventManagerTestClass
    {
        [SetUp]
        public void SetUp()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 7, 17, 10, 0, 0), new EventTime(2015, 7, 17, 12, 0, 0),
               "Nunit", "NunitDecription");
            conflictedEvent.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
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
        public void EventManager_SingleRecurrent_InnerPlusOuterIntersection_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 7, 17, 10, 0, 0), new EventTime(2015, 7, 17, 12, 0, 0),
               "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            List<Event> conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }
        [Test]
        public void EventManager_SingleRecurrent_InnerIntersection_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
             new EventTime(2015, 7, 18, 10, 0, 0), new EventTime(2015, 7, 18, 12, 0, 0),
             "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 2) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }
        [Test]
        public void EventManager_SingleRecurrent_TouchedLeft_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 15, 10, 0, 0), new EventTime(2015, 7, 15, 12, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 2) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
        [Test]
        public void EventManager_SingleRecurrent_LeftIntersection_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 15, 10, 0, 0), new EventTime(2015, 7, 15, 12, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 3) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }
        [Test]
        public void EventManager_SingleRecurrent_RightIntersection_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 20, 10, 0, 0), new EventTime(2015, 7, 20, 12, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 3) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }
        [Test]
        public void EventManager_SingleRecurrent_OutRight_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 22, 10, 0, 0), new EventTime(2015, 7, 22, 12, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 3) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
        [Test]
        public void EventManager_SingleRecurrent_InnerIntersectionNotThatOur_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 17, 9, 0, 0), new EventTime(2015, 7, 17, 10, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
        [Test]
        public void EventManager_SingleRecurrent_LeftIntersectionByhours_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
             new EventTime(2015, 7, 17, 9, 0, 0), new EventTime(2015, 7, 17, 11, 0, 0),
             "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }
        [Test]
        public void EventManager_SingleRecurrent_RightOutByHours_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 17, 12, 0, 0), new EventTime(2015, 7, 17, 13, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
        [Test]
        public void EventManager_SingleRecurrent_LeftIntersectionBeetweenTwo_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var simpleEvent = _eventFactory.CreateEvent(
                new EventTime(2015, 7, 19, 16, 0, 0), new EventTime(2015, 7, 19, 18, 0, 0),
                "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(simpleEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 17, 10, 30, 0), new EventTime(2015, 7, 17, 11, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }
        [Test]
        public void EventManager_SingleRecurrent_RightInterSectionByHours_RetunsConflictEvents()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var simpleEvent = _eventFactory.CreateEvent(
                new EventTime(2015, 7, 19, 16, 0, 0), new EventTime(2015, 7, 19, 18, 0, 0),
                "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(simpleEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 17, 9, 0, 0), new EventTime(2015, 7, 17, 19, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 2);
        }
        [Test]
        public void EventManager_SingleRecurrent_LeftIntersecionByHours_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var simpleEvent = _eventFactory.CreateEvent(
                new EventTime(2015, 7, 19, 16, 0, 0), new EventTime(2015, 7, 19, 18, 0, 0),
                "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(simpleEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 17, 9, 0, 0), new EventTime(2015, 7, 17, 13, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }
        [Test]
        public void EventManager_SingleRecurrent_BeetweenTwo_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var simpleEvent = _eventFactory.CreateEvent(
                new EventTime(2015, 7, 19, 16, 0, 0), new EventTime(2015, 7, 19, 18, 0, 0),
                "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(simpleEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2015, 7, 17, 12, 0, 0), new EventTime(2015, 7, 17, 13, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[] { RuleHelper.GetRuleString(FrequencyEnumaretion.DAILY, FrequencyType.COUNT, 5) };
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
    }
}
