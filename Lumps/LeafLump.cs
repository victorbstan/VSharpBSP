namespace VSharpBSP
{
    public class LeafLump
    {
        // http://www.mralligator.com/q3/#Leafs
        // The leafs lump stores the leaves of the map's BSP tree.
        // Each leaf is a convex region that contains, among other things,
        // a cluster index (for determining the other leafs potentially visible from within the leaf),
        // a list of faces (for rendering),
        // and a list of brushes (for collision detection).
        // There are a total of length / sizeof(leaf) records in the lump, where length is the size of the lump itself,
        // as specified in the lump directory. 
        public BSPLeaf[] Leaves { get; set; }
        
        public LeafLump(int leafCount)
        {
            Leaves = new BSPLeaf[leafCount];
        }

        public override string ToString()
        {
            return $"LeafLump: {Leaves.Length}";
        }
    }
}