using Fullstack.SAXS.Server.Domain.Entities.Sp;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.SAXS.Server.Infastructure.Persistence.DbContexts
{
    public class PosgresDbContext(DbContextOptions<PosgresDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<SpData> Datas { get; private set; }
        public DbSet<SpGeneration> Generations { get; private set; }
        public DbSet<SpGenerationNumberCounter> GenerationCurrentNum { get; private set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<SpGeneration>()
                .HasOne(gen => gen.SpData)
                .WithOne(data => data.SpGeneration)
                .HasForeignKey<SpGeneration>(gen => gen.IdSpData);
        }
    }
}
