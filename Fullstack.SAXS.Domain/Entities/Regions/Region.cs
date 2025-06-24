using System.Numerics;
using Fullstack.SAXS.Domain.Entities.Particles;

namespace Fullstack.SAXS.Domain.Entities.Regions
{
    public class Region
    {
        private Vector3 _center;
        private readonly float _edge;

        public Region(Vector3 center, float edge) =>
            (_center, _edge) = (center, edge);

        public bool Contains(Particle particle)
        {
            var fullereneR = particle.OuterSphereRadius;

            return
                _center.X - _edge / 2 + fullereneR <= particle.Center.X &&
                particle.Center.X <= _center.X + _edge / 2 - fullereneR &&
                _center.Y - _edge / 2 + fullereneR <= particle.Center.Y &&
                particle.Center.Y <= _center.Y + _edge / 2 - fullereneR &&
                _center.Z - _edge / 2 + fullereneR <= particle.Center.Z &&
                particle.Center.Z <= _center.Z + _edge / 2 - fullereneR;
        }

        public int MaxDepth(float maxSize)
        {
            int count = 0;
            float a = _edge;

            while (a > maxSize)
            {
                a /= 2;
                count++;
            }

            return count;
        }

        public Region[] OctoSplit()
        {
            var halfEdge = _edge / 2;
            var dCoord = _edge / 4;

            return [
                new (new (_center.X - dCoord, _center.Y - dCoord, _center.Z - dCoord), halfEdge),
                new (new (_center.X - dCoord, _center.Y - dCoord, _center.Z + dCoord), halfEdge),
                new (new (_center.X - dCoord, _center.Y + dCoord, _center.Z - dCoord), halfEdge),
                new (new (_center.X - dCoord, _center.Y + dCoord, _center.Z + dCoord), halfEdge),
                new (new (_center.X + dCoord, _center.Y - dCoord, _center.Z - dCoord), halfEdge),
                new (new (_center.X + dCoord, _center.Y - dCoord, _center.Z + dCoord), halfEdge),
                new (new (_center.X + dCoord, _center.Y + dCoord, _center.Z - dCoord), halfEdge),
                new (new (_center.X + dCoord, _center.Y + dCoord, _center.Z + dCoord), halfEdge),
            ];
        }
    }
}
