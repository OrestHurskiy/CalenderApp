﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Models.GoogleEvent
{
    public class EventTime
    {
        public int Year { get; set; }
        public int Mounth { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public DateTime ToDateTime()
        {
            return new DateTime(Year, Mounth, Day, Hour, Minute, Second);
        }
    }
}