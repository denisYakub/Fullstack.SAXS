using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Entities.Particles
{
    public class C60 : Particle
    {
        public override double OuterSphereRadius => _outerR;

        public override double InnerSphereRadius => _innerR;

        public override IReadOnlyCollection<Vector3D> Vertices => _vertices;

        public override ParticleTypes ParticleType => ParticleTypes.C60;

        protected override int[][] Faces => _faces;

        private readonly double _outerR;
        private readonly double _innerR;
        private Vector3D[] _vertices;

        private static readonly int[][] _faces = [
            [1, 4, 9, 6, 2],
            [3, 8, 16, 14, 7],
            [5, 11, 19, 21, 12],
            [10, 15, 24, 28, 18],
            [13, 22, 32, 30, 20],
            [17, 27, 37, 36, 26],
            [23, 33, 43, 44, 34],
            [25, 31, 41, 45, 35],
            [29, 40, 50, 49, 39],
            [38, 48, 56, 55, 47],
            [42, 46, 54, 58, 51],
            [52, 53, 57, 60, 59],
            [1, 2, 5, 12, 8, 3],
            [1, 3, 7, 15, 10, 4],
            [2, 6, 13, 20, 11, 5],
            [4, 10, 18, 27, 17, 9],
            [6, 9, 17, 26, 22, 13],
            [7, 14, 23, 34, 24, 15],
            [8, 12, 21, 31, 25, 16],
            [11, 20, 30, 40, 29, 19],
            [14, 16, 25, 35, 33, 23],
            [18, 28, 38, 47, 37, 27],
            [19, 29, 39, 41, 31, 21],
            [22, 26, 36, 46, 42, 32],
            [24, 34, 44, 48, 38, 28],
            [30, 32, 42, 51, 50, 40],
            [33, 35, 45, 53, 52, 43],
            [36, 37, 47, 55, 54, 46],
            [39, 49, 57, 53, 45, 41],
            [43, 52, 59, 56, 48, 44],
            [49, 50, 51, 58, 60, 57],
            [54, 55, 56, 59, 60, 58]
        ];

        public C60(double size, Vector3D center, EulerAngles rotationAngles)
            : base(size, center, rotationAngles)
        {
            _vertices = GenerateVertices(center, size, rotationAngles);

            _outerR = 0;
            _innerR = double.MaxValue;

            var massCenter = new Vector3D(
                _vertices.Average(v => v.X),
                _vertices.Average(v => v.Y),
                _vertices.Average(v => v.Z)
            );

            foreach (var v in _vertices)
            {
                var r = Vector3D.Distance(v, center);

                if (r > _outerR)
                    _outerR = r;

                if (r < _innerR)
                    _innerR = r;
            }
        }

        private static Vector3D[] GenerateVertices(Vector3D center, double size, EulerAngles rotationAngles)
        {
            Vector3D[] baseVertices = new Vector3D[]
            {
                new(-0.5836751, -0.6928964,  0.4708062),
                new( 0.8025761,  0.5979962,  0.2017741),
                new( 0.9602837, -0.0848813,  0.3362902),
                new( 0.4899547,  0.8302373,  0.3362902),
                new( 0.7222343, -0.6928964,  0.2017741),
                new(-0.8600213,  0.5293258,  0.1503935),
                new(-0.9517203, -0.1535518,  0.3362902),
                new(-0.1793548,  0.9938080,  0.1503935),
                new( 0.3819010, -0.9251375,  0.2017741),
                new(-0.2710537, -0.9251375,  0.3362902),
                new(-0.8494363, -0.5293258,  0.2017741),
                new( 0.8494363,  0.5293258, -0.2017741),
                new( 1.0071440, -0.1535518, -0.0672580),
                new( 0.2241935,  0.9938080,  0.0672580),
                new( 0.8600213, -0.5293258, -0.1503935),
                new(-0.7222343,  0.6928964, -0.2017741),
                new(-1.0071440,  0.1535518,  0.0672580),
                new(-0.3819010,  0.9251375, -0.2017741),
                new( 0.1793548, -0.9938080, -0.1503935),
                new(-0.2241935, -0.9938080, -0.0672580),
                new(-0.8025761, -0.5979962, -0.2017741),
                new( 0.5836751,  0.6928964, -0.4708062),
                new( 0.9517203,  0.1535518, -0.3362902),
                new( 0.2710537,  0.9251375, -0.3362902),
                new( 0.6574750, -0.5979962, -0.5025612),
                new(-0.7841999,  0.4182147, -0.5025612),
                new(-0.9602837,  0.0848813, -0.3362902),
                new(-0.1035333,  0.8826969, -0.5025612),
                new( 0.3171418, -0.8302373, -0.5025612),
                new(-0.4899547, -0.8302373, -0.3362902),
                new(-0.8579998, -0.2908927, -0.4708062),
                new( 0.5217096,  0.4182147, -0.7715933),
                new( 0.7491740,  0.0848813, -0.6884578),
                new( 0.6020514, -0.2908927, -0.7715933),
                new(-0.5058321,  0.3757740, -0.8033483),
                new(-0.1654988,  0.6080151, -0.8033483),
                new( 0.0513806, -0.6666667, -0.7715933),
                new(-0.3521676, -0.6666667, -0.6884578),
                new(-0.5796320, -0.3333333, -0.7715933),
                new( 0.1471226,  0.3757740, -0.9378643),
                new( 0.2274644, -0.3333333, -0.9378643),
                new(-0.4035482,  0.0000000, -0.9378643),
                new( 0.0000000,  0.0000000, -1.0210000),
            };

            var rotationMatrix = rotationAngles.CreateRotationMatrix();
            var result = new Vector3D[baseVertices.Length];

            for (int i = 0; i < baseVertices.Length; i++)
            {
                var scaled = baseVertices[i] * size;
                var rotated = Vector3D.Transform(scaled, rotationMatrix);
                result[i] = rotated + center;
            }

            return result;
        }
    }
}
