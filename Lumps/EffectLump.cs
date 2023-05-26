namespace VSharpBSP
{
    public class EffectLump
    {
        // Effects 
        // The effects lump stores references to volumetric shaders (typically fog) which affect the rendering of a particular group of faces.
        // There are a total of length / sizeof(effect) records in the lump, where length is the size of the lump itself,
        // as specified in the lump directory.

        public BSPEffect[] Effects { get; set; }
        
        public EffectLump(int effectCount)
        {
            Effects = new BSPEffect[effectCount];
        }

        public override string ToString()
        {
            return $"EffectLump: {Effects.Length}";
        }
    }
}