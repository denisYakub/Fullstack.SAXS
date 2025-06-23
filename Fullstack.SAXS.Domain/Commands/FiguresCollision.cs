using System.Numerics;

namespace Fullstack.SAXS.Domain.Commands
{
    public static class FiguresCollision
    {
        /// <summary>
        /// Checks if at least one sphere is 
        /// contained within another
        /// </summary>
        /// <param name="innerC">Vector3 center of the inner shpere</param>
        /// <param name="r1">float radius of the inner shpere</param>
        /// <param name="center2">Vector3 center of the outer shpere</param>
        /// <param name="outerR">float radius of the outer shpere</param>
        /// <returns>true if contains, else false</returns>
        public static bool SpheresInside(
            in Vector3 innerC, float innerR,
            in Vector3 outerC, float outerR
        )
        {
            var distanceSquar = Vector3.DistanceSquared(innerC, outerC);
            float radiusDiff = outerR - innerR;

            return radiusDiff >= 0 && distanceSquar <= radiusDiff * radiusDiff;
        }
        /// <summary>
        /// Checks two spheres intersection
        /// </summary>
        /// <param name="center1">Center of the first shpere</param>
        /// <param name="r1">Radius of the first shpere</param>
        /// <param name="center2">Center of the second shpere</param>
        /// <param name="r2">Radius of the second shpere</param>
        /// <returns>true if intersect, else false</returns>
        public static bool SpheresIntersect(
            in Vector3 center1, float r1,
            in Vector3 center2, float r2
        )
        {
            return Vector3.Distance(center2, center1) < (r1 + r2) * (r1 + r2);
        }
        /// <summary>
        /// Checks if point inside sphere
        /// </summary>
        /// <param name="sphereCenter">Center of th sphere</param>
        /// <param name="sphereRadius">Radius of the sphere</param>
        /// <param name="point"></param>
        /// <returns>true if point inside</returns>
        public static bool Pointinside(in Vector3 sphereCenter, float sphereRadius, in Vector3 point)
        {
            float distanceSquared = Vector3.DistanceSquared(point, sphereCenter);
            return distanceSquared <= sphereRadius * sphereRadius;
        }
    }
}
