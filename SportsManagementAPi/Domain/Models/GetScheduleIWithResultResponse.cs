using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class GetScheduleIWithResultResponse : BaseResponse
    {
        public List<Schedule> Schedules { get; private set; }

        public GetScheduleIWithResultResponse(bool success, string message, List<Schedule> schedules) : base(success, message)
        {
            Schedules = schedules;
        }
    }
}
