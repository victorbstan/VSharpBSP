using System;
using System.Collections.Generic;
using UnityEngine;

namespace VSharpBSP
{
    public static class Util
    {
        // This converts the verts from the format Q3 uses to the one Unity3D uses.
        // Look up the Q3 map/rendering specs if you want the details.
        // Quake3 also uses an odd scale where ~0.032 units is about 1 meter, so scale it way down
        // while you're at it.
        public static Vector3 Swizzle3(Vector3 vector, bool scale = true)
        {
            // Quake axis: x: depth, y: horizontal, z: vertical
            // Unity axis: x: horizontal, y: vertical, z: depth
            float tempZ = vector.z;
            float tempY = vector.y;
            vector.y = tempZ;
            vector.z = -tempY;
            vector.x = -vector.x;

            if (scale)
                vector *= Constants.f_scaleMultiple; // Scale
            return vector;
        }
        
        public static Vector3 AngleToVector3(float ang)
        {
            return Quaternion.AngleAxis(ang, Vector3.down) * Vector3.left;
        }
        
        
        public static void DrawArrow(Vector3 startPos, Vector3 endPos)
        {
            Vector3 angleVectorUp = new Vector3(0f, 0.40f,-1f)*0.2f; // length
            Vector3 angleVectorDown = new Vector3(0f, -0.40f,-1f)*0.2f; // length
            
            Vector3 arrowDirection = endPos - startPos;
            Vector3 arrowPos = startPos + (arrowDirection * 0.9f); // position along line
            
            Vector3 upTmp = Quaternion.LookRotation(arrowDirection) * angleVectorUp ;
            Vector3 downTmp = Quaternion.LookRotation(arrowDirection) * angleVectorDown;
            
            Gizmos.DrawLine(startPos, endPos);
            Gizmos.DrawRay(arrowPos, upTmp);
            Gizmos.DrawRay(arrowPos, downTmp);
        }
        
        public static Vector3 ClampMagnitude(Vector3 v, float max, float min)
        {
            double sm = v.sqrMagnitude;
            if(sm > (double)max * (double)max) return v.normalized * max;
            else if(sm < (double)min * (double)min) return v.normalized * min;
            return v;
        }
        
        public static bool StrStartsWithAny(string stringToTest, List<string> substrings)
        {
            if (string.IsNullOrEmpty(stringToTest) || substrings == null)
                return false;

            foreach (var substring in substrings)
            {
                if (stringToTest.StartsWith(substring, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        
        public static bool StrEqualsAny(string stringToTest, List<string> substrings)
        {
            if (string.IsNullOrEmpty(stringToTest) || substrings == null)
                return false;

            foreach (var substring in substrings)
            {
                if (stringToTest.Equals(substring, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        
        public static bool ContainsAny(string stringToTest, List<string> substrings)
        {
            if (string.IsNullOrEmpty(stringToTest) || substrings == null)
                return false;

            foreach (var substring in substrings)
            {
                if (stringToTest.Contains(substring, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        
        // Used to calculate mid point of such things as bounding boxes,
        // or anything that has a min & max vector
        public static Vector3 GetMidPoint(Vector3 start, Vector3 end, bool scale = false)
        {
            Vector3 difference = start - end;
            Vector3 quarterPoint = start + difference * 0.25f;
            Vector3 midPoint = start + difference * 0.5f;

            if (scale)
                return midPoint * Constants.f_scaleMultiple;
            
            return midPoint;
        }
    }
}

