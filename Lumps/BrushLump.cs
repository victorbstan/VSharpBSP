
using System.Text;

namespace VSharpBSP
{
    public class BrushLump
    {
        public BSPBrush[] Brushes { get; set; }

        public BrushLump(int brushCount)
        {
            Brushes = new BSPBrush[brushCount];
        }

        public override string ToString()
        {
            StringBuilder blob = new StringBuilder();
            int count = 0;
            foreach (BSPBrush item in Brushes)
            {
                blob.Append("Brush_" + count.ToString());
                count++;
            }

            return blob.ToString();
        }
    }
}
