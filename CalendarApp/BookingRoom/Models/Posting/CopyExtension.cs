using BookingRoom.Models.Posting.BookingRoom.Models.Posting;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Models.Posting
{
    public static class CopyExtension
    {
        public static void EventCopy(this Event toAddEvent, EventForAdding forAddingEvent)
        {
            toAddEvent.Summary = forAddingEvent.Summary;
            toAddEvent.Description = forAddingEvent.Description;
            toAddEvent.Start = new EventDateTime()
            {
                DateTime = new DateTime(forAddingEvent.Start.Year, forAddingEvent.Start.Mounth, forAddingEvent.Start.Day, forAddingEvent.Start.Hour, forAddingEvent.Start.Minute, forAddingEvent.Start.Second)
            };
            toAddEvent.End = new EventDateTime()
            {
                DateTime = new DateTime(forAddingEvent.End.Year, forAddingEvent.End.Mounth, forAddingEvent.End.Day, forAddingEvent.End.Hour, forAddingEvent.End.Minute, forAddingEvent.End.Second)
            };

        }
    }
}