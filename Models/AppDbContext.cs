using System;
using Microsoft.EntityFrameworkCore;

namespace CosmosPostgresAPI.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pharmacy> Pharmacies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pharmacy>()
        .ToTable("pharmacies")
        .HasKey(p => p.PharmacyId);
    }

    public async Task DistributeTableAsync()
    {
        await Database.ExecuteSqlRawAsync("SELECT create_distributed_table('pharmacies', 'PharmacyId');");
    }

}
