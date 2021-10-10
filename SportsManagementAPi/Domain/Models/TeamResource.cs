using System;

namespace SportsManagementAPi.Domain.Models
{
    public class TeamResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ManagerId { get; set; }

    }
}
