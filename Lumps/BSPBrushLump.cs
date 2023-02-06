
using System.Text;

namespace VSharpBSP
{
    public class BSPBrushLump
    {
        public BSPBrush[] Brushes { get; set; }

        public BSPBrushLump(int brushCount)
        {
            Brushes = new BSPBrush[brushCount];
        }

        public void PrintInfo()
        {
            //Debug.Log("Models:\r\n");
            //foreach (BSPModel Model in Models)
            //{
            //    Debug.Log("Model - Leafs: " + Model.leafsCount.ToString() + " Nodes: " + Model.nodes[0] + ", " + Model.nodes[1] + ", " + Model.nodes[2] + ", " + Model.nodes[3]);
            //}
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
