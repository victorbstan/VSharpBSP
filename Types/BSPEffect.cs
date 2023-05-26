namespace VSharpBSP
{
    public class BSPEffect
    {
        // http://www.mralligator.com/q3/#Effects
        // string[64]   name 	    Effect shader.
        // int          brush 	    Brush that generated this effect.
        // int          unknown 	Always 5, except in q3dm8, which has one effect with -1. 
        public string name;
        public int brush;
        public int unknown;
        
        public BSPEffect(string name, int brush, int unknown)
        {
            this.name = name;
            this.brush = brush;
            this.unknown = unknown;
        }
        
        public override string ToString()
        {
            return "BSPEffect: " + name + ", " + brush + ", " + unknown;
        }
    }
}