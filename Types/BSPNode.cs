namespace VSharpBSP
{
    public class BSPNode
    {
        // http://www.mralligator.com/q3/#Nodes
        // int      plane 	    Plane index.
        // int[2]   children 	Children indices. Negative numbers are leaf indices: -(leaf+1).
        // int[3]   mins 	    Integer bounding box min coord.
        // int[3]   maxs 	    Integer bounding box max coord. 
        
        public int plane;
        public int[] children;
        public int[] mins;
        public int[] maxs;
        
        public BSPNode(int plane, int[] children, int[] mins, int[] maxs)
        {
            this.plane = plane;
            this.children = children;
            this.mins = mins;
            this.maxs = maxs;
        }
        
        public override string ToString()
        {
            return "BSPNode: " + plane + "," + children + "," + mins + "," + maxs;
        }
    }
}