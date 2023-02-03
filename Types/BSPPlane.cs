using UnityEngine;
using UnityEngine.ProBuilder;

namespace VSharpBSP
{
    
    public class BSPPlane
    {
        // http://www.mralligator.com/q3/#Planes
        // float[3]     normal 	Plane normal.
        // float dist 	Distance from origin to plane along normal. 

        public Vector3 normal;
        public float dist;

        public BSPPlane(Vector3 normal, float dist)
        {
            this.normal = normal;
            this.dist = dist;
        }
    }
}