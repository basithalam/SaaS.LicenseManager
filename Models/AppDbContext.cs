using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SaaS.LicenseManager.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    }
}