﻿using System;
using Microsoft.AspNetCore.Identity;

namespace server.Data.Website
{
    [Serializable]
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        //public virtual ApplicationUser User { get; set; }
    }
}