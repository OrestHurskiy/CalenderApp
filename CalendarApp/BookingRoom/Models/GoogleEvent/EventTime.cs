using System;

namespace BookingRoom.Models.GoogleEvent
{
    public class EventTime
    {
        public int Year { get; }
        public int Mounth { get; }
        public int Day { get; }
        public int Hour { get; }
        public int Minute { get; }
        public int Second { get; }

        public EventTime(int year, int mounth, int day, int hour, int minute, int second)
        {
            Year = year;
            Mounth = mounth;
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