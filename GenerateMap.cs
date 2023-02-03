using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

namespace VSharpBSP
{
    public class GenerateMap : MonoBehaviour
    {
        // Public
        public bool generateAtRuntime = true;
        public Material replacementTexture;
        public bool useRippedTextures;
        public bool renderBezPatches;
        public string mapName;
        public bool mapIsInsidePK3;
        public bool applyLightmaps;
        public int tessellations = 5;
        public List<string> customNoClipTextures;

        // Private
        private int faceCount = 0;
        private BSPMap map;
        private List<string> _noClipTextures;
        

        void Awake()
        {
            if (generateAtRuntime)
                Run();
        }

        public void Run()
        {
            // TODO: move this somewhere else
            List<string> presetNoClipTextures = new List<string> {
                "models/mapobjects/teleporter/energy", "models/mapobjects/teleporter/widget",
                "models/mapobjects/slamp/", "models/mapobjects/flag/", "models/mapobjects/storch/",
                "models/mapobjects/gratelamp/", "models/mapobjects/spotlamp/", "textures/liquids/", 
                "textures/sfx/portal", "textures/sfx/flame", "textures/sfx/hellfog", "textures/sfx/fog",
                "models/mapobjects/weeds", "textures/sfx/q3dm9fog", "textures/sfx/xflame", "textures/sfx/beam",
                "textures/base_wall/protobanner", "models/mapobjects/spotlamp/beam", "textures/sfx/proto_zzztblu3",
                "models/mapobjects/baph/bapholamp_fx", "textures/sfx/lavabeam"
            };
            // Initialize hardcoded list of no clip texture prefixes
            _noClipTextures = customNoClipTextures.Concat(presetNoClipTextures).ToList();
            Debug.Log("NO CLIP TEXTURES COUNT: " + _noClipTextures.Count);
            
            // Create meshes container game object, parent of all entities in scene
            GameObject meshesContainer = new GameObject(Constants.modelsDirName);
            // Add as child of parent (this) game object
            meshesContainer.transform.parent = gameObject.transform;
            
            // Create brushes container game object, parent of all entities in scene
            GameObject brushesContainer = new GameObject("Brushes");
            // Add as child of parent (this) game object
            brushesContainer.transform.parent = gameObject.transform;

            // Create a new BSPmap, which is an object that
            // represents the map and all its data as a whole
            if (mapIsInsidePK3)
                map = new BSPMap(mapName, true);
            else
                map = new BSPMap(Constants.mapAssetsPath + mapName, false);


            //
            // MODEL MESHES
            //

            int modelIndex = 0;
            foreach (BSPModel model in map.modelLump.Models)
            {
                // Create Model container game object to hold related game objects
                GameObject mContainer = new GameObject(model.ToString($"{modelIndex}"));
                // Add as child of parent meshesContainer game object
                mContainer.transform.parent = meshesContainer.transform;

                // MODEL BOUNDING BOX

                // Create BoundingBox container game object for each mesh
                GameObject bBoxGO = new GameObject("BoundingBox");
                // Add as child of parent meshesContainer game object
                bBoxGO.transform.parent = mContainer.transform;
                // Add Unity Component
                BoundingBox bBoxComp = bBoxGO.AddComponent<BoundingBox>();
                // Set up BoundingBox component params
                bBoxComp.origin = model.Origin();
                bBoxComp.size = model.Size();
                // Move BBox GameObject to origin TODO move to self initialization call
                bBoxGO.transform.position = bBoxComp.origin;
                // Add Gizmo TODO move to self initialization call
                BoundingBoxGizmo bbGizmo = bBoxGO.AddComponent<BoundingBoxGizmo>();

                // MODEL FACES

                // Get faces of a particular model
                Face[] modelFaces = map.faceLump.Faces.Skip(model.faceIndex).Take(model.faceCount).ToArray();

                // Each face is its own gameobject
                // 1=polygon, 2=patch, 3=mesh, 4=billboard
                foreach (Face face in modelFaces)
                {
                    if (face.type == 1) // polygon
                    {
                        GeneratePolygonObject(face, bBoxGO);
                        faceCount++;
                    }
                    else if (face.type == 2) // bez patch
                    {
                        if (renderBezPatches)
                        {
                            GenerateBezObject(face, bBoxGO);
                        }
                        faceCount++;
                    }
                    else if (face.type == 3) // mesh
                    {
                        GeneratePolygonObject(face, bBoxGO);
                        faceCount++;
                    }
                    else if (face.type == 4) // billboard
                    {
                        // Skipping because it was not a polygon, mesh, or bez patch
                        Debug.Log($"Skipped Face type {face.type} (billboard) {faceCount.ToString()}.");
                        faceCount++;
                    }
                    else
                    {
                        // Should not get here
                        // Skipping because it was not a polygon, mesh, or bez patch
                        Debug.Log($"Skipped Face type {face.type} (unknown) {faceCount.ToString()}.");
                        faceCount++;
                    }
                }

                // MODEL BRUSHES
                
                // Model > Brushes > Brushsides > Plane > BrushToMesh()
                // NOTE: This section might not be usable in Unity,
                // I don't know how to build collision shapes from this
                
                // Create Brushes container game object for each mesh
                GameObject bContainer = new GameObject($"Brush_{modelIndex}");
                // Add as child of parent meshesContainer game object
                bContainer.transform.parent = brushesContainer.transform;
                
                // BRUSHES
                
                BSPBrush[] modelBrushes = map.brushLump.Brushes.Skip(model.brushIndex).Take(model.brushCount).ToArray();
                foreach (BSPBrush brush in modelBrushes)
                {
                    Debug.Log(brush);
                }

                modelIndex++;
            }
            GC.Collect();

            //
            // ENTITIES
            //

            // Get all file names in Entities directory
            // This list will be used to match entites and models to their scripts
            string[] files = System.IO.Directory.GetFiles(
                Constants.entScriptDir,
                "*.cs",
                System.IO.SearchOption.TopDirectoryOnly
                ).Select(path => ProcessPath(path)).ToArray();

            // Create entities container game object,
            // parent of all entities in scene
            GameObject e_container = new GameObject("Entities");
            // Add as child of parent (this) game object
            e_container.transform.parent = gameObject.transform;

            // Get a "chunk" of string that should represent an entity definition
            string[] e_chunks = map.entityLump.ToString().Split('{', '}');

            // Parse chunks and their pieces and put them into a more useful list
            List<Dictionary<string, string>> ent_list = new List<Dictionary<string, string>>();
            foreach (string chunk_item in e_chunks)
            {
                string e_chunk = chunk_item.Trim(' ', '\n', '\0');
                if (e_chunk.Length > 0)
                {
                    // Parse entity string

                    // Split the chunks at newline
                    string[] e_lines = e_chunk.Split('\n');
                    Dictionary<string, string> e_dict = new Dictionary<string, string>();
                    foreach (string e_line in e_lines)
                    {
                        // Split key value pairs, match first space (space after closing ")
                        string[] e_key_val = e_line.Split("\" ");
                        if (e_key_val.Length == 2)
                        {
                            // Format and add key value
                            string e_key = e_key_val[0].Trim('"');
                            string e_val = e_key_val[1].Trim('"');
                            e_dict.Add(e_key, e_val);
                        }
                    }
                    // Add to list if we have a classname
                    if (e_dict.ContainsKey("classname"))
                    {
                        ent_list.Add(e_dict);
                    }
                    else
                    {
                        // Should not get here
                        Debug.Log(e_chunk);
                        Debug.Log(e_lines);
                        Debug.Log(e_dict);
                    }
                }
            }
            GC.Collect();

            //
            // ATTACH ENTITIES TO MODELS
            //

            foreach (Dictionary<string, string> e_dict in ent_list)
            {
                // Worldspawn entity
                if (e_dict.ContainsKey("classname") && e_dict["classname"] == "worldspawn")
                {
                    // Find worldspawn model, which should always have index 0
                    GameObject modelGO = FindModel("0");
                    if (modelGO == null) return;
                    // Add entity script
                    string entityComponentName = GetEntityTypeName(files, "worldspawn");
                    Entities.Entity comp = (Entities.Entity)modelGO.AddComponent(Type.GetType(entityComponentName));
                    comp.AddAttributes(e_dict);
                    comp.Init();
                }
                // Model entities
                else if (
                    e_dict.ContainsKey("classname") &&
                    e_dict.ContainsKey("model") &&
                    (
                        e_dict["classname"].StartsWith("func_") ||
                        e_dict["classname"].StartsWith("trigger_")
                    )
                )
                {
                    // Add as child of parent (this) game object
                    string index = e_dict["model"];
                    if (string.IsNullOrEmpty(index)) return;
                    index = index.Substring(1); // remove * from string
                    if (string.IsNullOrEmpty(index)) return;
                    // Find model to attach Game Object to
                    GameObject modelGO = FindModel(index);
                    if (modelGO == null) return;
                    // Add entity script
                    string entityComponentName = GetEntityTypeName(files, e_dict["classname"]);
                    Entities.Entity comp = (Entities.Entity)modelGO.AddComponent(Type.GetType(entityComponentName));
                    comp.AddAttributes(e_dict);
                    comp.Init();
                }
                // Point entities
                else if (e_dict.ContainsKey("classname"))
                {
                    // Instantiate a new GO
                    GameObject entity = new GameObject();
                    entity.name = e_dict["classname"];

                    // Place in world, translate BSP coords to Unity coords
                    if (e_dict.ContainsKey("origin"))
                    {
                        string[] e_loc = e_dict["origin"].Split(' ');
                        Vector3 e_vec = Util.Swizzle3(
                            new Vector3(
                                float.Parse(e_loc[0]),
                                float.Parse(e_loc[1]),
                                float.Parse(e_loc[2])
                                )
                            );
                        entity.transform.position = e_vec;
                    }
                    // Add as child of parent (this) game object
                    entity.transform.parent = e_container.transform;
                    // Add entity script
                    string entityComponentName = GetEntityTypeName(files, entity.name);
                    Entities.Entity comp = (Entities.Entity)entity.AddComponent(Type.GetType(entityComponentName));
                    comp.AddAttributes(e_dict);
                    comp.Init();

                    // Add gizmo script
                    entity.AddComponent<EntityGizmo>();
                }
                else
                {
                    // Should not get here
                    Debug.Log("No class name!");
                    Debug.Log(e_dict);
                }
            }
            GC.Collect();
        }


