using BookingRoom.Models.GoogleCalendar;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Models.GoogleEvent
{
    public static class ToEventConverter
    {
        private static Event _newEvent;
        public static Event ToEvent(CalendarEvent eventForConverting)
        {
            _newEvent = new Event();
            _newEvent.Summary = eventForConverting.Summary;
            _newEvent.Description = eventForConverting.Description;
            _newEvent.Start = new EventDateTime()
            {
                DateTime = eventForConverting.Start.ToDateTime()
            };
            _newEvent.End = new EventDateTime()
            {
                DateTime = eventForConverting.End.ToDateTime()
            };
            return _newEvent;
        }
    }
}