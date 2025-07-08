using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Entities.Particles
{
    public class C70 : Particle
    {
        public override double OuterSphereRadius => _outerR;

        public override double InnerSphereRadius => _innerR;

        public override IReadOnlyCollection<Vector3D> Vertices => _vertices;

        public override ParticleTypes ParticleType => ParticleTypes.C70;

        protected override int[][] Faces => _faces;

        private readonly double _outerR;
        private readonly double _innerR;
        private readonly Vector3D[] _vertices;

        private static readonly int[][] _faces = [
            [0, 1, 10, 2],
            [1, 3, 0, 15],
            [2, 9, 0, 39],
            [3, 1, 4, 37],
            [4, 3, 5, 48],
            [5, 4, 6, 14],
            [6, 67, 5, 49],
            [7, 8, 20, 9],
            [8, 10, 7, 25],
            [9, 19, 2, 7],
            [10, 8, 0, 11],
            [11, 10, 12, 15],
            [12, 11, 13, 24],
            [13, 63, 12, 16],
            [14, 15, 16, 5],
            [15, 1, 14, 11],
            [16, 68, 14, 13],
            [17, 30, 18, 19],
            [18, 20, 17, 35],
            [19, 9, 29, 17],
            [20, 18, 7, 21],
            [21, 20, 22, 25],
            [22, 21, 23, 34],
            [23, 59, 22, 26],
            [24, 25, 26, 12],
            [25, 8, 24, 21],
            [26, 64, 24, 23],
            [27, 40, 28, 29],
            [28, 30, 45, 27],
            [29, 19, 39, 27],
            [30, 28, 17, 31],
            [31, 30, 32, 35],
            [32, 31, 33, 44],
            [33, 55, 32, 36],
            [34, 36, 35, 22],
            [35, 18, 34, 31],
            [36, 60, 34, 33],
            [37, 39, 3, 38],
            [38, 40, 37, 48],
            [39, 37, 29, 2],
            [40, 38, 27, 41],
            [41, 40, 42, 45],
            [42, 41, 43, 47],
            [43, 51, 42, 46],
            [44, 45, 46, 32],
            [45, 28, 44, 41],
            [46, 56, 44, 43],
            [47, 49, 48, 42],
            [48, 38, 47, 4],
            [49, 52, 47, 6],
            [50, 51, 56, 53],
            [51, 52, 50, 43],
            [52, 51, 66, 49],
            [53, 57, 50, 69],
            [54, 60, 55, 57],
            [55, 56, 54, 33],
            [56, 55, 50, 46],
            [57, 61, 54, 53],
            [58, 64, 59, 61],
            [59, 60, 58, 23],
            [60, 59, 54, 36],
            [61, 57, 65, 58],
            [62, 68, 63, 65],
            [63, 64, 62, 13],
            [64, 63, 58, 26],
            [65, 61, 69, 62],
            [66, 52, 69, 67],
            [67, 68, 66, 6],
            [68, 67, 62, 16],
            [69, 66, 65, 53]
        ];
        private static readonly Vector3D[] _baseVertices = [
            new(3.1004, -2.2295, 0.6489),
            new(2.3569, -2.2755, 1.8062),
            new(3.8685, -1.1257, 0.3176),
            new(2.3606, -1.1905, 2.6493),
            new(1.1889, -0.806, 3.2678),
            new(0.0054, -1.5495, 3.1503),
            new(-1.1776, -0.8038, 3.2693),
            new(3.104, -1.3076, -1.9212),
            new(2.3587, -2.4209, -1.6048),
            new(3.8734, -0.6492, -0.9785),
            new(2.356, -2.8828, -0.312),
            new(1.1847, -3.3637, 0.2396),
            new(-0.0032, -3.483, -0.4919),
            new(-1.1925, -3.3624, 0.2385),
            new(-0.0013, -2.6864, 2.2726),
            new(1.1858, -2.9755, 1.5923),
            new(-1.1901, -2.9717, 1.5925),
            new(3.1243, 1.4331, -1.8401),
            new(2.3724, 0.7796, -2.7964),
            new(3.8842, 0.7299, -0.9256),
            new(2.3581, -0.5916, -2.8349),
            new(1.186, -1.2682, -3.1094),
            new(0.006, -0.5953, -3.4519),
            new(-1.1784, -1.2662, -3.1103),
            new(-0.0023, -2.9961, -1.8463),
            new(1.1854, -2.4385, -2.3322),
            new(-1.1899, -2.4367, -2.3305),
            new(3.1097, 2.1786, 0.7951),
            new(2.3653, 2.8928, -0.1246),
            new(3.8819, 1.0975, 0.4049),
            new(2.3683, 2.517, -1.4465),
            new(1.1951, 2.5828, -2.1701),
            new(0.0063, 3.1074, -1.6485),
            new(-1.1842, 2.5842, -2.1688),
            new(0.0053, 0.8398, -3.4103),
            new(1.1958, 1.4719, -3.0317),
            new(-1.1833, 1.4729, -3.0317),
            new(3.1207, -0.0822, 2.3341),
            new(2.373, 1.0126, 2.7274),
            new(3.8813, -0.0445, 1.1832),
            new(2.3643, 2.146, 1.9527),
            new(1.1934, 2.8555, 1.7754),
            new(0.0123, 2.5175, 2.4494),
            new(-1.1709, 2.8558, 1.7768),
            new(0.0069, 3.5053, -0.2662),
            new(1.1932, 3.3377, 0.4557),
            new(-1.1797, 3.3365, 0.4553),
            new(0.0069, 1.3342, 3.261),
            new(1.1942, 0.5964, 3.3247),
            new(-1.1814, 0.6002, 3.3224),
            new(-3.0824, 2.1894, 0.7972),
            new(-2.3477, 2.1489, 1.9624),
            new(-2.3589, 1.0138, 2.7339),
            new(-3.8435, 1.1037, 0.392),
            new(-3.1028, 1.4396, -1.8469),
            new(-2.362, 2.5343, -1.4474),
            new(-2.3551, 2.9083, -0.1298),
            new(-3.8509, 0.7183, -0.9357),
            new(-3.0945, -1.3133, -1.9306),
            new(-2.3558, -0.5859, -2.8448),
            new(-2.3584, 0.784, -2.8061),
            new(-3.8515, -0.6622, -0.9707),
            new(-3.0979, -2.2406, 0.6537),
            new(-2.3693, -2.8978, -0.3196),
            new(-2.3675, -2.4358, -1.6091),
            new(-3.854, -1.123, 0.3324),
            new(-3.1072, -0.0741, 2.3435),
            new(-2.36, -1.1942, 2.6586),
            new(-2.3623, -2.2781, 1.8184),
            new(-3.8571, -0.0346, 1.185)
        ];

        public C70(double size, Vector3D center, EulerAngles rotationAngles)
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
