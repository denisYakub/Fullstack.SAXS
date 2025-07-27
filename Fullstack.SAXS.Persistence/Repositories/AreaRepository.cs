using System.Text.Json;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Sp;
using Fullstack.SAXS.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.SAXS.Persistence.Repositories
{
    public class AreaRepository(IFileService fileService, PosgresDbContext postgres) : IStorage
    {
        public async Task AddRangeAsync(IEnumerable<Area> entities, Guid idUser)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities), "Shouldn't be null.");

            var entityList = entities.ToList();

            var genNum = GetGenNum();

            for (var i = 0; i < entityList.Count; i++)
            {
                var path = await fileService
                    .WriteAsync(entityList.ElementAt(i), genNum)
                    .ConfigureAwait(false);

                var data = new SpData(path);

                var entity = entityList.ElementAt(i);

                var gen =
                    new SpGeneration(
                        idUser,
                        genNum, entity.Series,
                        entity.AreaType,
                        entity.ParticlesType,
                        entity.OuterRadius, 
                        null, entity.Particles.Count(),
                        data.Id
                    );

                await postgres.Datas
                    .AddAsync(data)
                    .ConfigureAwait(false);
                await postgres.Generations
                    .AddAsync(gen)
                    .ConfigureAwait(false);

            }
            await postgres.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<string> GetAllGenerationsAsync()
        {
            var gens = await postgres.Generations
                .Select(g => new
                {
                    g.GenNum,
                    g.SeriesNum,
                    g.AreaType,
                    g.ParticleType,
                    g.AreaOuterRadius,
                    g.Phi,
                    g.ParticleNum,
                    g.IdSpData
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return JsonSerializer.Serialize(gens);
        }

        public async Task<Area> GetAreaAsync(Guid idArea)
        {
            var path = await postgres.Datas
                .FirstOrDefaultAsync(d => d.Id == idArea)
                .ConfigureAwait(false);

            if (path == null)
                throw new KeyNotFoundException($"No data found with ID: {idArea}.");

            return await fileService
                .ReadAsync(path.Path)
                .ConfigureAwait(false);
        }

        public async Task<string> GetAllGenerationsAsync(Guid idUser)
        {
            var gens = await postgres.Generations
                .Where(g => g.IdUser == idUser)
                .Select(g => new
                {
                    g.GenNum,
                    g.SeriesNum,
                    g.AreaType,
                    g.ParticleType,
                    g.AreaOuterRadius,
                    g.Phi,
                    g.ParticleNum,
                    g.IdSpData
                })
                .ToListAsync()
                .ConfigureAwait(false);

            return JsonSerializer.Serialize(gens);
        }

        public async Task UpdateAvgPhiAsync(Guid idUser, Guid idArea, double avgPhi)
        {
            var generation = await postgres
                .Generations
                .FirstAsync(g => g.IdSpData == idArea && g.IdUser == idUser)
                .ConfigureAwait(false);

            generation.ChangePhi(avgPhi);

            await postgres
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }

        private long GetGenNum()
        {
            var counter = 
                postgres
                .GenerationCurrentNum
                .FirstOrDefault();

            if (counter == null)
            {
                counter = new SpGenerationNumberCounter();
                postgres.GenerationCurrentNum.Add(counter);
            }
            else
            {
                counter.Increase();
            }

            postgres.SaveChanges();

            return counter.CurrentNum;
        }
    }
}
