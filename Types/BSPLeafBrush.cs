namespace VSharpBSP
{
    public class BSPLeafBrush
    {
        // http://www.mralligator.com/q3/#Leafbrushes
        // int brush 	Brush index. 
        public int brush { get; set; }
        
        public BSPLeafBrush(int brush)
        {
            this.brush = brush;
        }
        
        public override string ToString()
        {
            return "BSPLeafBrush: " + brush;
        }
    }
}