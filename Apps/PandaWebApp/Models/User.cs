﻿namespace PandaWebApp.Models
{
    using System.Collections.Generic;


    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; } = Role.User;

        public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
    }
}
