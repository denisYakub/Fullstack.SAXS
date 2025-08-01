using System.Text.Json;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Dtos;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.Models;
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

            var genNum = await GetGenNumAsync()
                .ConfigureAwait(false);

            for (var i = 0; i < entityList.Count; i++)
            {
                var path = await fileService
                    .WriteAsync(entityList.ElementAt(i), genNum)
                    .ConfigureAwait(false);

                var entity = entityList.ElementAt(i);

                var gen =
                    new SpGeneration()
                    {
                        UserId = idUser,
                        GenNum = genNum,
                        SeriesNum = entity.Series,
                        AreaType = entity.AreaType.ToString(),
                        ParticleType = entity.ParticlesType.ToString(),
                        AreaOuterRadius = entity.OuterRadius,
                        ParticleNum = entity.Particles.Count(),
                    };

                var data = new SpData()
                {
                    Path = path,
                    GenId = gen.Id,
                    Gen = gen,
                };

                gen.Data = data;

                await postgres.Datas
                    .AddAsync(data)
                    .ConfigureAwait(false);
                await postgres.Generations
                    .AddAsync(gen)
                    .ConfigureAwait(false);

            }
            await postgres
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }

        public async Task<string> GetGenerationsAsync(Guid? idUser)
        {
            var query = postgres
                .Generations
                .AsQueryable();

            if (idUser.HasValue)
                query = query.Where(g => g.UserId == idUser.Value);

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
                    g.Data!.Id
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

        public async Task UpdateAvgPhiAsync(Guid idUser, Guid idArea, double avgPhi)
        {
            var generation = await postgres
                .Generations
                .FirstAsync(g => g.Data!.Id == idArea && g.Data!.Id == idUser)
                .ConfigureAwait(false);

            generation.Phi = avgPhi;

            await postgres
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }

        private async Task<long> GetGenNumAsync()
        {
            var counter = await postgres
                .GenerationCurrentNum
                .FirstOrDefaultAsync();

            if (counter == null)
            {
                counter = new SpGenerationNumberCounter();

                await postgres
                    .GenerationCurrentNum
                    .AddAsync(counter);
            }
            else
            {
                counter.Increase();
            }

            await postgres
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return counter.CurrentNum;
        }

        public async Task SaveSystemTaskAsync(SystemCreateDto dto)
        {
            var sysTask = new SpSystemTask()
            {
                UserId = dto.UserId,
                State = TaskState.InProgress,
                AreaSize = dto.AreaSize,
                AreaNumber = dto.AreaNumber,
                AreaType = "Sphere",
                ParticleNumber = dto.ParticleNumber,
                ParticleType = dto.ParticleType.ToString(),
                ParticleMinSize = dto.ParticleMinSize,
                ParticleMaxSize = dto.ParticleMaxSize,
                ParticleSizeShape = dto.ParticleSizeShape,
                ParticleSizeScale = dto.ParticleSizeScale,
                ParticleAlphaRotation = dto.ParticleAlphaRotation,
                ParticleBetaRotation = dto.ParticleBetaRotation,
                ParticleGammaRotation = dto.ParticleGammaRotation,
            };

            await postgres
                .SystemTasks
                .AddAsync(sysTask)
                .ConfigureAwait(false);

            await postgres
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}
