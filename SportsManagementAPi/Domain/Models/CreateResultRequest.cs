using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class CreateResultRequest
    {
        public Guid GameId { get; set; }
        public string WinnerTeamName { get; set; }
        public string LoserTeamName { get; set; }
        public string ResultDetails { get; set; }
    }
}
