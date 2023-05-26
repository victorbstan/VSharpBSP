namespace VSharpBSP
{
    public class VisdataLump
    {
        // http://www.mralligator.com/q3/#Visdata
        // The visdata lump stores bit vectors that provide cluster-to-cluster visibility information.
        // There is exactly one visdata record, with a length equal to that specified in the lump directory. 
        
        public BSPVisdata Visdata { get; set; }

        public VisdataLump(BSPVisdata visdata)
        {
            Visdata = visdata;
        }

        public override string ToString()
        {
            return "VisdataLump: " + Visdata.ToString();
        }
    }
}