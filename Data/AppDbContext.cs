namespace APBD_PROJEKT.Data;

using Microsoft.EntityFrameworkCore;
using Models;
public class AppDbContext : DbContext
{
    public DbSet<CompanyClient> CompanyClients { get; set; }
    
    public DbSet<IndividualClient> IndividualClients { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
  
        modelBuilder.Entity<CompanyClient>().HasData(
        );
        
        modelBuilder.Entity<IndividualClient>().HasData(
        );
    }
}