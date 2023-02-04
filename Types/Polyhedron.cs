using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace VSharpBSP
{
    public class Polyhedron
    {
        public readonly BSPPlane[] planes;
        public List<Vector3> pointCloud;

        public Polyhedron(IEnumerable<BSPPlane> planes)
        {
            this.planes = planes.ToArray();
            this.pointCloud = new List<Vector3>();
            this.PointCloud();
        }

        public List<Vector3> PointCloud()
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
                            pointCloud.Add(point);
                        }
                    }
                }
            }

            return pointCloud;
        }

        private bool GetPlaneIntersectionPoint(BSPPlane p0, BSPPlane p1, BSPPlane p2, out Vector3 intersectionPoint)
        {
            const float EPSILON = 1e-4f;

            var det = Vector3.Dot(Vector3.Cross(p0.normal, p1.normal), p2.normal);
            if (det < EPSILON)
            {
                intersectionPoint = Vector3.zero;
                return false;
            }

            intersectionPoint =
                (-(p0.distance * Vector3.Cross(p1.normal, p2.normal)) -
                 (p1.distance * Vector3.Cross(p2.normal, p0.normal)) -
                 (p2.distance * Vector3.Cross(p0.normal, p1.normal))) / det;

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