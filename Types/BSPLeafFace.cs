namespace VSharpBSP
{
    public class BSPLeafFace
    {
        // http://www.mralligator.com/q3/#Leaffaces
        // int face 	Face index. 
        public int face { get; set; }
        
        public BSPLeafFace(int face)
        {
            this.face = face;
        }
        
        public override string ToString()
        {
            return "BSPLeafFace: " + face;
        }
    }
}