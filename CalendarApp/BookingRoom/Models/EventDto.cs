﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingRoom.Models
{
    public class EventDto
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string Summary { get; set; }
        public DateTime? Created { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
            
    }
}
