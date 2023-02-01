
namespace VSharpBSP
{
    public static class Constants
    {
        // Some config settings referenced from here: https://github.com/radiatoryang/scopa/blob/49fbf8519fe480d02327c71b843ab483081d0ff5/Runtime/ScopaMapConfigAsset.cs
        // (default: 0.03125, 1 m = 32 units) The global scaling factor for all brush geometry and entity origins.
        public const double d_scaleMultiple = 0.03125;
        public const float f_scaleMultiple = 0.032F;
        public const float doorTriggerPadding = 2f;
        public const string entScriptDir = "Assets/Scripts/VSharpBSP/Entities";
        public const string mapAssetsPath = "Assets/baseq3/maps/";
        public const string modelsDirName = "Models";
        public const string modelsPath = "Map/" + modelsDirName;
        public const string bspMaterialName = "Legacy Shaders/Lightmapped/Diffuse";
        public const string bspMeshMaterialName = "Legacy Shaders/VertexLit";
        public const string gameAssetsPath = "Assets/baseq3/";
    }
}
