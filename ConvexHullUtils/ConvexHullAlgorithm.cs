namespace VSharpBSP.ConvexHullUtils
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ConvexHullAlgorithm
    {
        public static List<Vector3> Generate(List<Vector3> vertices)
        {
            // Clone the input vertices to avoid modifying the original list
            List<Vector3> clonedVertices = new List<Vector3>(vertices);

            // Sort the vertices by their x-coordinate (or any other coordinate if desired)
            clonedVertices.Sort((a, b) => a.x.CompareTo(b.x));

            // Initialize two empty lists for the upper and lower hulls
            List<Vector3> upperHull = new List<Vector3>();
            List<Vector3> lowerHull = new List<Vector3>();

            // Compute the upper hull
            for (int i = 0; i < clonedVertices.Count; i++)
            {
                while (upperHull.Count >= 2 &&
                       IsTurnLeft(upperHull[upperHull.Count - 2], upperHull[upperHull.Count - 1], clonedVertices[i]))
                {
                    upperHull.RemoveAt(upperHull.Count - 1);
                }

                upperHull.Add(clonedVertices[i]);
            }

            // Compute the lower hull
            for (int i = clonedVertices.Count - 1; i >= 0; i--)
            {
                while (lowerHull.Count >= 2 &&
                       IsTurnLeft(lowerHull[lowerHull.Count - 2], lowerHull[lowerHull.Count - 1], clonedVertices[i]))
                {
                    lowerHull.RemoveAt(lowerHull.Count - 1);
                }

                lowerHull.Add(clonedVertices[i]);
            }

            // Remove the last vertex from each hull (it is common to both hulls)
            upperHull.RemoveAt(upperHull.Count - 1);
            lowerHull.RemoveAt(lowerHull.Count - 1);

            // Combine the upper and lower hulls to form the convex hull
            upperHull.AddRange(lowerHull);
            return upperHull;
        }

        private static bool IsTurnLeft(Vector3 a, Vector3 b, Vector3 c)
        {
            // Check if the three points make a left turn
            float crossProduct = Vector3.Cross(b - a, c - a).y;
            return crossProduct > 0f;
        }
    }

}