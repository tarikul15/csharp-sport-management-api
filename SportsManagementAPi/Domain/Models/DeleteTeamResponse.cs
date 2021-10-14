using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class DeleteTeamResponse : BaseResponse
    {
        public Team Team { get; private set; }

        public DeleteTeamResponse(bool success, string message, Team team) : base(success, message)
        {
            Team = team;
        }
    }
}
