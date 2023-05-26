namespace VSharpBSP
{
    public class BSPVisdata
    {
        // int                      n_vecs 	    Number of vectors.
        // int                      sz_vecs 	Size of each vector, in bytes.
        // ubyte[n_vecs * sz_vecs]  vecs 	    Visibility data. One bit per cluster per vector.
        
        // Cluster x is visible from cluster y if the (1 << y % 8) bit of vecs[x * sz_vecs + y / 8] is set.
        // Note that clusters are associated with leaves. 
        
        // The number of vectors is equal to the number of clusters divided by 8 rounded up to the next integer. 
        // The vector length is equal to the number of vectors divided by 8 rounded up to the next integer.
        
        public int n_vecs;
        public int sz_vecs;
        public byte[] vecs;
        
        public BSPVisdata(int n_vecs, int sz_vecs, byte[] vecs)
        {
            this.n_vecs = n_vecs;
            this.sz_vecs = sz_vecs;
            this.vecs = vecs;
        }
        
        public override string ToString()
        {
            return "BSPVisdata: " + n_vecs + ", " + sz_vecs + ", " + vecs.Length;
        }
    }
}