using Microsoft.EntityFrameworkCore;
using TestAPI.Models;

namespace TestAPI.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {


        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Departement> Departements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
              .HasOne(e => e.Departement)
              .WithMany(p => p.Employee)
              .HasForeignKey(e => e.DepId)
              .IsRequired();
        }

    }
}
