using System;
using System.Collections.Generic;


namespace SportsManagementAPi.Domain.Models
{
    public class Manager
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Team Team { get; set; }

        public List<Player> Players { get; set; }

        public List<Schedule> Schedules { get; set; }

        public List<Result> Results { get; set; }
    }
}