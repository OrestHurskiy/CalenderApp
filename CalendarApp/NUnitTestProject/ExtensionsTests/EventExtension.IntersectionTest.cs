using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingRoom.Models;
using BookingRoom.Models.GoogleEvent;
using GoogleCalendarService.Extensions;
using NUnit.Framework;

namespace NUnitTestProject.ExtensionsTests
{
    [TestFixture]
    public class EventExtensionTest
    {
        [Test]
        public void LeftIntersectionTest()
        {
            var _eventFactory = new EventFactory();
            var target = _eventFactory.CreateEvent(
                new EventTime(2015, 6, 17, 10, 0, 0), new EventTime(2015, 6, 17, 12, 0, 0),
                "Nunit", "NunitDecription");
            var source = _eventFactory.CreateEvent(
                new EventTime(2015, 6, 17, 9, 0, 0), new EventTime(2015, 6, 17, 11, 0, 0),
                "Nunit", "NunitDecription");
            bool result = ToEventConverter.ToEvent(source,null).
                LeftIntersection(ToEventConverter.ToEvent(target, null));
            Assert.AreEqual(result,true);
        }

        [Test]
        public void RightIntersectionTest()
        {
            var _eventFactory = new EventFactory();
            var target = _eventFactory.CreateEvent(
                new EventTime(2015, 6, 17, 10, 0, 0), new EventTime(2015, 6, 17, 12, 0, 0),
                "Nunit", "NunitDecription");
            var source = _eventFactory.CreateEvent(
                new EventTime(2015, 6, 17, 11, 0, 0), new EventTime(2015, 6, 17, 13, 0, 0),
                "Nunit", "NunitDecription");
            bool result = ToEventConverter.ToEvent(source, null).
                RightIntersection(ToEventConverter.ToEvent(target, null));
            Assert.AreEqual(result, true);
        }

        [Test]
        public void InnerIntersectionTest()
        {
            var _eventFactory = new EventFactory();
            var target = _eventFactory.CreateEvent(
                new EventTime(2015, 6, 17, 10, 0, 0), new EventTime(2015, 6, 17, 12, 0, 0),
                "Nunit", "NunitDecription");
            var source = _eventFactory.CreateEvent(
                new EventTime(2015, 6, 17, 10, 30, 0), new EventTime(2015, 6, 17, 11, 0, 0),
                "Nunit", "NunitDecription");
            bool result = ToEventConverter.ToEvent(source, null).
                InnerIntersection(ToEventConverter.ToEvent(target, null));
            Assert.AreEqual(result, true);
        }

        [Test]
        public void OuterIntersectionTest()
        {
            var _eventFactory = new EventFactory();
            var target = _eventFactory.CreateEvent(
                new EventTime(2015, 6, 17, 10, 0, 0), new EventTime(2015, 6, 17, 12, 0, 0),
                "Nunit", "NunitDecription");
            var source = _eventFactory.CreateEvent(
                new EventTime(2015, 6, 17, 9, 30, 0), new EventTime(2015, 6, 17, 13, 0, 0),
                "Nunit", "NunitDecription");
            bool result = ToEventConverter.ToEvent(source, null).
                OuterIntersection(ToEventConverter.ToEvent(target, null));
            Assert.AreEqual(result, true);
        }
    }
}
