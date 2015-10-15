using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using GoogleCalendarService.Helpers;

namespace GoogleCalendarService
{
    public class BookingService : IBookingService
    {
        private readonly CalendarService _service;
        private readonly ILog _log;

        public BookingService(CalendarService service, ILog log) : this(service)
        {
            _log = log;
        }

        public BookingService(CalendarService service)
        {
            _service = service;
        }
        public Event UpdateEvent(Event eventToUpdate, string calendarId, string eventId)
        {
            try
            {
                return _service.Events.Update(eventToUpdate, calendarId, eventId).Execute();
            }
            catch (Exception e)
            {
                _log?.Error("Exception - \n" + e);
                throw;
            }
        }

        public Event PostEvent(Event eventToAdd, string calendarId)
        {
            try
            {             
               return _service.Events.Insert(eventToAdd, calendarId).Execute();
            }
            catch (Exception e)
            {
                _log?.Error("Exception - \n" + e);
                throw;
            }
        }

        public List<Event> GetEvents(string calendarId)
        {
            int maxResults = Convert.ToInt32(AppSettingsHelper.GetAppSetting(AppSetingsConst.MaxResults));

            try
            {
                EventsResource.ListRequest setUpRequset = _service.Events.List(calendarId);
                setUpRequset.MaxResults = maxResults;
                Events request = setUpRequset.Execute();
                string token = request.NextPageToken;
                List<Event> eventList = request.Items.ToList();
                while (token != null)
                {
                    var tempRequst = _service.Events.List(calendarId);
                    tempRequst.MaxResults = maxResults;
                    tempRequst.PageToken = token;
                    eventList = eventList.Concat(tempRequst.Execute().Items).ToList();
                    var updatedRequest = tempRequst.Execute();
                    token = updatedRequest.NextPageToken;
                }

                return eventList;
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

        public string GetCalendarTimeZone(string calendarId)
        {
            var calendars = _service.CalendarList.List().Execute();
            return calendars.Items.First(cl => cl.Id == calendarId).TimeZone;
        }
    }
}
