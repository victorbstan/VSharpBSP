using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace VSharpBSP
{
    public class Polyhedron
    {
        public readonly BSPPlane[] planes;
        public HashSet<Vector3> vertices;
        public int[] triangles;

        public Polyhedron(IEnumerable<BSPPlane> newPlanes)
        {
            this.planes = newPlanes.ToArray();
            this.vertices = new HashSet<Vector3>();
            this.ProcessVertices();
            // this.triangles = ConvexHull.Generate(this.vertices.ToArray());
        }

        public void ProcessVertices()
        {
            for (int i = 0; i < planes.Length; i++)
            {
                BSPPlane p1 = planes[i];

                for (int j = i + 1; j < planes.Length; j++)
                {
                    BSPPlane p2 = planes[j];

                    for (int k = j + 1; k < planes.Length; k++)
                    {
                        BSPPlane p3 = planes[k];

                        Vector3 point = new Vector3();
                        if (GetPlaneIntersectionPoint(p1, p2, p3, out point))
                        {
                            this.vertices.Add(point);
                        }
                    }
                }
            }
        }

        private bool GetPlaneIntersectionPoint(BSPPlane p0, BSPPlane p1, BSPPlane p2, out Vector3 intersectionPoint)
        {
            const float EPSILON = 1e-4f;

            var dot = Vector3.Dot(Vector3.Cross(p0.normal, p1.normal), p2.normal);
            if (dot < float.Epsilon)
            // if (dot < EPSILON)
            {
                intersectionPoint = Vector3.zero;
                return false;
            }

            intersectionPoint =
                (-(p0.distance * Vector3.Cross(p1.normal, p2.normal)) -
                 (p1.distance * Vector3.Cross(p2.normal, p0.normal)) -
                 (p2.distance * Vector3.Cross(p0.normal, p1.normal))) / dot;

            return true;
        }

        // public bool PointInHull(BSPPlane[] pointPlanes, Vector3 point)
        // {
        //     for (int i = 0; i < pointPlanes.Length; i++)
        //     {
        //         BSPPlane plane = pointPlanes[i];
        //         float dist = Vector3.Dot(point, plane.normal) - plane.distance;
        //         if (dist > 0.01) return false; // indicates the point lies in outside the hull
        //     }
        //
        //     return true;
        // }
    }
}