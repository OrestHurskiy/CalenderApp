using System.Collections.Generic;
using BookingRoom.Helpers;
using BookingRoom.Models.GoogleEvent;
using Google.Apis.Calendar.v3.Data;
using NUnit.Framework;

namespace NUnitTestProject
{
    [TestFixture]
    public class SignleEventConflictsTest : EventManagerTestClass
    {
        [SetUp]
        public void SetUp()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
              new EventTime(2015, 10, 17, 10, 0, 0), new EventTime(2015, 10, 17, 12, 0, 0),
              "Nunit", "NunitDecription");
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
        public void EventManager_SingleEvent_LeftIntersection_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 9, 0, 0), new EventTime(2015, 10, 17, 11, 0, 0),
               "Nunit", "NunitDecription");
            List<Event> conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_SingleEvent_InnerIntersection_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 10, 30, 0), new EventTime(2015, 10, 17, 11, 0, 0),
               "Nunit", "NunitDecription");
            List<Event> conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_SingleEvent_RightIntersection_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 10, 30, 0), new EventTime(2015, 10, 17, 13, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_SingleEvent_OuterIntersection_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 9, 0, 0), new EventTime(2015, 10, 17, 13, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_SingleEvent_OuterPlusInnerIntersection_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 10, 0, 0), new EventTime(2015, 10, 17, 12, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_SingleEvent_TouchedLeft_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 9, 0, 0), new EventTime(2015, 10, 17, 10, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }


        [Test]
        public void EventManager_SingleEvent_InnerIntersectionWithTouched_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 11, 0, 0), new EventTime(2015, 10, 17, 12, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_SingleEvent_IntersectTwoEvents_RetunsConflictEvents()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 15, 0, 0), new EventTime(2015, 10, 17, 16, 0, 0),
               "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 9, 0, 0), new EventTime(2015, 10, 17, 17, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 2);
        }

        [Test]
        public void EventManager_SingleEvent_IntersectFirst_TouchedSecond_RetunsConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 15, 0, 0), new EventTime(2015, 10, 17, 16, 0, 0),
               "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 9, 0, 0), new EventTime(2015, 10, 17, 15, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 1);
        }

        [Test]
        public void EventManager_SingleEvent_TouchedFirstInside_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 15, 0, 0), new EventTime(2015, 10, 17, 16, 0, 0),
               "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 12, 0, 0), new EventTime(2015, 10, 17, 13, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }

        [Test]
        public void EventManager_SingleEvent_BeetwenTwo_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 15, 0, 0), new EventTime(2015, 10, 17, 16, 0, 0),
               "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 13, 0, 0), new EventTime(2015, 10, 17, 14, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }

        [Test]
        public void EventManager_SingleEvent_TouchedTwoBeetwen_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 15, 0, 0), new EventTime(2015, 10, 17, 16, 0, 0),
               "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 12, 0, 0), new EventTime(2015, 10, 17, 15, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }

        [Test]
        public void EventManager_SingleEvent_TouchedSecondBeetwen_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 15, 0, 0), new EventTime(2015, 10, 17, 16, 0, 0),
               "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 14, 0, 0), new EventTime(2015, 10, 17, 15, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }

        [Test]
        public void EventManager_SingleEvent_OutWithoutIntersect_RetunsNoConflictEvent()
        {
            var calendarId = AppSettingsHelper.GetAppSetting(AppSetingsConst.TestCalendar);

            var conflictedEvent = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 15, 0, 0), new EventTime(2015, 10, 17, 16, 0, 0),
               "Nunit", "NunitDecription");
            _bookingService.PostEvent(ToEventConverter.ToEvent(conflictedEvent, _timezone), calendarId);

            var eventToAdd = _eventFactory.CreateEvent(
               new EventTime(2015, 10, 17, 18, 0, 0), new EventTime(2015, 10, 17, 19, 0, 0),
               "Nunit", "NunitDecription");
            var conflictedList = _eventManager.CheckEvent(calendarId, ToEventConverter.ToEvent(eventToAdd, _timezone));
            Assert.AreEqual(conflictedList.Count, 0);
        }
    }
}
