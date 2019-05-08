using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LitterManager.Models;
using Microsoft.AspNetCore.Identity;

namespace LitterManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Litter> Litters { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<BreedDescription> BreedDescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Litter>().OwnsOne<PriceRange>("Prices", a =>
            {
                a.Property(ca => ca.PriceFrom);
                a.Property(ca => ca.PriceTo);
            });

            builder.Entity<LitterBreedDescriptions>()
                .HasKey(bc => new { bc.LitterId, bc.BreedDescriptionId });
        }
    }
}
