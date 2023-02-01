
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
    }
}
