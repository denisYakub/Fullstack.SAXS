using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Entities.Particles
{
    public class C540 : Particle
    {
        public override double OuterSphereRadius => _outerR;

        public override double InnerSphereRadius => _innerR;

        public override IReadOnlyCollection<Vector3D> Vertices => _vertices;

        public override ParticleTypes ParticleType => ParticleTypes.C540;

        protected override int[][] Faces => _faces;

        private readonly double _outerR;
        private readonly double _innerR;
        private Vector3D[] _vertices;

        private static readonly int[][] _faces = [

        ];

        public C540(double size, Vector3D center, EulerAngles rotationAngles)
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
            ;
        }

        private static Vector3D[] GenerateVertices(Vector3D center, double size, EulerAngles rotationAngles)
        {
            var baseVertices = new Vector3D[]
            {

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
