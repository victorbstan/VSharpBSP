namespace VSharpBSP.ConvexHullUtils
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PlaneIntersection
    {
        public static List<Vector3> FindIntersectingPoints(List<BSPPlane> planes)
        {
            List<Vector3> intersectingPoints = new List<Vector3>();

            // Iterate through each pair of planes
            for (int i = 0; i < planes.Count - 1; i++)
            {
                for (int j = i + 1; j < planes.Count; j++)
                {
                    BSPPlane planeA = planes[i];
                    BSPPlane planeB = planes[j];

                    // Find the intersection line between the two planes
                    if (IntersectPlanes(planeA, planeB, out Vector3 intersectionPoint))
                    {
                        intersectingPoints.Add(intersectionPoint);
                    }
                }
            }

            return intersectingPoints;
        }

        private static bool IntersectPlanes(BSPPlane planeA, BSPPlane planeB, out Vector3 intersectionPoint)
        {
            // Calculate the direction of the intersection line
            Vector3 lineDirection = Vector3.Cross(planeA.normal, planeB.normal);

            // Check if the planes are parallel or nearly parallel
            float directionMagnitude = lineDirection.magnitude;
            if (directionMagnitude < Mathf.Epsilon)
            {
                intersectionPoint = Vector3.zero;
                return false;
            }

            // Calculate the intersection point between the two planes
            Vector3 lineNormalA = Vector3.Cross(planeA.normal, lineDirection);
            Vector3 lineNormalB = Vector3.Cross(planeB.normal, lineDirection);
            float dotA = Vector3.Dot(lineNormalA, planeA.normal);
            float dotB = Vector3.Dot(lineNormalB, planeB.normal);
            float scaleFactorA = (planeA.distance * dotB - planeB.distance * dotA) / directionMagnitude;
            intersectionPoint = -lineNormalA * scaleFactorA;

            return true;
        }
    }

}