﻿using Microsoft.AspNetCore.Identity;

namespace VehiGate.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
