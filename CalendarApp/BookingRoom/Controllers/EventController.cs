using BookingRoom.Models.GoogleConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookingRoom.Controllers
{
    public class EventController : ApiController
    {
        protected readonly ICalendarConnection _connection;
        public EventController(ICalendarConnection connection)
        {
            _connection = connection;
        }
    }
}
