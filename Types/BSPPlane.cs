using UnityEngine;
using UnityEngine.ProBuilder;

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
            this.normal = norm;
            this.distance = dist;

            // Swizzle A
            // this.normal = new Vector3(
            //     -norm.x,
            //     norm.z,
            //     -norm.y
            // );
            
            // Swizzle B
            // float tempx = -norm.x;
            // float tempy = norm.z;
            // float tempz = -norm.y;
            // this.normal = new Vector3(tempx, tempy, tempz);
            
            // Swizzle C
            // float tempZ = normal.z;
            // float tempY = normal.y;
            // normal.x = -normal.x;
            // normal.y = tempZ;
            // normal.z = -tempY;
            //
            // this.distance = dist * Constants.f_scaleMultiple;
        }
    }
}