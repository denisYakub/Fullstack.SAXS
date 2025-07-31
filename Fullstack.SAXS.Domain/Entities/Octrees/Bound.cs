using Fullstack.SAXS.Domain.Entities.Particles;
using Fullstack.SAXS.Domain.Enums;
using Fullstack.SAXS.Domain.ValueObjects;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class Bound(int edge, Vector3D center, int depth = 0)
    {
        public int Edge { get { return edge; } }
        public Vector3D Center { get { return center; } }
        public int Depth { get {  return depth; } }

        public Bound(double edge, Vector3D center)
            : this(1 << (int)Math.Ceiling(Math.Log(edge, 2)), center) { }

        public OctantId GetIdOfContainingSubBound(Particle particle)
        {
            if (particle == null)
                throw new ArgumentNullException(nameof(particle), "Shouldn't be null.");

            var subBounds = OctoSplit(Edge, center, Depth);

            return particle switch
            {
                Particle p when subBounds[0].Contains(p) => OctantId.First,
                Particle p when subBounds[1].Contains(p) => OctantId.Second,
                Particle p when subBounds[2].Contains(p) => OctantId.Third,
                Particle p when subBounds[3].Contains(p) => OctantId.Fourth,
                Particle p when subBounds[4].Contains(p) => OctantId.Fifth,
                Particle p when subBounds[5].Contains(p) => OctantId.Sixth,
                Particle p when subBounds[6].Contains(p) => OctantId.Seventh,
                Particle p when subBounds[7].Contains(p) => OctantId.Eighth,
                _ => OctantId.None,
            };
        }

        public bool Contains(Particle particle)
        {
            if (particle == null)
                throw new ArgumentNullException(nameof(particle), "Shouldn't be null.");

            var half = Edge >> 1;

            return
                particle.Center.X - particle.OuterSphereRadius >= center.X - half &&
                particle.Center.X + particle.OuterSphereRadius <= center.X + half &&
                particle.Center.Y - particle.OuterSphereRadius >= center.Y - half &&
                particle.Center.Y + particle.OuterSphereRadius <= center.Y + half &&
                particle.Center.Z - particle.OuterSphereRadius >= center.Z - half &&
                particle.Center.Z + particle.OuterSphereRadius <= center.Z + half;
        }

        public static Bound[] OctoSplit(int edge, Vector3D center, int depth)
        {
            var halfEdge = edge >> 1;
            var quarter = edge >> 2;
            var goDeeper = depth + 1;

            var offsets = new[]
            {
                new Vector3D(center.X - quarter, center.Y - quarter, center.Z - quarter),
                new Vector3D(center.X - quarter, center.Y - quarter, center.Z + quarter),
                new Vector3D(center.X - quarter, center.Y + quarter, center.Z - quarter),
                new Vector3D(center.X - quarter, center.Y + quarter, center.Z + quarter),
                new Vector3D(center.X + quarter, center.Y - quarter, center.Z - quarter),
                new Vector3D(center.X + quarter, center.Y - quarter, center.Z + quarter),
                new Vector3D(center.X + quarter, center.Y + quarter, center.Z - quarter),
                new Vector3D(center.X + quarter, center.Y + quarter, center.Z + quarter)
            };

            return offsets
                .Select(o => new Bound(halfEdge, o, goDeeper))
                .ToArray();
        }
    }
}
