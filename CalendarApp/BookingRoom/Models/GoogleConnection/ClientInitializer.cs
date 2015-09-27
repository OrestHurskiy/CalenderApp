using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;

namespace BookingRoom.Models.GoogleConnection
{
    public class ClientInitializer
    {
        private readonly BaseClientService.Initializer _initializer;

        public ClientInitializer()
        {
            _initializer = new BaseClientService.Initializer();
        }

        public BaseClientService.Initializer ClientService
        {
            get { return _initializer; }
        }
    }
}
