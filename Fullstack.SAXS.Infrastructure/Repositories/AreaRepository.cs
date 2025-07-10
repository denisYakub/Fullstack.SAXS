using System.Text.Json;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Sp;
using Fullstack.SAXS.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.SAXS.Infrastructure.Repositories
{
    public class AreaRepository(IFileService file, PosgresDbContext postgres) : IStorage
    {
        public async Task AddRangeAsync(IEnumerable<Area> entities, Guid idUser)
        {
            var genNum = GetGenNum();

            for (var i = 0; i < entities.Count(); i++)
            {
                var path = await file.WriteAsync(entities.ElementAt(i), genNum);

                var data = new SpData(path);

                var entity = entities.ElementAt(i);

                var gen =
                    new SpGeneration(
                        idUser,
                        genNum, entity.Series,
                        entity.AreaType,
                        entity.ParticlesType ?? 0,
                        entity.OuterRadius, 
                        null, entity.Particles.Count(),
                        data.Id
                    );

                await postgres.Datas.AddAsync(data);
                await postgres.Generations.AddAsync(gen);

            }


            await postgres.SaveChangesAsync();
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
                .ToListAsync();

            return JsonSerializer.Serialize(gens);
        }

        public async Task<Area> GetAreaAsync(Guid id)
        {
            var path = await postgres.Datas.FirstOrDefaultAsync(d => d.Id == id);

            if (path == null)
                throw new KeyNotFoundException($"No data found with ID: {id}.");

            return await file.ReadAsync(path.Path);
        }

        public async Task<string> GetGenerationsAsync(Guid idUser)
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
                .ToListAsync();

            return JsonSerializer.Serialize(gens);
        }

        public async Task SaveAvgPhiAsync(Guid idUser, Guid id, double phi)
        {
            var generation = await postgres
                .Generations
                .FirstAsync(g => g.IdSpData == id && g.IdUser == idUser);

            generation.ChangePhi(phi);

            await postgres.SaveChangesAsync();
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
