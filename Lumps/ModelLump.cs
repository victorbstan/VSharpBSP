
namespace VSharpBSP
{
    public class ModelLump
    {
        public BSPModel[] Models { get; set; }

        public ModelLump(int modelCount) {
            Models = new BSPModel[modelCount];
        }
        
        public override string ToString()
        {
            return "ModelLump: " + Models.Length;
        }
    }
}
