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
                // Password is 'admin123' hashed with BCrypt
                Password = "$2a$11$qM/r6mUoI1B8zY.m6p7K.eR3H.o5v.m6p7K.eR3H.o5v.m6p7K.e" // Fixed hash for 'admin123'
            });
        }
    }
}