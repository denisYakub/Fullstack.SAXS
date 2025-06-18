using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Infrastructure.DbContexts;
using Fullstack.SAXS.Server.Domain.Entities.Areas;
using Fullstack.SAXS.Server.Domain.Entities.Sp;

namespace Fullstack.SAXS.Infrastructure.Repositories
{
    public class AreaRepository(
        IFileService file, PosgresDbContext postgres, string folderPath
    ) : IStorage
    {
        private ICollection<Area> _areas = new List<Area>(10);
        private ICollection<SpData> _areaPathDatas = new List<SpData>(10);

        public void Add(Area entity)
        {
            _areas.Add(entity);

            var path = file.Write(entity, folderPath);

            _areaPathDatas.Add(new(path));
        }

        public async Task AddAsync(Area entity)
        {
            _areas.Add(entity);

            var path = await file.WriteAsync(entity, folderPath);

            _areaPathDatas.Add(new (path));
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
                        _areas.ElementAt(i).AreaType, _areas.ElementAt(i).ParticlesType,
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
                        _areas.ElementAt(i).AreaType, _areas.ElementAt(i).ParticlesType,
                        null, _areas.ElementAt(i).Particles.Count(),
                        _areaPathDatas.ElementAt(i).Id
                    );

                await postgres.Generations.AddAsync(gen);
            }
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
