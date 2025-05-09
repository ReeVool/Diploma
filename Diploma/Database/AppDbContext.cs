using Microsoft.EntityFrameworkCore;
using Diploma.Database.Models;
using Diploma.Models;
using System.IO;

namespace Diploma.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<Manufacturers> Manufacturers { get; set; }
        public DbSet<Pharmacy> Pharmacy { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<QuerriesToBuy> QuerriesToBuy { get; set; }
        public DbSet<Workers> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 1
            //optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\jamal\\source\\repos\\WPF\\Diploma\\Diploma\\Database\\PharmacyDatabase.mdf;Integrated Security=True");

            // 2
            string baseDirectory = AppContext.BaseDirectory;
            string relativePath = Path.Combine("Database", "PharmacyDatabase.mdf");
            string absoluteDbPath = Path.Combine(baseDirectory, relativePath);
            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={absoluteDbPath};Integrated Security=True";

            optionsBuilder.UseSqlServer(connectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workers>()
                .HasIndex(e => e.Login)
                .IsUnique();
        }
    }
}
