using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fullstack.SAXS.Domain.Enums;

namespace Fullstack.SAXS.Domain.Entities.Sp
{
    [Table("sp_generation")]
    public class SpGeneration
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }
        [Column("id_user")]
        public Guid IdUser { get; private set; }
        [Column("generation_number")]
        public long GenNum { get; private set; }
        [Column("series_number")]
        public int SeriesNum { get; private set; }
        [Column("area_type")]
        public string AreaType { get; private set; }
        [Column("particle_type")]
        public string ParticleType { get; private set; }
        [Column("phi")]
        public double? Phi { get; private set; }
        [Column("particle_number")]
        public int ParticleNum { get; private set; }
        [Column("id_sp_data")]
        public Guid IdSpData { get; private set; }
        [ForeignKey(nameof(IdSpData))]
        public SpData? SpData { get; private set; }

        public SpGeneration()
        {
            Id = Guid.Empty;
            IdUser = Guid.Empty;
            GenNum = -1;
            SeriesNum = -1;
            AreaType = string.Empty;
            ParticleType = string.Empty;
            Phi = 0;
            ParticleNum = 0;
            IdSpData = Guid.Empty;
        }

        public SpGeneration(
            Guid idUser,
            long genNum, int seriesNum, 
            AreaTypes areaType, ParticleTypes particleType,
            double? phi, int particleNum,
            Guid idSpData
        )
        {
            Id = Guid.NewGuid();
            IdUser = idUser;
            GenNum = genNum;
            SeriesNum = seriesNum;
            AreaType = areaType.ToString();
            ParticleType = particleType.ToString();
            Phi = phi;
            ParticleNum = particleNum;
            IdSpData = idSpData;
        }

        public SpGeneration(
            Guid id, Guid idUser,
            long genNum, int seriesNum,
            string areaType, string particleType,
            double? phi, int particleNum,
            Guid idSpData
        )
        {
            Id = id;
            IdUser = idUser;
            GenNum = genNum;
            SeriesNum = seriesNum;
            AreaType = areaType;
            ParticleType = particleType;
            Phi = phi;
            ParticleNum = particleNum;
            IdSpData = idSpData;
        }

        public void ChangePhi(double phi)
        {
            Phi = phi;
        }
    }
}
