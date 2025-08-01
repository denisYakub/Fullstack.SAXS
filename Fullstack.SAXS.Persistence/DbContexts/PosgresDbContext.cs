using Fullstack.SAXS.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.SAXS.Persistence.DbContexts
{
    public class PosgresDbContext(DbContextOptions<PosgresDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<SpData> Datas { get; private set; }
        public DbSet<SpGeneration> Generations { get; private set; }
        public DbSet<SpGenerationNumberCounter> GenerationCurrentNum { get; private set; }
        public DbSet<SpSystemTask> SystemTasks { get; private set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SpGeneration>()
                .HasOne(g => g.Data)
                .WithOne(d => d.Gen)
                .HasForeignKey<SpData>(d => d.GenId);
        }
    }
}