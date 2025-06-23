using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Infrastructure.DbContexts;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using Fullstack.SAXS.Server.Domain.Entities.Sp;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.SAXS.Infrastructure.Repositories
{
    public class AreaRepository(IFileService file, PosgresDbContext postgres) : IStorage
    {
        private ICollection<Area> _areas = new List<Area>(10);
        private ICollection<SpData> _areaPathDatas = new List<SpData>(10);

        public void Add(Area entity)
        {
            _areas.Add(entity);

            var path = file.Write(entity);

            _areaPathDatas.Add(new(path));
        }

        public async Task AddAsync(Area entity)
        {
            _areas.Add(entity);

            var path = await file.WriteAsync(entity);

            _areaPathDatas.Add(new (path));
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
            postgres.Datas.AddRange(_areaPathDatas);

            var genNum = GetGenNum();

            for (var i = 0; i < _areas.Count; i++)
            {
                var gen = 
                    new SpGeneration(
                        idUser, 
                        genNum, _areas.ElementAt(i).Series, 
                        _areas.ElementAt(i).AreaType,
                        _areas.ElementAt(i).ParticlesType ?? 0,
                        null, _areas.ElementAt(i).Particles.Count(), 
                        _areaPathDatas.ElementAt(i).Id
                    );

                postgres.Generations.Add(gen);
            }
        }

        public async Task SaveAsync(Guid idUser)
        {
            await postgres.Datas.AddRangeAsync(_areaPathDatas);

            var genNum = GetGenNum();

            for (var i = 0; i < _areas.Count; i++)
            {
                var gen =
                    new SpGeneration(
                        idUser,
                        genNum, _areas.ElementAt(i).Series,
                        _areas.ElementAt(i).AreaType,
                        _areas.ElementAt(i).ParticlesType ?? 0,
                        null, _areas.ElementAt(i).Particles.Count(),
                        _areaPathDatas.ElementAt(i).Id
                    );

                await postgres.Generations.AddAsync(gen);
            }

            postgres.SaveChanges();
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
