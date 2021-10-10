using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class CreatePlayerRequest
    {
        public string Name { get; set; }
        public string TeamName { get; set; }
        public string Details { get; set; }
    }
}
