using System;
using NUnit.Framework;
using NUnitTestProject.Common;
using Microsoft.Practices.Unity;
using GoogleCalendarService;
using Google.Apis.Calendar.v3.Data;
using BookingRoom.Models.GoogleCalendar;
using BookingRoom.Models.GoogleEvent;

namespace NUnitTestProject
{
    [TestFixture]
    public class BaseIntegrationTest
    {
        protected IServiceLocator _serviceLocator;
        protected IUnityContainer _unityContainer;
        protected MeetingBooking _meetingBooking;

        [TestFixtureSetUp]
        public void Initialize()
        {           
            _unityContainer = Dependency.UnityConfig.GetUnityContainer();
            _serviceLocator = _unityContainer.Resolve<IServiceLocator>();
            _meetingBooking = _serviceLocator.Get<MeetingBooking>();
        }
        protected CalendarEvent CreateEvent(EventTime start,EventTime end,string summary,string desctiption)
        {
            CalendarEvent newEvent = new CalendarEvent();
            newEvent.Start = start;
            newEvent.End = end;
            newEvent.Summary = summary;
            newEvent.Description = desctiption;
            return newEvent;
        }

    }
}