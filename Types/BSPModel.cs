using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace VSharpBSP
{
    public class BSPModel
    {
        public Vector3 bBoxMin;
        public Vector3 bBoxMax;
        public int faceIndex;
        public int faceCount;
        public int brushIndex;
        public int brushCount;

        // http://www.mralligator.com/q3/#Models
        // float[3] mins     Bounding box min coord.
        // float[3] maxs     Bounding box max coord.
        // int face          First face for model.
        // int n_faces       Number of faces for model.
        // int brush         First brush for model.
        // int n_brushes     Number of brushes for model.
        public BSPModel(
            Vector3 bBoxMin,
            Vector3 bBoxMax,
            int faceIndex,
            int faceCount,
            int brushIndex,
            int brushCount)
        {
            this.bBoxMin = bBoxMin;
            this.bBoxMax = bBoxMax;
            this.faceIndex = faceIndex;
            this.faceCount = faceCount;
            this.brushIndex = brushIndex;
            this.brushCount = brushCount;
        }

        public Vector3 Origin()
        {
            Vector3 origS = (bBoxMax + bBoxMin) * 0.5f;
            return Util.Swizzle3(origS);
        }

        public Vector3 Size()
        {
            Vector3 sizeS = Util.Swizzle3(bBoxMax - bBoxMin);
            return new Vector3(
                Mathf.Abs(sizeS.x),
                Mathf.Abs(sizeS.y),
                Mathf.Abs(sizeS.z)
                );
        }

        public string ToString(string value = null)
        {
            if (value != null)
                return "Model_" + value;
            else
                return $"Model_{faceIndex}_{faceCount}";
        }
    }
}

