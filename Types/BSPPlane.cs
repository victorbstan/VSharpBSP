using UnityEngine;

namespace VSharpBSP
{
    
    public class BSPPlane
    {
        // http://www.mralligator.com/q3/#Planes
        // float[3]     normal 	Plane normal.
        // float dist 	Distance from origin to plane along normal. 
        
        // Note that planes are paired. The pair of planes with indices i and i ^ 1 are coincident planes with opposing normals. 

        public Vector3 normal;
        public float distance;

        public BSPPlane(Vector3 norm, float dist)
        {
            // this.normal = norm;
            this.normal = new Vector3(norm.x, norm.z, -norm.y);
            // this.normal = CoordinateTransformer.TransformIdTech3NormalToUnity(this.normal);
            this.normal.Normalize();
            
            // this.distance = dist;
            this.distance = dist * Constants.f_scaleMultiple;
        }
    }
}