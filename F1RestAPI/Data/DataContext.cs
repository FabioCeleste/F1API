using Microsoft.EntityFrameworkCore;
using F1RestAPI.Models;

namespace F1RestAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Driver> Drivers => Set<Driver>();
        public DbSet<Constructor> Constructors => Set<Constructor>();
        public DbSet<DriverConstructor> DriverConstructors => Set<DriverConstructor>();
        public DbSet<IpCountryState> IpCountryStates => Set<IpCountryState>();
        public DbSet<User> Users => Set<User>();
        public DbSet<IpConnectionCount> IpConnectionCounts => Set<IpConnectionCount>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(d => d.DriverId);
                entity.Property(d => d.DriverId).ValueGeneratedOnAdd();
                entity.Property(d => d.DriverCode).HasMaxLength(3);
                entity.Property(d => d.DriverRef).IsRequired();
                entity.Property(d => d.DriverNumber).IsRequired();
                entity.Property(d => d.DriverForename).IsRequired();
                entity.Property(d => d.DriverSurname).IsRequired();
                entity.Property(d => d.DateOfBirth).IsRequired();
                entity.Property(d => d.Nationality).IsRequired();
                entity.Property(d => d.WikipediaUrl).IsRequired();
                entity.Property(d => d.PolePositions).IsRequired();
                entity.Property(d => d.Wins).IsRequired();
                entity.Property(d => d.Podiumns).IsRequired();
                entity.Property(d => d.DNF).IsRequired();
            });

            modelBuilder.Entity<Constructor>(entity =>
            {
                entity.HasKey(c => c.ConstructorId);
                entity.Property(c => c.ConstructorId).ValueGeneratedOnAdd();
                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.ConstructorRef).IsRequired();
                entity.Property(c => c.Nationality).IsRequired();
                entity.Property(c => c.Url).IsRequired();
            });

            modelBuilder.Entity<DriverConstructor>(entity =>
            {
                entity.HasKey(dc => new
                {
                    dc.DriverId,
                    dc.ConstructorId,
                    dc.Year
                });
            });

            modelBuilder.Entity<DriverConstructor>()
            .HasOne(dc => dc.Driver)
            .WithMany(d => d.DriverConstructors)
            .HasForeignKey(dc => dc.DriverId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DriverConstructor>()
            .HasOne(dc => dc.Constructor)
            .WithMany(c => c.DriverConstructors)
            .HasForeignKey(dc => dc.ConstructorId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IpCountryState>(entity =>
            {
                entity.HasKey(iC => new { iC.Country, iC.City });
                entity.Property(iC => iC.City);
                entity.Property(iC => iC.Country);
                entity.Property(iC => iC.Count);
            });

            modelBuilder.Entity<IpConnectionCount>(entity =>
            {
                entity.HasKey(iC => iC.Ip);
                entity.Property(iC => iC.Count);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.PasswordHash);
                entity.Property(u => u.Username);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}