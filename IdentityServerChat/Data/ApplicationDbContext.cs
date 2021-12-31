using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityServerAspNetIdentity.Models;
using System;
using EntityServerTests.Models;

namespace IdentityServerAspNetIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
           // Database.EnsureCreated();
            Console.WriteLine("DB Created");
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        public DbSet<ClientSecrets> ClientSecrets { get; set; }
        public DbSet<ClientScope> ClientScopes { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }
        public DbSet<ApiResourceScope> ApiResourceScopes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
          //  builder.Entity<Client>().HasKey(e => e.Id);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
