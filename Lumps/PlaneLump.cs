using System.Text;

namespace VSharpBSP
{
    // http://www.mralligator.com/q3/#Planes
    // The planes lump stores a generic set of planes that are in turn referenced by nodes and brushsides.
    // There are a total of length / sizeof(plane) records in the lump, where length is the size of the lump itself,
    // as specified in the lump directory.
    public class PlaneLump
    {
        public BSPPlane[] Planes { get; set; }

        public PlaneLump(int planesCount)
        {
            Planes = new BSPPlane[planesCount];
        }

        public override string ToString()
        {
            StringBuilder blob = new StringBuilder();
            int count = 0;
            foreach (BSPPlane plane in Planes)
            {
                blob.Append("Plane_" + count.ToString());
                count++;
            }

            return blob.ToString();
        }
    }
}