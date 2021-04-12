
namespace  ImmunizationApi.Repository.Example
{
    using Microsoft.EntityFrameworkCore;

    using ImmunizationApi.Models.Entity;

    public class ImmunizationDbContext : DbContext
    {
        public ImmunizationDbContext(DbContextOptions<ImmunizationDbContext> options) : base(options)
        {
        }

        public DbSet<PatientEntity> Patients { get;  set; }
        public DbSet<LocationEntity> Locations { get; set; }
        public DbSet<VaccineEntity> Vaccines { get; set; }
        public DbSet<ImmunizationEntity> Immunizations { get; set; }
    }
}