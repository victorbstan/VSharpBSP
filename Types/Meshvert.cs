namespace VSharpBSP
{
    public class Meshvert
    {
        // http://www.mralligator.com/q3/#Meshverts
        // int  offset 	   Vertex index offset, relative to first vertex of corresponding face. 

        public int offset;
        
        public Meshvert(int offset)
        {
            this.offset = offset;
        }
        
        public override string ToString()
        {
            return "Meshvert: " + offset;
        }
    }
}