namespace VSharpBSP
{
    public class NodeLump
    {
        // http://www.mralligator.com/q3/#Nodes
        // The nodes lump stores all of the nodes in the map's BSP tree.
        // The BSP tree is used primarily as a spatial subdivision scheme, dividing the world into convex regions called leafs.
        // The first node in the lump is the tree's root node.
        // There are a total of length / sizeof(node) records in the lump, where length is the size of the lump itself,
        // as specified in the lump directory. 
        
        public BSPNode[] Nodes { get; set; }
        
        public NodeLump(int nodeCount)
        {
            Nodes = new BSPNode[nodeCount];
        }
        
        public override string ToString()
        {
            return $"NodeLump: {Nodes.Length}";
        }
    }
}