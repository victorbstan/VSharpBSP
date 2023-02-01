using System.IO;

namespace VSharpBSP
{
    public class BSPHeader
    {
		public BSPDirectoryEntry[] Directory {
			get;
			set;
		}

		public string Magic {
			get;
			private set;
		}

		public uint Version {
			get;
			private set;
		}

        private BinaryReader BSP;

		private const int LumpCount = 17;

        public BSPHeader(BinaryReader BSP)
        {
            this.BSP = BSP;

            ReadMagic();
            ReadVersion();
            ReadLumps();
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== BSP Header =====\r\n";
            blob += ("Magic Number: " + Magic + "\r\n");
            blob += ("BSP Version: " + Version + "\r\n");
            blob += ("Header Directory:\r\n");
            int count = 0;
            foreach (BSPDirectoryEntry entry in Directory)
            {
                blob += ("Lump " + count + ": " + entry.Name + " Offset: " + entry.Offset + " Length: " + entry.Length + "\r\n");
                count++;
            }
            return blob;
        }

        private void ReadLumps()
        {
            Directory = new BSPDirectoryEntry[LumpCount];
            for (int i = 0; i < 17; i++)
            {
                Directory[i] = new BSPDirectoryEntry(BSP.ReadInt32(), BSP.ReadInt32());
            }

            Directory[0].Name = "Entities";         // 0   Entities Game-related object descriptions.
            Directory[1].Name = "Textures";         // 1   Textures Surface descriptions.
            Directory[2].Name = "Planes";           // 2   Planes Planes used by map geometry.
            Directory[3].Name = "Nodes";            // 3   Nodes BSP tree nodes.
            Directory[4].Name = "Leafs";            // 4   Leafs BSP tree leaves.
            Directory[5].Name = "Leaf faces";       // 5   Leaffaces Lists of face indices, one list per leaf.
            Directory[6].Name = "Leaf brushes";     // 6   Leafbrushes Lists of brush indices, one list per leaf.
            Directory[7].Name = "Models";           // 7   Models Descriptions of rigid world geometry in map.
            Directory[8].Name = "Brushes";          // 8   Brushes Convex polyhedra used to describe solid space.
            Directory[9].Name = "Brush sides";      // 9   Brushsides Brush surfaces.
            Directory[10].Name = "Vertexes";        // 10  Vertexes Vertices used to describe faces.
            Directory[11].Name = "Mesh vertexes";   // 11  Meshverts Lists of offsets, one list per mesh.
            Directory[12].Name = "Effects";         // 12  Effects List of special map effects.
            Directory[13].Name = "Faces";           // 13  Faces Surface geometry.
            Directory[14].Name = "Lightmaps";       // 14  Lightmaps Packed lightmap data.
            Directory[15].Name = "Light volumes";   // 15  Lightvols Local illumination data.
            Directory[16].Name = "Vis data";        // 16  Visdata Cluster-cluster visibility data. 
        }

        private void ReadMagic()
        {
            BSP.BaseStream.Seek(0, SeekOrigin.Begin);
            Magic = new string(BSP.ReadChars(4));
        }


        private void ReadVersion()
        {
            BSP.BaseStream.Seek(4, SeekOrigin.Begin);
            Version = BSP.ReadUInt32();
        }
    }
}
