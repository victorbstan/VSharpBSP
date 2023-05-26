namespace VSharpBSP
{
    public class LightvolLump
    {
        // http://www.mralligator.com/q3/#Lightvols
        // The lightvols lump stores a uniform grid of lighting information used to illuminate non-map objects.
        // There are a total of length / sizeof(lightvol) records in the lump, where length is the size of the lump itself,
        // as specified in the lump directory.
        //
        //     Lightvols make up a 3D grid whose dimensions are:
        //
        // nx = floor(models[0].maxs[0] / 64) - ceil(models[0].mins[0] / 64) + 1
        // ny = floor(models[0].maxs[1] / 64) - ceil(models[0].mins[1] / 64) + 1
        // nz = floor(models[0].maxs[2] / 128) - ceil(models[0].mins[2] / 128) + 1 
        public BSPLightvol[] lightvols { get; set; }
        
        public LightvolLump(int lightvolsCount)
        {
            lightvols = new BSPLightvol[lightvolsCount];
        }

        public override string ToString()
        {
            return "LightvolLump: " + lightvols;
        }
    }
}