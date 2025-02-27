﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestRESTAPI.Data.Models;

namespace TestRESTAPI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext>  options) : base(options) 
        { 
        
        }

        public DbSet<Orders> Orders { get; set; }
    }
}
