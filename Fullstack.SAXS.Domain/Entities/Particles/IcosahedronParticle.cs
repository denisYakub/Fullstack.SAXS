using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Entities.Particles
{
    public class IcosahedronParticle : Particle
    {
        public override double OuterSphereRadius => _outerR;

        public override double InnerSphereRadius => _innerR;

        protected override IReadOnlyCollection<int[]> Faces => _faces;

        public override IReadOnlyCollection<Vector3D> Vertices => _vertices;

        public override ParticleTypes ParticleType => ParticleTypes.Icosahedron;

        private readonly double _outerR;
        private readonly double _innerR;
        private Vector3D[] _vertices;

        private static readonly int[][] _faces = [
            [0, 11, 5], [0, 5, 1],
            [0, 1, 7], [0, 7, 10],
            [0, 10, 11], [1, 5, 9],
            [5, 11, 4], [11, 10, 2],
            [10, 7, 6], [7, 1, 8],
            [3, 9, 4], [3, 4, 2],
            [3, 2, 6], [3, 6, 8],
            [3, 8, 9], [4, 9, 5],
            [2, 4, 11], [6, 2, 10],
            [8, 6, 7], [9, 8, 1]
        ];
        private static double _phi = (1 + Math.Sqrt(5)) / 2;
        public IcosahedronParticle(double size, Vector3D center, EulerAngles rotationAngles)
            : base(size, center, rotationAngles)
        {
            _vertices = GenerateVertices(center, size, rotationAngles);

            var edge = GetEdgeSize(size);

            _outerR = 0.951 * edge;
            _innerR = 0.7557 * edge;
        }

        private static Vector3D[] GenerateVertices(Vector3D center, double size, EulerAngles rotationAngles)
        {
            var vertices = new Vector3D[12];

            vertices[0] = new Vector3D(-1, _phi, 0) * size;
            vertices[1] = new Vector3D(1, _phi, 0) * size;
            vertices[2] = new Vector3D(-1, -_phi, 0) * size;
            vertices[3] = new Vector3D(1, -_phi, 0) * size;

            vertices[4] = new Vector3D(0, -1, _phi) * size;
            vertices[5] = new Vector3D(0, 1, _phi) * size;
            vertices[6] = new Vector3D(0, -1, -_phi) * size;
            vertices[7] = new Vector3D(0, 1, -_phi) * size;

            vertices[8] = new Vector3D(_phi, 0, -1) * size;
            vertices[9] = new Vector3D(_phi, 0, 1) * size;
            vertices[10] = new Vector3D(-_phi, 0, -1) * size;
            vertices[11] = new Vector3D(-_phi, 0, 1) * size;


            int index = 12;

            var edgeSet = new HashSet<int>();
            static int EncodeEdge(int a, int b) => Math.Min(a, b) << 10 | Math.Max(a, b);

            foreach (var face in _faces)
            {
                for (int i = 0; i < face.Length; i++)
                {
                    if (index >= vertices.Length)
                        break;

                    int v1 = face[i];
                    int v2 = face[(i + 1) % face.Length];

                    int code = EncodeEdge(v1, v2);
                    if (!edgeSet.Add(code)) continue;

                    vertices[index++] = (vertices[v1] + vertices[v2]) * 0.5f;
                }
            }

            for (int i = 0; i < index; i++)
            {
                vertices[i] = Vector3D
                    .Transform(
                        vertices[i],
                        rotationAngles.CreateRotationMatrix()
                    );
                vertices[i] += center;
            }

            return vertices;
        }

        public double GenerateVolume()
        {
            var numberOfDotsInsideFullerene = 0;
            var samples = 1_000_000;
            var radius = _outerR;

            var random = new Random();

            for (int i = 0; i < samples; i++)
            {
                Vector3D dot;

                do
                {
                    var x = (random.NextDouble() * 2 - 1);
                    var y = (random.NextDouble() * 2 - 1);
                    var z = (random.NextDouble() * 2 - 1);

                    dot = new Vector3D(x, y, z);
                }
                while (dot.LengthSquared() > 1);

                dot *= radius;

                if (Contains(dot))
                    numberOfDotsInsideFullerene++;
            }

            var outerSphereVolume = 4f / 3f * Math.PI * Math.Pow(radius, 3);

            return outerSphereVolume * numberOfDotsInsideFullerene / samples;
        }

        private static double GetEdgeSize(double size)
        {
            return Vector3D
                .Distance(new Vector3D(-1, _phi, 0) * size, new Vector3D(-_phi, 0, 1) * size);
        }
    }
}
