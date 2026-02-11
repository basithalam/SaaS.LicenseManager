using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SaaS.LicenseManager.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Admin User
            modelBuilder.Entity<AdminUser>().HasData(new AdminUser
            {
                Id = 1,
                Username = "admin",
                Email = "admin@example.com",
                // Password is 'admin123' hashed with a fixed salt for deterministic seeding
                Password = BCrypt.Net.BCrypt.HashPassword("admin123", "$2a$11$abcdefghijklmnopqrstuu") 
            });
        }
    }
}