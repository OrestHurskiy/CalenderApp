using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleCalendarService
{
    public class BookingService
    {
        private CalendarService _service;
        private ILog _log;

        public BookingService(CalendarService service, ILog log)
        {
            _service = service;
            _log = log;
        }

        public BookingService(CalendarService service)
        {
            _service = service;
        }
        public Event UpdateEvent(Event eventForUpdate, string calendarId, string eventId)
        {
            try
            {
                return _service.Events.Update(eventForUpdate, calendarId, eventId).Execute();
            }
            catch (Exception e)
            {
                _log?.Error("Exception - \n" + e);
                throw;
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
                _log?.Error("Exception - \n" + e);
                throw;
            }
        }

        public IList<Event> GetEvents(string calendarId)
        {
            try
            {
                var request = _service.Events.List(calendarId);
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
                return request.Execute().Items;
            }
            catch (Exception e)
            {
                _log?.Error("Exception - \n" + e);
                throw;
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
                _log?.Error("Exception - \n" + e);
                throw;
            }
        }

        public Event SearchEventById(string calendarId, string eventId)
        {
            try
            {
                return _service.Events.Get(calendarId, eventId).Execute();
            }
            catch (Exception e)
            {
                _log?.Error("Exception - \n" + e);
                throw;
            }
        }
    }
}
