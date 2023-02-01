
namespace VSharpBSP
{
    public class BSPBrush
    {
        // http://www.mralligator.com/q3/#Brushes
        // int brushside       First brushside for brush.
        // int n_brushsides    Number of brushsides for brush.
        // int texture         Texture index.

        public int brushsideIndex;
        public int brushdidesCount;
        public int textureIndex;

        public BSPBrush(int brushsideIndex, int brushdidesCount, int textureIndex)
        {
            this.brushsideIndex = brushsideIndex;
            this.brushdidesCount = brushdidesCount;
            this.textureIndex = textureIndex;
        }
    }
}