        //
        // HELPER FUNCTIONS
        //

        private static string GetEntityTypeName(string[] files, string classname)
        {
            string entityComponentName = "";

            // Script
            switch (classname)
            {
                // Hardcode matches
                case string cname when cname.StartsWith("ammo"):
                    entityComponentName = "VSharpBSP.Entities.Item";
                    break;
                case string cname when cname.StartsWith("light") || cname.StartsWith("ambient"):
                    entityComponentName = "VSharpBSP.Entities.Light";
                    break;
                // "SmartMatch(TM)"
                default:
                    // Loop over found filenames in Entities directory
                    if (files.Length > 0)
                    {
                        // Get matching filename
                        for (int i = 0; i < files.Length; i++)
                        {
                            string fileName = files[i];

                            if (classname.StartsWith(fileName.ToLower()))
                            {
                                Type classType = Type.GetType($"VSharpBSP.Entities.{fileName}");
                                if (classType != null)
                                    entityComponentName = $"VSharpBSP.Entities.{fileName}";
                                break;
                            }
                            // If at end of list of files, add Misc component
                            else if (i == (files.Length - 1))
                            {
                                entityComponentName = "VSharpBSP.Entities.Misc";
                                break;
                            }
                        }
                    }
                    break;
            }

            return entityComponentName;
        }

