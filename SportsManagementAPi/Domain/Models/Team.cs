using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ManagerId { get; set; }

        public Manager Manager { get; set; }

        public List<Player> Players { get; set; }

        public List<Schedule> Schedules { get; set; }

        public List<Result> Results { get; set; }
    }
}
