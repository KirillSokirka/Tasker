﻿using Microsoft.AspNetCore.Identity;

namespace Tasker.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiryTime { get; set; }
}