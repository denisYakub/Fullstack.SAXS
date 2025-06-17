using System.Numerics;
using Fullstack.SAXS.Server.Domain.Enums;
using Fullstack.SAXS.Server.Domain.ValueObjects;

namespace Fullstack.SAXS.Server.Domain.Entities.Particles
{
    public class IcosahedronParticle : Particle
    {
        public override float Volume => GenerateVolume();

        public override float OuterSphereRadius => _outerR;

        public override float InnerSphereRadius => _innerR;

        public override ReadOnlySpan<int[]> Faces => _faces;

        public override ParticleTypes ParticleType => ParticleTypes.Icosahedron;

        public override ReadOnlySpan<Vector3> Vertices => _vertices;

        private readonly float _outerR;
        private readonly float _innerR;
        private Vector3[] _vertices;
        private static readonly int[][] _faces = [
            [0, 11, 5],
            [0, 5, 1],
            [0, 1, 7],
            [0, 7, 10],
            [0, 10, 11],
            [1, 5, 9],
            [5, 11, 4],
            [11, 10, 2],
            [10, 7, 6],
            [7, 1, 8],
            [3, 9, 4],
            [3, 4, 2],
            [3, 2, 6],
            [3, 6, 8],
            [3, 8, 9],
            [4, 9, 5],
            [2, 4, 11],
            [6, 2, 10],
            [8, 6, 7],
            [9, 8, 1]
        ];

        public IcosahedronParticle(float size, Vector3 center, EulerAngles rotationAngles)
            : base(size, center, rotationAngles)
        {
            var _phi = (1 + MathF.Sqrt(5)) / 2;

            _vertices = new Vector3[42];

            _vertices[0] = new Vector3(-1, _phi, 0) * size;
            _vertices[1] = new Vector3(1, _phi, 0) * size;
            _vertices[2] = new Vector3(-1, -_phi, 0) * size;
            _vertices[3] = new Vector3(1, -_phi, 0) * size;

            _vertices[4] = new Vector3(0, -1, _phi) * size;
            _vertices[5] = new Vector3(0, 1, _phi) * size;
            _vertices[6] = new Vector3(0, -1, -_phi) * size;
            _vertices[7] = new Vector3(0, 1, -_phi) * size;

            _vertices[8] = new Vector3(_phi, 0, -1) * size;
            _vertices[9] = new Vector3(_phi, 0, 1) * size;
            _vertices[10] = new Vector3(-_phi, 0, -1) * size;
            _vertices[11] = new Vector3(-_phi, 0, 1) * size;

            var edgeSet = new HashSet<int>();

            int index = 12;

            static int EncodeEdge(int a, int b) => Math.Min(a, b) << 10 | Math.Max(a, b);

            foreach (var face in _faces)
            {
                for (int i = 0; i < face.Length; i++)
                {
                    if (index >= _vertices.Length)
                        break;

                    int v1 = face[i];
                    int v2 = face[(i + 1) % face.Length];

                    int code = EncodeEdge(v1, v2);
                    if (!edgeSet.Add(code)) continue;

                    _vertices[index++] = (_vertices[v1] + _vertices[v2]) * 0.5f;
                }
            }

            for (int i = 0; i < index; i++)
            {
                _vertices[i] = Vector3
                    .Transform(
                        _vertices[i], 
                        rotationAngles.CreateRotationMatrix()
                    );
                _vertices[i] += center;
            }

            var edge = Vector3
                .Distance(_vertices[_faces[0][0]], _vertices[_faces[0][1]]) * size;
            _outerR = 0.951f * edge;
            _innerR = 0.7557f * edge;
        }

        public float GenerateVolume()
        {
            int numberOfDotsInsideFullerene = 0;
            int samples = 1_000_000;
            float radius = _outerR;

            var random = new Random();

            for (int i = 0; i < samples; i++)
            {
                Vector3 dot;

                do
                {
                    float x = (float)(random.NextDouble() * 2 - 1);
                    float y = (float)(random.NextDouble() * 2 - 1);
                    float z = (float)(random.NextDouble() * 2 - 1);

                    dot = new Vector3(x, y, z);
                }
                while (dot.LengthSquared() > 1);

                dot *= radius;

                if (Contains(dot))
                    numberOfDotsInsideFullerene++;
            }

            float outerSphereVolume = 4f / 3f * MathF.PI * MathF.Pow(radius, 3);

            return outerSphereVolume * numberOfDotsInsideFullerene / samples;
        }
    }
}
