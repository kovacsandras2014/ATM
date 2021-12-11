using System.Threading;
using System.Threading.Tasks;
using ATM.Model.DbModel.Entities;
using ATM.Model.DbModel.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace ATM.Model.DbModel
{
    public class AtmDbContext : DbContext
    {
        public DbSet<Denominations> Denominations => Set<Denominations>();

        public AtmDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DenominationsConfiguration());
        }

    }
}
