namespace VSharpBSP.ConvexHullUtils
{
    using UnityEngine;

    public class PlaneToQuadConverter
    {
        public static GameObject CreateQuadFromPlane(Plane plane)
        {
            // Create a new GameObject to hold the quad
            GameObject quadObject = new GameObject("Quad");
            
            // Create a new MeshFilter and MeshRenderer for the quad
            MeshFilter meshFilter = quadObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = quadObject.AddComponent<MeshRenderer>();
            
            // Create a new mesh for the quad
            Mesh quadMesh = new Mesh();
            
            // Define the vertices of the quad based on the plane's normal and distance
            Vector3 planeNormal = plane.normal;
            float planeDistance = plane.distance;
            Vector3 quadCenter = -planeNormal * planeDistance;
            Vector2 quadSize = new Vector2(10f, 10f); // Customize the size of the quad
            
            Vector3[] vertices =
            {
                quadCenter + quadSize.x * Vector3.right + quadSize.y * Vector3.up,
                quadCenter + quadSize.x * Vector3.right - quadSize.y * Vector3.up,
                quadCenter - quadSize.x * Vector3.right - quadSize.y * Vector3.up,
                quadCenter - quadSize.x * Vector3.right + quadSize.y * Vector3.up
            };
            
            quadMesh.vertices = vertices;
            
            // Define the triangles of the quad
            int[] triangles =
            {
                0, 1, 2,
                2, 3, 0
            };
            
            quadMesh.triangles = triangles;
            
            // Calculate the normals of the quad
            Vector3[] normals = new Vector3[4];
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = planeNormal;
            }
            
            quadMesh.normals = normals;
            
            // Set up the quad's material (you can customize this part)
            meshRenderer.material = new Material(Shader.Find("Standard"));
            
            // Assign the mesh to the MeshFilter
            meshFilter.mesh = quadMesh;
            
            return quadObject;
        }
    }
}