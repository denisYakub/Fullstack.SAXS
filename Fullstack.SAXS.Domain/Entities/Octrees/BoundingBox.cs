using System.Numerics;

namespace Fullstack.SAXS.Domain.Entities.Octrees
{
    public class BoundingBox
    {
        public float Edge {  get; init; }
        public Vector3 Center { get; init; }

        public BoundingBox(float edge, Vector3 center)
        {
            Edge = edge;
            Center = center;
        }

        public BoundingBox[] OctoSplit()
        {
            var halfEdge = Edge / 2;
            var dCoord = Edge / 4;

            return [
                new (halfEdge, new (Center.X - dCoord, Center.Y - dCoord, Center.Z - dCoord)),
                new (halfEdge, new (Center.X - dCoord, Center.Y - dCoord, Center.Z + dCoord)),
                new (halfEdge, new (Center.X - dCoord, Center.Y + dCoord, Center.Z - dCoord)),
                new (halfEdge, new (Center.X - dCoord, Center.Y + dCoord, Center.Z + dCoord)),
                new (halfEdge, new (Center.X + dCoord, Center.Y - dCoord, Center.Z - dCoord)),
                new (halfEdge, new (Center.X + dCoord, Center.Y - dCoord, Center.Z + dCoord)),
                new (halfEdge, new (Center.X + dCoord, Center.Y + dCoord, Center.Z - dCoord)),
                new (halfEdge, new (Center.X + dCoord, Center.Y + dCoord, Center.Z + dCoord)),
            ];
        }

        public bool Contains(Vector3 point)
        {
            var half = new Vector3(Edge / 2);
            var min = Center - half;
            var max = Center + half;

            return point.X >= min.X && point.X <= max.X &&
                   point.Y >= min.Y && point.Y <= max.Y &&
                   point.Z >= min.Z && point.Z <= max.Z;
        }
    }
}
