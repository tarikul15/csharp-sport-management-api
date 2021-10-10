using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ManagerId { get; set; }

        public Manager Manager { get; set; }
    }
}
