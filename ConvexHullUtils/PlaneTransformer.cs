namespace VSharpBSP.ConvexHullUtils
{
    using UnityEngine;

    public class PlaneTransformer
    {
        public static Plane TransformIdTech3PlaneToUnity(BSPPlane bspPlane)
        {
            Vector3 idTech3Normal = bspPlane.normal;
            float idTech3Dist = bspPlane.distance;
                
            // Transform the id Tech 3 normal to Unity 3D normal
            // Vector3 unityNormal = idTech3Normal;
            Vector3 unityNormal = Util.Swizzle3(idTech3Normal);

            // Calculate the Unity 3D distance value
            float unityDist = idTech3Dist * unityNormal.magnitude;

            // Create and return the Unity 3D plane
            return new Plane(unityNormal, unityDist);
            // return new Plane(idTech3Normal, unityDist);
        }
    }

}