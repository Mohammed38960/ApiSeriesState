using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIRESTSTATE.Models.EntityFramework;

public partial class SerieDbContext : DbContext
{
    public SerieDbContext()
    {
    }

    public SerieDbContext(DbContextOptions<SerieDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Serie> Series { get; set; }

//    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=localhost;Database=SerieDB;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Serie>(entity =>
        {
            entity.HasKey(e => e.Serieid).HasName("serie_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
