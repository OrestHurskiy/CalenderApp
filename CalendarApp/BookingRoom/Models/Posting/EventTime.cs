using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Models.Posting
{
    public class EventTime
    {
        public int Year { get; set; }
        public int Mounth { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
    }
}