        private GameObject FindModel(string index)
        {
            return GameObject.Find(Constants.modelsPath + "/Model_" + index);
        }

        // Helper class to return only the name of files without extension from a provided path
        private string ProcessPath(string path)
        {
            string fileName = Path.GetFileName(path);
            fileName = fileName.Split('.').First();
            return fileName;
        }

        #region Object Generation
        // This makes gameobjects for every bez patch in a face
        // they are tessellated according to the "tessellations" field
        // in the editor
        void GenerateBezObject(Face face, GameObject mContainer, bool isStatic = true)
        {
            int numPatches = ((face.size[0] - 1) / 2) * ((face.size[1] - 1) / 2);

            for (int i = 0; i < numPatches; i++)
            {
                GameObject bezObject = new GameObject();
                bezObject.isStatic = isStatic;
                bezObject.transform.parent = mContainer.transform;
                bezObject.name = "BSPface (bez) " + faceCount.ToString();
                bezObject.AddComponent<MeshFilter>().mesh = GenerateBezMesh(face, i);
                bezObject.AddComponent<MeshRenderer>();
                if (Util.StrStartsWithAny(FetchTextureName(face), _noClipTextures) == false)
                    bezObject.AddComponent<MeshCollider>();
                if (useRippedTextures)
                    bezObject.GetComponent<Renderer>().material = FetchMaterial(face);
                else
                    bezObject.GetComponent<Renderer>().material = replacementTexture;
            }
        }


