using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VSharpBSP;

namespace Editor
{
    // http://answers.unity3d.com/questions/126048/create-a-button-in-the-inspector.html#answer-360940
    [CustomEditor(typeof(GenerateMap))]
    class GenerateMapCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GenerateMap script = (GenerateMap)target;
            if (GUILayout.Button("Generate Map"))
            {
                Debug.Log("Generate Map: " + script.mapName);
                script.Run();
            }
            if (GUILayout.Button("Clear Map"))
            {
                // http://forum.unity3d.com/threads/deleting-all-chidlren-of-an-object.92827/
                var children = new List<GameObject>();
                foreach (Transform child in script.gameObject.transform) children.Add(child.gameObject);
                children.ForEach(child => DestroyImmediate(child));
            }
        }
    }
}
