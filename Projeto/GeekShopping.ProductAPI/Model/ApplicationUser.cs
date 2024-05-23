﻿using Microsoft.AspNetCore.Identity;

namespace GeekShopping.ProductAPI.Model;
public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