        // This takes one face and generates a gameobject complete with
        // mesh, renderer, material with texture, and collider.
        void GeneratePolygonObject(Face face, GameObject mContainer, bool isStatic = true)
        {
            GameObject faceObject = new GameObject("BSPface " + faceCount.ToString());
            faceObject.isStatic = isStatic;
            faceObject.transform.parent = mContainer.transform;
            // Our GeneratePolygonMesh will optimze and add the UVs for us
            faceObject.AddComponent<MeshFilter>().mesh = GeneratePolygonMesh(face);
            faceObject.AddComponent<MeshRenderer>();
            Debug.Log("TEX NAME: " + FetchTextureName(face));
            if (Util.StrStartsWithAny(FetchTextureName(face), _noClipTextures))
                Debug.Log("MATCH: " + FetchTextureName(face));
            if (Util.StrStartsWithAny(FetchTextureName(face), _noClipTextures) == false)
                faceObject.AddComponent<MeshCollider>();
            if (useRippedTextures)
                faceObject.GetComponent<Renderer>().material = FetchMaterial(face);
            else
                faceObject.GetComponent<Renderer>().material = replacementTexture;
        }

        #endregion

        #region Mesh Generation
        // This forms a mesh from a bez patch of your choice
        // from the face of your choice.
        // It's ready to render with tex coords and all.
        Mesh GenerateBezMesh(Face face, int patchNumber)
        {
            //Calculate how many patches there are using size[]
            //There are n_patchesX by n_patchesY patches in the grid, each of those
            //starts at a vert (i,j) in the overall grid
            //We don't actually need to know how many are on the Y length
            //but the forumla is here for historical/academic purposes
            int n_patchesX = ((face.size[0]) - 1) / 2;
            //int n_patchesY = ((face.size[1]) - 1) / 2;


            //Calculate what [n,m] patch we want by using an index
            //called patchNumber  Think of patchNumber as if you 
            //numbered the patches left to right, top to bottom on
            //the grid in a piece of paper.
            int pxStep = 0;
            int pyStep = 0;
            for (int i = 0; i < patchNumber; i++)
            {
                pxStep++;
                if (pxStep == n_patchesX)
                {
                    pxStep = 0;
                    pyStep++;
                }
            }

            //Create an array the size of the grid, which is given by
            //size[] on the face object.
            Vertex[,] vertGrid = new Vertex[face.size[0], face.size[1]];

            //Read the verts for this face into the grid, making sure
            //that the final shape of the grid matches the size[] of
            //the face.
            int gridXstep = 0;
            int gridYstep = 0;
            int vertStep = face.vertex;
            for (int i = 0; i < face.n_vertexes; i++)
            {
                vertGrid[gridXstep, gridYstep] = map.vertexLump.Verts[vertStep];
                vertStep++;
                gridXstep++;
                if (gridXstep == face.size[0])
                {
                    gridXstep = 0;
                    gridYstep++;
                }
            }

            //We now need to pluck out exactly nine vertexes to pass to our
            //teselate function, so lets calculate the starting vertex of the
            //3x3 grid of nine vertexes that will make up our patch.
            //we already know how many patches are in the grid, which we have
            //as n and m.  There are n by m patches.  Since this method will
            //create one gameobject at a time, we only need to be able to grab
            //one.  The starting vertex will be called vi,vj think of vi,vj as x,y
            //coords into the grid.
            int vi = 2 * pxStep;
            int vj = 2 * pyStep;
            //Now that we have those, we need to get the vert at [vi,vj] and then
            //the two verts at [vi+1,vj] and [vi+2,vj], and then [vi,vj+1], etc.
            //the ending vert will at [vi+2,vj+2]

            List<Vector3> bverts = new List<Vector3>();

            //read texture/lightmap coords while we're at it
            //they will be tessellated as well.
            List<Vector2> uvs = new List<Vector2>();
            List<Vector2> uv2s = new List<Vector2>();

            //Top row
            bverts.Add(vertGrid[vi, vj].position);
            bverts.Add(vertGrid[vi + 1, vj].position);
            bverts.Add(vertGrid[vi + 2, vj].position);

            uvs.Add(vertGrid[vi, vj].texcoord);
            uvs.Add(vertGrid[vi + 1, vj].texcoord);
            uvs.Add(vertGrid[vi + 2, vj].texcoord);

            uv2s.Add(vertGrid[vi, vj].lmcoord);
            uv2s.Add(vertGrid[vi + 1, vj].lmcoord);
            uv2s.Add(vertGrid[vi + 2, vj].lmcoord);

            //Middle row
            bverts.Add(vertGrid[vi, vj + 1].position);
            bverts.Add(vertGrid[vi + 1, vj + 1].position);
            bverts.Add(vertGrid[vi + 2, vj + 1].position);

            uvs.Add(vertGrid[vi, vj + 1].texcoord);
            uvs.Add(vertGrid[vi + 1, vj + 1].texcoord);
            uvs.Add(vertGrid[vi + 2, vj + 1].texcoord);

            uv2s.Add(vertGrid[vi, vj + 1].lmcoord);
            uv2s.Add(vertGrid[vi + 1, vj + 1].lmcoord);
            uv2s.Add(vertGrid[vi + 2, vj + 1].lmcoord);

            //Bottom row
            bverts.Add(vertGrid[vi, vj + 2].position);
            bverts.Add(vertGrid[vi + 1, vj + 2].position);
            bverts.Add(vertGrid[vi + 2, vj + 2].position);

            uvs.Add(vertGrid[vi, vj + 2].texcoord);
            uvs.Add(vertGrid[vi + 1, vj + 2].texcoord);
            uvs.Add(vertGrid[vi + 2, vj + 2].texcoord);

            uv2s.Add(vertGrid[vi, vj + 2].lmcoord);
            uv2s.Add(vertGrid[vi + 1, vj + 2].lmcoord);
            uv2s.Add(vertGrid[vi + 2, vj + 2].lmcoord);

            //Now that we have our control grid, it's business as usual
            Mesh bezMesh = new Mesh();
            bezMesh.name = "BSPfacemesh (bez)";
            BezierMesh bezPatch = new BezierMesh(tessellations, bverts, uvs, uv2s);
            return bezPatch.Mesh;
        }

