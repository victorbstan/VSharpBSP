namespace VSharpBSP
{
    public class LeafFaceLump
    {
        // http://www.mralligator.com/q3/#Leaffaces
        // The leaffaces lump stores lists of face indices, with one list per leaf.
        // There are a total of length / sizeof(leafface) records in the lump, where length is the size of the lump itself,
        // as specified in the lump directory. 
        
        public BSPLeafFace[] LeafFaces { get; set; }

        public LeafFaceLump(int leafFaceCount)
        {
            LeafFaces = new BSPLeafFace[leafFaceCount];
        }
        
        public string PrintInfo()
        {
            return $"LeafFaceLump: {LeafFaces.Length}";
        }
    }   
    
}