using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class PlayerResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TeamId { get; set; }
        public Guid ManagerId { get; set; }
        public string Details { get; set; }
    }
}
