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

        protected override IReadOnlyCollection<int[]> Faces => _faces;

        private readonly double _outerR;
        private readonly double _innerR;
        private readonly Vector3D[] _vertices;

        private static readonly int[][] _faces = [
            [0, 3, 8, 5, 1], [2, 7, 15, 13, 6],
            [4, 10, 18, 20, 11], [9, 14, 23, 27, 17],
            [12, 21, 31, 29, 19], [16, 26, 36, 35, 25],
            [22, 32, 42, 43, 33], [24, 30, 40, 44, 34],
            [28, 39, 49, 48, 38], [37, 47, 55, 54, 46],
            [41, 45, 53, 57, 50], [51, 52, 56, 59, 58],
            [0, 1, 4, 11, 7, 2], [0, 2, 6, 14, 9, 3],
            [1, 5, 12, 19, 10, 4], [3, 9, 17, 26, 16, 8],
            [5, 8, 16, 25, 21, 12], [6, 13, 22, 33, 23, 14],
            [7, 11, 20, 30, 24, 15], [10, 19, 29, 39, 28, 18],
            [13, 15, 24, 34, 32, 22], [17, 27, 37, 46, 36, 26],
            [18, 28, 38, 40, 30, 20], [21, 25, 35, 45, 41, 31],
            [23, 33, 43, 47, 37, 27], [29, 31, 41, 50, 49, 39],
            [32, 34, 44, 52, 51, 42], [35, 36, 46, 54, 53, 45],
            [38, 48, 56, 52, 44, 40], [42, 51, 58, 55, 47, 43],
            [48, 49, 50, 57, 59, 56], [53, 54, 55, 58, 59, 57]
        ];
        private static readonly Vector3D[] _baseVertices = [
            new(0, 0, 1.021),
            new(0.4035482, 0, 0.9378643),
            new(-0.2274644, 0.3333333, 0.9378643),
            new(-0.1471226, -0.375774, 0.9378643),
            new(0.579632, 0.3333333, 0.7715933),
            new(0.5058321, -0.375774, 0.8033483),
            new(-0.6020514, 0.2908927, 0.7715933),
            new(-0.05138057, 0.6666667, 0.7715933),
            new(0.1654988, -0.6080151, 0.8033483),
            new(-0.5217096, -0.4182147, 0.7715933),
            new(0.8579998, 0.2908927, 0.4708062),
            new(0.3521676, 0.6666667, 0.6884578),
            new(0.7841999, -0.4182147, 0.5025612),
            new(-0.657475, 0.5979962, 0.5025612),
            new(-0.749174, -0.08488134, 0.6884578),
            new(-0.3171418, 0.8302373, 0.5025612),
            new(0.1035333, -0.8826969, 0.5025612),
            new(-0.5836751, -0.6928964, 0.4708062),
            new(0.8025761, 0.5979962, 0.2017741),
            new(0.9602837, -0.08488134, 0.3362902),
            new(0.4899547, 0.8302373, 0.3362902),
            new(0.7222343, -0.6928964, 0.2017741),
            new(-0.8600213, 0.5293258, 0.1503935),
            new(-0.9517203, -0.1535518, 0.3362902),
            new(-0.1793548, 0.993808, 0.1503935),
            new(0.381901, -0.9251375, 0.2017741),
            new(-0.2710537, -0.9251375, 0.3362902),
            new(-0.8494363, -0.5293258, 0.2017741),
            new(0.8494363, 0.5293258, -0.2017741),
            new(1.007144, -0.1535518, -0.06725804),
            new(0.2241935, 0.993808, 0.06725804),
            new(0.8600213, -0.5293258, -0.1503935),
            new(-0.7222343, 0.6928964, -0.2017741),
            new(-1.007144, 0.1535518, 0.06725804),
            new(-0.381901, 0.9251375, -0.2017741),
            new(0.1793548, -0.993808, -0.1503935),
            new(-0.2241935, -0.993808, -0.06725804),
            new(-0.8025761, -0.5979962, -0.2017741),
            new(0.5836751, 0.6928964, -0.4708062),
            new(0.9517203, 0.1535518, -0.3362902),
            new(0.2710537, 0.9251375, -0.3362902),
            new(0.657475, -0.5979962, -0.5025612),
            new(-0.7841999, 0.4182147, -0.5025612),
            new(-0.9602837, 0.08488134, -0.3362902),
            new(-0.1035333, 0.8826969, -0.5025612),
            new(0.3171418, -0.8302373, -0.5025612),
            new(-0.4899547, -0.8302373, -0.3362902),
            new(-0.8579998, -0.2908927, -0.4708062),
            new(0.5217096, 0.4182147, -0.7715933),
            new(0.749174, 0.08488134, -0.6884578),
            new(0.6020514, -0.2908927, -0.7715933),
            new(-0.5058321, 0.375774, -0.8033483),
            new(-0.1654988, 0.6080151, -0.8033483),
            new(0.05138057, -0.6666667, -0.7715933),
            new(-0.3521676, -0.6666667, -0.6884578),
            new(-0.579632, -0.3333333, -0.7715933),
            new(0.1471226, 0.375774, -0.9378643),
            new(0.2274644, -0.3333333, -0.9378643),
            new(-0.4035482, 0, -0.9378643),
            new(0, 0, -1.021)
        ];

        public C60(double size, Vector3D center, EulerAngles rotationAngles)
            : base(size, center, rotationAngles)
        {
            _vertices = GenerateVertices(center, size, rotationAngles);

            _outerR = 0;
            _innerR = double.MaxValue;

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
            var rotationMatrix = rotationAngles.CreateRotationMatrix();
            var result = new Vector3D[_baseVertices.Length];

            for (int i = 0; i < _baseVertices.Length; i++)
            {
                var scaled = _baseVertices[i] * size;
                var rotated = Vector3D.Transform(scaled, rotationMatrix);
                result[i] = rotated + center;
            }

            return result;
        }
    }
}
