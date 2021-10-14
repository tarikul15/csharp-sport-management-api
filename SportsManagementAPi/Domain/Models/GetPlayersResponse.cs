using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class GetPlayersResponse : BaseResponse
    {
        public List<Player> Players { get; private set; }

        public GetPlayersResponse(bool success, string message, List<Player> players) : base(success, message)
        {
            Players = players;
        }
    }
}
