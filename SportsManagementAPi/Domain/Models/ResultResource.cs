using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class ResultResource
    {
        public Guid GameId { get; set; }
        public Guid WinnerTeamId { get; set; }
        public Guid LoserTeamId { get; set; }
        public string ResultDetails { get; set; }
        public Guid ManagerId { get; set; }
    }
}
