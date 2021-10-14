using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class CreateScheduleRequest
    {
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string ScheduleDetails { get; set; }
    }
}
