using System;

namespace SportsManagementAPi.Domain.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TeamId { get; set; }
        public Guid ManagerId { get; set; }
        public string Details { get; set; }

        public Team Team { get; set; }
        public Manager Manager { get; set; }

    }
}
