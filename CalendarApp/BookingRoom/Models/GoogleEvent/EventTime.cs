
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Models.GoogleEvent
{
    public class EventTime
    {
        public int Year { get; private set; }
        public int Mounth { get; private set; }
        public int Day { get; private set; }
        public int Hour { get; private set; }
        public int Minute { get; private set; }
        public int Second { get; private set; }

        public EventTime(int year, int mouth, int day, int hour, int minute, int second)
        {
            Year = year;
            Mounth = mouth;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Mounth, Day, Hour, Minute, Second);
        }
    }
}