using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Domain.Entities.Areas;
using Fullstack.SAXS.Domain.Entities.Sp;
using Fullstack.SAXS.Infrastructure.DbContexts;
using MathNet.Numerics.Statistics.Mcmc;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.SAXS.Infrastructure.Repositories
{
    public class AreaRepository(IFileService file, PosgresDbContext postgres) : IStorage
    {
        private ICollection<Area> _areas = new List<Area>(10);

        public void Add(Area entity)
        {
            _areas.Add(entity);
        }

        public Area GetArea(Guid id)
        {
            var path = postgres.Datas.FirstOrDefault(d => d.Id == id);

            if (path == null)
                throw new ArgumentException($"No data with this id {id}");

            return file.Read(path.Path);
        }

        public async Task<Area> GetAreaAsync(Guid id)
        {
            var path = await postgres.Datas.FirstOrDefaultAsync(d => d.Id == id);

            if (path == null)
                throw new ArgumentException($"No data with this id {id}");

            return await file.ReadAsync(path.Path);
        }

        public void Save(Guid idUser)
        {
            var genNum = GetGenNum();

            for (var i = 0; i < _areas.Count; i++)
            {
                var path = file.Write(_areas.ElementAt(i), genNum);

                var data = new SpData(path);

                var gen = 
                    new SpGeneration(
                        idUser, 
                        genNum, _areas.ElementAt(i).Series, 
                        _areas.ElementAt(i).AreaType,
                        _areas.ElementAt(i).ParticlesType ?? 0,
                        null, _areas.ElementAt(i).Particles.Count(),
                        data.Id
                    );

                postgres.Datas.Add(data);
                postgres.Generations.Add(gen);

            }


            postgres.SaveChanges();
        }

        public async Task SaveAsync(Guid idUser)
        {
            var genNum = GetGenNum();

            for (var i = 0; i < _areas.Count; i++)
            {
                var path = await file.WriteAsync(_areas.ElementAt(i), genNum);

                var data = new SpData(path);

                var gen =
                    new SpGeneration(
                        idUser,
                        genNum, _areas.ElementAt(i).Series,
                        _areas.ElementAt(i).AreaType,
                        _areas.ElementAt(i).ParticlesType ?? 0,
                        null, _areas.ElementAt(i).Particles.Count(),
                        data.Id
                    );

                await postgres.Datas.AddAsync(data);
                await postgres.Generations.AddAsync(gen);

            }


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
