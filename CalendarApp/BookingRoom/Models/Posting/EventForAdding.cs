using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Models.Posting
{
    /// <summary>
    /// Class for Event Adding
    /// </summary>
    public class EventForAdding
    {
        /// <summary>
        /// Start Time of the Event
        /// </summary>
        public EventTime Start { get; set; }
        /// <summary>
        /// End Time of the event
        /// </summary>
        public EventTime End { get; set; }
        /// <summary>
        /// Title of the event
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// Description of the event
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ID of the calendar that will be added
        /// </summary>
        public string CalendarID { get; set; }
    }
}