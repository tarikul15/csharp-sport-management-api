using System;


namespace SportsManagementAPi.Domain.Models
{
    public class Manager
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Team Team { get; set; }
    }
}