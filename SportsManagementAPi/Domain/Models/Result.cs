using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class Result
    {
        public Guid GameId { get; set; }
        public Guid WinnerTeamId { get; set; }
        public Guid LoserTeamId { get; set; }
        public Guid ManagerId { get; set; }
        public string ResultDetails { get; set; }
        public Schedule Schedule { get; set; }
        public List<Manager> Managers { get; set; }
        public List<Team> Teams { get; set; }
    }
}
