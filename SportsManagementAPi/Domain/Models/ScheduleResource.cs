using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class ScheduleResource
    {
        public Guid GameId { get; set; }
        public Guid HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public Guid AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public DateTime ScheduledTime { get; set; }
        public Guid ManagerId { get; set; }
    }
}
