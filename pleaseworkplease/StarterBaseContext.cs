using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace pleaseworkplease;

public partial class StarterBaseContext : IdentityDbContext<Users>
{
    public StarterBaseContext()
    {
    }

    public StarterBaseContext(DbContextOptions<StarterBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Novel> Novels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationBuilder configurationbuilder = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json", optional: true);
        IConfigurationRoot config = configurationbuilder.Build();


        if (!optionsBuilder.IsConfigured)


        {


            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));


        }

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Novel>(entity =>
        {
            entity.HasOne(d => d.GenreNavigation).WithMany(p => p.Novels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Novels_Genre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
