namespace VSharpBSP.ConvexHullUtils
{
    using UnityEngine;

    public class CoordinateTransformer
    {
        public static Vector3 TransformIdTech3NormalToUnity(Vector3 idTech3Normal)
        {
            // Swap the Y and Z components to account for the difference in coordinate space
            float unityX = idTech3Normal.x;
            float unityY = idTech3Normal.z;
            float unityZ = idTech3Normal.y;

            return new Vector3(unityX, unityY, unityZ);
        }
    }

}