using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmbeddedStock2.Models;

namespace EmbeddedStock2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EmbeddedStock2;Trusted_Connection=True;");
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentType> ComponentTypes { get; set; }
        public DbSet<CategoryComponentType> CategoryComponentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CategoryComponentType>()
            .HasKey(t => new { t.CategoryId, t.ComponentTypeId });

            builder.Entity<CategoryComponentType>()
                .HasOne(pt => pt.Category)
                .WithMany(p => p.CategoryComponentTypes)
                .HasForeignKey(pt => pt.CategoryId);

            builder.Entity<CategoryComponentType>()
                .HasOne(pt => pt.ComponentType)
                .WithMany(t => t.CategoryComponentTypes)
                .HasForeignKey(pt => pt.ComponentTypeId);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

    }
}
