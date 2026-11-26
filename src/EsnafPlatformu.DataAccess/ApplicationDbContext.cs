using EsnafPlatformu.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BusinessEntity = EsnafPlatformu.Entities.Business;

namespace EsnafPlatformu.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<BusinessEntity> Businesses => Set<BusinessEntity>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    public DbSet<Store> Stores => Set<Store>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var business = modelBuilder.Entity<BusinessEntity>();
        business.Property(x => x.OwnerUserId).IsRequired();

        business.Property(x => x.Name).HasMaxLength(100);
        business.Property(x => x.Slug).HasMaxLength(60);
        business.Property(x => x.City).HasMaxLength(80);
        business.Property(x => x.District).HasMaxLength(80);

        business.Property(x => x.ShortDescription).HasMaxLength(200);
        business.Property(x => x.About).HasMaxLength(3000);
        business.Property(x => x.Phone).HasMaxLength(30);
        business.Property(x => x.ContactEmail).HasMaxLength(254);

        business.HasIndex(x => x.Slug)
            .IsUnique()
            .HasDatabaseName("ux_businesses_slug");

        business.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<BusinessEntity>(x => x.OwnerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        business.HasIndex(x => x.OwnerUserId)
            .IsUnique()
            .HasDatabaseName("ux_businesses_owner_user_id");

        var menuItem = modelBuilder.Entity<MenuItem>();

        menuItem.Property(x => x.Price).HasPrecision(10, 2);

        menuItem.HasOne(x => x.Business)
            .WithMany(x => x.MenuItems)
            .HasForeignKey(x => x.BusinessId)
            .OnDelete(DeleteBehavior.Restrict);


    var store = modelBuilder.Entity<Store>();

store.Property(x => x.Name)
    .HasMaxLength(100)
    .IsRequired();

store.HasOne(x => x.Business)
    .WithMany()
    .HasForeignKey(x => x.BusinessId)
    .OnDelete(DeleteBehavior.Restrict);

    }
}