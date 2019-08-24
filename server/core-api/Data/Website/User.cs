﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using server.Data.Auth;

namespace server.Data.Website
{
    [Serializable]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public override Guid Id { get; set; }
        public int AccountId { get; set; }

        [MinLength(2)]
        [MaxLength(16)]
        public string Firstname { get; set; }

        [MinLength(2)]
        [MaxLength(16)]
        public string Lastname { get; set; }
        public byte Online { get; set; }
        public DateTime JoinDate { get; set; }
        public string Location { get; set; }
        public int TotalVotes { get; set; }

        public bool IsOnline()
        {
            return Online == 1;
        }

        //public virtual List<ApplicationUserClaim> Claims { get; set; }
        //public virtual List<ApplicationUserLogin> Logins { get; set; }
        //public virtual List<ApplicationUserToken> Tokens { get; set; }

        [JsonIgnore]
        public virtual List<ApplicationUserRole> UserRoles { get; set; }
    }

    [Serializable]
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole(string name) : base(name)
        {
        }

        public virtual List<ApplicationUserRole> UserRoles { get; set; }
        //public virtual List<ApplicationRoleClaim> RoleClaims { get; set; }
    }

    [Serializable]
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }

    [Serializable]
    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
        //public virtual ApplicationUser User { get; set; }
    }

    [Serializable]
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        //public virtual ApplicationUser User { get; set; }
    }

    [Serializable]
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        //public virtual ApplicationRole Role { get; set; }
    }

    [Serializable]
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        //public virtual ApplicationUser User { get; set; }
    }
}
