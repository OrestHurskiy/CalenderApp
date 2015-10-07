using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using NUnit.Framework;

namespace NUnitTestProject
{
    [TestFixture]
    public class DisplayEventTest : BaseIntegrationTest
    {
        [Test]
        public void Wrong_Input_Data_Must_Throw_Exception()
        {
            var calendarId = string.Empty;//error

            Assert.Throws(typeof(Google.GoogleApiException),
                delegate { _meetingBooking.GetEvents(calendarId); });
        }

        [Test]
        public void Checking_Displaying_Events()
        {
            var testCalendarId = System.Configuration.ConfigurationManager.AppSettings["TestCalendar"];
            var listOfEvents = _meetingBooking.GetEvents(testCalendarId);

            Assert.IsNotNull(listOfEvents);
            Assert.IsInstanceOf<IList<Event>>(listOfEvents);
        }
    }
}
