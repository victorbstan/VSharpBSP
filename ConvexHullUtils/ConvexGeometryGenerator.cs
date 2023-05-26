using System.Collections.Generic;

namespace VSharpBSP.ConvexHullUtils
{
    using UnityEngine;

    public class ConvexGeometryGenerator : MonoBehaviour
    {
        public MeshFilter meshFilter;

        // public void GenerateConvexGeometry(List<BSPPlane> planes)
        // {
        //     Vector3[] vertices;
        //     int[] triangles;
        //
        //     // Generate vertices and triangles using the provided plane data
        //     GenerateVerticesAndTriangles(planes, out vertices, out triangles);
        //
        //     // Create a new mesh and assign the generated vertices and triangles
        //     Mesh mesh = new Mesh();
        //     mesh.vertices = vertices;
        //     mesh.triangles = triangles;
        //
        //     // Calculate normals and tangents
        //     mesh.RecalculateNormals();
        //     mesh.RecalculateTangents();
        //
        //     // Assign the generated mesh to the MeshFilter component
        //     meshFilter.mesh = mesh;
        // }

        // private void GenerateVerticesAndTriangles(List<BSPPlane> planes, out Vector3[] vertices, out int[] triangles)
        // {
        //     PlaneIntersection planeIntersection = new PlaneIntersection();
        //
        //     List<Vector3> intersectingPointsList = planeIntersection.FindIntersectingPoints(planes);
        //
        //     ConvexHullAlgorithm convexHullAlgorithm = new ConvexHullAlgorithm();
        //     // Create a convex hull using the provided plane data
        //     List<Vector3> convexHull = convexHullAlgorithm.Generate(intersectingPointsList.ToArray());
        //
        //     for (int i = 0; i < planes.Count; i++)
        //     {
        //         PlaneData plane = planes[i];
        //         // convexHull.AddPlane(plane.normal, plane.dist);
        //     }
        //
        //     // Generate vertices and triangles from the convex hull
        //     // convexHull.GenerateMeshData(out vertices, out triangles);
        // }
    }

    public struct PlaneData
    {
        public Vector3 normal;
        public float dist;
    }

}