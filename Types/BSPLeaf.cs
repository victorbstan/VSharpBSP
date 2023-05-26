using UnityEngine;

namespace VSharpBSP
{
    public class BSPLeaf
    {
        // http://www.mralligator.com/q3/#Leafs
        // int      cluster 	    Visdata cluster index.
        // int      area 	        Areaportal area.
        // int[3]   mins 	        Integer bounding box min coord.
        // int[3]   maxs 	        Integer bounding box max coord.
        // int      leafface 	    First leafface for leaf.
        // int      n_leaffaces 	Number of leaffaces for leaf.
        // int      leafbrush 	    First leafbrush for leaf.
        // int      n_leafbrushes 	Number of leafbrushes for leaf. 
        public int cluster;
        public int area;
        public Vector3 mins;
        public Vector3 maxs;
        public int leafface;
        public int n_leaffaces;
        public int leafbrush;
        public int n_leafbrushes;
        
        public BSPLeaf(
            int cluster,
            int area,
            Vector3 mins,
            Vector3 maxs,
            int leafface,
            int n_leaffaces,
            int leafbrush,
            int n_leafbrushes)
        {
            this.cluster = cluster;
            this.area = area;
            this.mins = mins;
            this.maxs = maxs;
            this.leafface = leafface;
            this.n_leaffaces = n_leaffaces;
            this.leafbrush = leafbrush;
            this.n_leafbrushes = n_leafbrushes;
        }
        
        public override string ToString()
        {
            return $"BSPLeaf: {cluster} {area} {mins} {maxs} {leafface} {n_leaffaces} {leafbrush} {n_leafbrushes}";
        }
    }
}