﻿using Leave_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Leave_Application.Data
{
    public class AppDbContext : DbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LeaveTable> LeaveApplications { get; set; }
    }
}
