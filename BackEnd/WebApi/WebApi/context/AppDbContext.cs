using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> User { get; set; }

    public DbSet<Organization> Organization { get; set; }

    public DbSet<License> License { get; set; }

    public DbSet<PathPhoto> PathPhoto { get; set; }

    public DbSet<Absence> Absence { get; set; }

    public DbSet<Truck> Truck { get; set; }

    public DbSet<TruckBreakDowns> TruckBreakDowns { get; set; }

    public DbSet<Transport> Transport { get; set; }

    public DbSet<TransportReviewParameters> TransportReviewParameters { get; set; }

    public DbSet<ServiceTransport> Service { get; set; }

    public DbSet<ServiceCoord> ServiceCoord { get; set; }
}