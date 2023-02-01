
namespace VSharpBSP
{
    public class BSPModelLump
    {
        public BSPModel[] Models { get; set; }

        public BSPModelLump(int modelCount) {
            Models = new BSPModel[modelCount];
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
