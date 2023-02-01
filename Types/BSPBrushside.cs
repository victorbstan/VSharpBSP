
namespace VSharpBSP
{
    public class BSPBrushside
    {
        // http://www.mralligator.com/q3/#Brushsides
        // int plane     Plane index.
        // int texture   Texture index.

        public int planeIndex;
        public int textureIndex;

        public BSPBrushside(int planeIndex, int textureIndex)
        {
            this.planeIndex = planeIndex;
            this.textureIndex = textureIndex;
        }
    }
}