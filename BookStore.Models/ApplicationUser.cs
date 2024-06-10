﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models;

public class ApplicationUser:IdentityUser
{
    [Required]
    public string Name { get; set; }
    public string? StreetAddres { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
}
