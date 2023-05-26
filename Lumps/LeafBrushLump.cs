namespace VSharpBSP
{
    public class LeafBrushLump
    {
        // http://www.mralligator.com/q3/#Leafbrushes
        // The leafbrushes lump stores lists of brush indices,
        // with one list per leaf. There are a total of length / sizeof(leafbrush) records in the lump,
        // where length is the size of the lump itself, as specified in the lump directory. 
        
        public BSPLeafBrush[] LeafBrushes { get; set; }

        public LeafBrushLump(int leafBrushCount)
        {
            LeafBrushes = new BSPLeafBrush[leafBrushCount];
        }
        
        public string PrintInfo()
        {
            return $"LeafBrushLump: {LeafBrushes.Length}";
        }
    }
}