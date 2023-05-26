namespace VSharpBSP
{
    public class BSPLightvol
    {
        // http://www.mralligator.com/q3/#Lightvols
        // ubyte[3] ambient 	Ambient color component. RGB.
        // ubyte[3] directional 	Directional color component. RGB.
        // ubyte[2] dir 	Direction to light. 0=phi, 1=theta. 
        
        public byte[] ambient;
        public byte[] directional;
        public byte[] dir;
        
        public BSPLightvol(byte[] ambient, byte[] directional, byte[] dir)
        {
            this.ambient = ambient;
            this.directional = directional;
            this.dir = dir;
        }
        
        public override string ToString()
        {
            return "BSPLightvol: " + ambient + " " + directional + " " + dir;
        }
    }
}