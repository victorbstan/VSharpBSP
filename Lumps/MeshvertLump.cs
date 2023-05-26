namespace VSharpBSP
{
    public class MeshvertLump
    {
        // http://www.mralligator.com/q3/#Meshverts
        // The meshverts lump stores lists of vertex offsets, used to describe generalized triangle meshes.
        // There are a total of length / sizeof(meshvert) records in the lump, where length is the size of the lump itself,
        // as specified in the lump directory. 
        public Meshvert[] Meshverts { get; set; }
        
        public MeshvertLump(int meshvertCount)
        {
            Meshverts = new Meshvert[meshvertCount];
        }

        public override string ToString()
        {
            return "MeshvertLump: " + Meshverts.Length;
        }
    }
}