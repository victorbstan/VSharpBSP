
using System.Text;

namespace VSharpBSP
{
    public class BSPBrushsideLump
    {
        public BSPBrushside[] Brushsides { get; set; }

        public BSPBrushsideLump(int brushsideCount)
        {
            Brushsides = new BSPBrushside[brushsideCount];
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
            foreach (BSPBrushside item in Brushsides)
            {
                blob.Append("Brushside_" + count.ToString());
                count++;
            }

            return blob.ToString();
        }
    }
}
