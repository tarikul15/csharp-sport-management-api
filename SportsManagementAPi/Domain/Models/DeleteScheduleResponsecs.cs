using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class DeleteScheduleResponse : BaseResponse
    {
        public Schedule Schedule { get; private set; }

        public DeleteScheduleResponse(bool success, string message, Schedule schedule) : base(success, message)
        {
            Schedule = schedule;
        }
    }
}
