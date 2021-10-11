using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class CreateScheduleResponse : BaseResponse
    {
            public Schedule Schedule { get; private set; }

            public CreateScheduleResponse(bool success, string message, Schedule schedule) : base(success, message)
            {
                Schedule = schedule;
            }
        }
}
