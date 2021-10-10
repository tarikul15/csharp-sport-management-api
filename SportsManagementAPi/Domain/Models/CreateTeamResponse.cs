using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class CreateTeamResponse : BaseResponse
    {
        public Team Team { get; private set; }

        public CreateTeamResponse(bool success, string message, Team team) : base(success, message)
        {
            Team = team;
        }
    }
}
