using BookingRoom.Helpers;
using BookingRoom.Models.GoogleEvent;
using GoogleCalendarService.Manager;
using NUnit.Framework;
using NUnitTestProject.Helpers;


namespace NUnitTestProject.EventManagerTests
{
    [TestFixture]
    public class YearlyEventConflictsTest : EventManagerTestClass
    {
        [SetUp]
        public void SetUp()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
                new EventTime(2015, 4, 19, 10, 0, 0), new EventTime(2015, 4, 19, 12, 0, 0),
                "Nunit", "NunitDecription");
            conflictedEvent.Recurrence = new[] {"RRULE:FREQ=YEARLY;COUNT=3"};
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
        public void EventManager_YearlyyRecurrent_Intersect_IntersectAllByhour_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2010, 4, 19, 10, 0, 0), new EventTime(2010, 4, 19, 12, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[]
            {RuleHelper.GetRuleString(FrequencyEnumaretion.YEARLY, FrequencyType.COUNT, 6)};
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_YearlyyRecurrent_IntersectleftByhour_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2010, 4, 19, 9, 0, 0), new EventTime(2010, 4, 19, 12, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[]
            {RuleHelper.GetRuleString(FrequencyEnumaretion.YEARLY, FrequencyType.COUNT, 6)};

            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_YearlyyRecurrent_IntersectRightByhour_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2010, 4, 19, 10, 0, 0), new EventTime(2010, 4, 19, 15, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[]
            {RuleHelper.GetRuleString(FrequencyEnumaretion.YEARLY, FrequencyType.COUNT, 7)};

            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_YearlyyRecurrent_IntersectInnerByhour_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2010, 4, 19, 10, 30, 0), new EventTime(2010, 4, 19, 11, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[]
            {RuleHelper.GetRuleString(FrequencyEnumaretion.YEARLY, FrequencyType.COUNT, 7)};

            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_YearlyyRecurrent_IntersectOuterByhour_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
              new EventTime(2010, 4, 19, 9, 0, 0), new EventTime(2010, 4, 19, 15, 0, 0),
              "Nunit", "NunitDecription");
            eventToAdd.Recurrence = new[]
            {RuleHelper.GetRuleString(FrequencyEnumaretion.YEARLY, FrequencyType.COUNT, 10)};

            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

    }
}
