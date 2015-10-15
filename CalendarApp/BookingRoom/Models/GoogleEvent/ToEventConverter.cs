using System;
using BookingRoom.Models.GoogleCalendar;
using Google.Apis.Calendar.v3.Data;

namespace BookingRoom.Models.GoogleEvent
{
    public static class ToEventConverter
    {
        public static Event ToEvent(CalendarEvent eventForConverting,string timeZone)
        {
            Event newEvent = new Event
            {
                Recurrence = eventForConverting.Recurrence,
                Summary = eventForConverting.Summary,
                Description = eventForConverting.Description,
                Start = new EventDateTime
                {
                    TimeZone = timeZone,
                    DateTime = eventForConverting.Start.ToDateTime()
                },
                End = new EventDateTime()
                {
                    TimeZone = timeZone,
                    DateTime = eventForConverting.End.ToDateTime()
                }
            };
            return newEvent;
        }
    }
}