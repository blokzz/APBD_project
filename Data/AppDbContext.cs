namespace APBD_PROJEKT.Data;

using Microsoft.EntityFrameworkCore;
using Models;
public class AppDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<CompanyClient> CompanyClients { get; set; }
    public DbSet<IndividualClient> IndividualClients { get; set; }
    
    public DbSet<ContractPayments> ContractPayments { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<SoftwareDiscount> SoftwareDiscounts { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }
    public DbSet<Employee> Employees { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasData(new Employee
        {
            Id = 1,
            Login = "admin",
            PasswordHash = "$2a$11$gKSs4tyRgaghlPGq24qNxeJJ8Cz6tzRQbjA6erAWxURs3mZ2Xt4Y2",
            Role = "Admin"
        });
        
        modelBuilder.Entity<SoftwareDiscount>()
            .HasKey(sd => new { sd.DiscountId, sd.SoftwareId });

        modelBuilder.Entity<SoftwareDiscount>()
            .HasOne(sd => sd.Discount)
            .WithMany(s => s.SoftwareDiscounts)
            .HasForeignKey(sd => sd.DiscountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SoftwareDiscount>()
            .HasOne(sd => sd.Software)
            .WithMany(s => s.SoftwareDiscounts)
            .HasForeignKey(sd => sd.SoftwareId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<IndividualClient>().ToTable("IndividualClients");
        modelBuilder.Entity<CompanyClient>().ToTable("CompanyClients");

        modelBuilder.Entity<IndividualClient>()
            .HasIndex(c => c.PESEL)
            .IsUnique();

        modelBuilder.Entity<CompanyClient>()
            .HasIndex(c => c.KRS)
            .IsUnique();
        
        
        modelBuilder.Entity<Software>().HasData(
            new Software
            {
                Id = 1,
                Name = "FinanceApp",
                Description = "System finansowy",
                CurrentVersion = "1.0",
                Category = "Finanse",
                YearlyLicensePrice = 5000m
            },
            new Software
            {
                Id = 2,
                Name = "EduPlatform",
                Description = "Platforma edukacyjna",
                CurrentVersion = "2.1",
                Category = "Edukacja",
                YearlyLicensePrice = 3000m
            }
        );
    }
}