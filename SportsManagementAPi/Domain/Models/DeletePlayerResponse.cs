using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class DeletePlayerResponse : BaseResponse
    {
        public Player Player { get; private set; }

        public DeletePlayerResponse(bool success, string message, Player player) : base(success, message)
        {
            Player = player;
        }
    }
}
