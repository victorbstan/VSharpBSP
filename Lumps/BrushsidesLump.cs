
using System.Text;

namespace VSharpBSP
{
    public class BrushsideLump
    {
        public BSPBrushside[] Brushsides { get; set; }

        public BrushsideLump(int brushsideCount)
        {
            Brushsides = new BSPBrushside[brushsideCount];
        }

        public override string ToString()
        {
            StringBuilder blob = new StringBuilder();
            int count = 0;
            foreach (BSPBrushside item in Brushsides)
            {
                blob.Append("Brushside_" + count.ToString());
                count++;
            }

            return blob.ToString();
        }
    }
}
