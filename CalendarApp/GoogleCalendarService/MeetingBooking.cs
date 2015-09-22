using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarService
{
    public class MeetingBooking
    {
        private CalendarService _service;
        private ILog _log;

        public MeetingBooking(CalendarService service, ILog log)
        {
            _service = service;
            _log = log;
        }

        public Event UpdateEvent(Event eventForUpdate, string calendarId, string eventId)
        {
            try
            {
                return _service.Events.Update(eventForUpdate, calendarId, eventId).Execute();
            }
            catch (Exception e)
            {
                _log.Error("Exception - \n"+e);
                throw e;
            }
        }

        public Event PostEvent(Event eventForAdd, string calendarId)
        {
            try
            {
                return _service.Events.Insert(eventForAdd, calendarId).Execute();
            }
            catch (Exception e)
            {
                _log.Error("Exception - \n" + e);
                throw e;
            }
        }

        public IList<Event> GetEvents(string calendarId)
        {
            try
            {
                return _service.Events.List(calendarId).Execute().Items;
            }
            catch (Exception e)
            {
                _log.Error("Exception - \n" + e);
                throw e;
            }
        }

        public void DeleteEvent(string calendarId, string eventId)
        {
            try
            {               
                _service.Events.Delete(calendarId, eventId).Execute();
            }
            catch (Exception e)
            {
                _log.Error("Exception - \n" + e);
                throw e;
            }
        }

        public Event SearchEventById(string calendarId, string eventId)
        {
            try
            {
                return _service.Events.List(calendarId).Execute().Items.SingleOrDefault(ev => ev.Id == eventId);
            }
            catch (Exception e)
            {
                _log.Error("Exception - \n" + e);
                throw e;
            }
        }
    }
}
