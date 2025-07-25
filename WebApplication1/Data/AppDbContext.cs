﻿using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Models;

namespace WebApplication1.Data {
    public class AppDbContext : DbContext {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            
        }
        
        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

    }
}
