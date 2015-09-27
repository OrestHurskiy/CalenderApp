using Google.Apis.Calendar.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingRoom.Models.GoogleConnection
{
    public interface IGoogleCalendarService
    {
        CalendarService GoogleCalendar { get; }
    }
}
