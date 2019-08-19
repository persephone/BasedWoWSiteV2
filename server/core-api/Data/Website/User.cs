﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace server.Data.Website
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public override Guid Id { get; set; }
        public int AccountId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public byte Online { get; set; }
        public DateTime JoinDate { get; set; }
        public string Location { get; set; }
        public int TotalVotes { get; set; }

        public bool IsOnline()
        {
            return Online == 1;
        }
    }

    public class ApplicationRole : IdentityRole<Guid> { }
}