        // Generate a mesh for a simple polygon/mesh face
        // It's ready to render with tex coords and all.
        Mesh GeneratePolygonMesh(Face face)
        {
            Mesh worldFace = new Mesh();
            worldFace.name = "BSPface (poly/mesh)";

            // Rip verts, uvs, and normals
            // I have ripping normals commented because it looks
            // like it's better to just let Unity recalculate them for us.
            List<Vector3> verts = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector2> uv2s = new List<Vector2>();
            int vstep = face.vertex;
            for (int i = 0; i < face.n_vertexes; i++)
            {
                verts.Add(map.vertexLump.Verts[vstep].position);
                uvs.Add(map.vertexLump.Verts[vstep].texcoord);
                uv2s.Add(map.vertexLump.Verts[vstep].lmcoord);
                vstep++;
            }

            // add the verts, uvs, and normals we ripped to the gameobjects mesh filter
            worldFace.vertices = verts.ToArray();

            // Add the texture co-ords (or UVs) to the face/mesh
            worldFace.uv = uvs.ToArray();
            worldFace.uv2 = uv2s.ToArray();

            // Rip meshverts / triangles
            List<int> mverts = new List<int>();
            int mstep = face.meshvert;
            for (int i = 0; i < face.n_meshverts; i++)
            {
                mverts.Add(map.vertexLump.MeshVerts[mstep]);
                mstep++;
            }

            // add the meshverts to the object being built
            worldFace.triangles = mverts.ToArray();

            // Let Unity do some heavy lifting for us
            worldFace.RecalculateBounds();
            worldFace.RecalculateNormals();
            worldFace.Optimize();

            return worldFace;
        }
        #endregion

        #region Material Generation
        // This returns a material with the correct texture for a given face
        Material FetchMaterial(Face face)
        {
            string texName = map.textureLump.Textures[face.texture].Name;

            // Load the primary texture for the face from the texture lump
            // The texture lump itself will have already looked over all
            // available .pk3 files and compiled a dictionary of textures for us.
            Texture2D tex;

            if (map.textureLump.ContainsTexture(texName))
            {
                tex = map.textureLump.GetTexture(texName);
            }
            else
            {
                Debug.Log("Texture not found! " + texName);

                return replacementTexture;
            }

            // Lightmapping is on, so calc the lightmaps
            if (face.lm_index >= 0 && applyLightmaps)
            {
                // Pick a shader that supports lightmaps
                Material bspMaterial = new Material(Shader.Find(Constants.bspMaterialName));

                // LM experiment
                Texture2D lmap = map.lightmapLump.Lightmaps[face.lm_index];
                lmap.Compress(true);
                lmap.Apply();

                // Put the textures in the shader.
                bspMaterial.mainTexture = tex;
                bspMaterial.SetTexture("_LightMap", lmap);

                return bspMaterial;
            }
            else
            {
                // Lightmapping is off, so don't.
                // This is likely a mesh object and not part of the bsp
                Material bspMaterial = new Material(Shader.Find(Constants.bspMeshMaterialName));
                bspMaterial.mainTexture = tex;
                return bspMaterial;
            }

        }
        #endregion

        public string FetchTextureName(Face face)
        {
            return map.textureLump.Textures[face.texture].Name;
        }
    }
}

