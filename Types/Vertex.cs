using UnityEngine;

namespace VSharpBSP
{
    public class Vertex
    {
        public Vector3 position;
        public Vector3 normal;
        public byte[] color;

        // These are texture coords, or UVs
        public Vector2 texcoord = new Vector2();
        public Vector2 lmcoord = new Vector2();

        public Vertex(
            Vector3 position,
            float texX,
            float texY,
            float lmX,
            float lmY,
            Vector3 normal,
            byte[] color)
        {
            this.position = position;
            this.normal = normal;

            // Color data doesn't get used
            this.color = color;

            // Invert the texture coords, to account for
            // the difference in the way Unity and Quake3
            // handle them.
            texcoord.x = texX;
            texcoord.y = -texY;

            // Lightmaps aren't used for now, but store the
            // data for them anyway.  Inverted, same as above.
            lmcoord.x = lmX;
            lmcoord.y = lmY;

            // Do that swizzlin'.
            // Dunno why it's called swizzle.  An article I was reading called it that.
            // Swizzle, swizzle, swizzle.
            Swizzle();
        }

        // This converts the verts from the format Q3 uses to the one Unity3D uses.
        // Look up the Q3 map/rendering specs if you want the details.
        // Quake3 also uses an odd scale where ~0.032 units is about 1 meter, so scale it way down
        // while you're at it.
        private void Swizzle()
        {
            position = Util.Swizzle3(position);
            normal = Util.Swizzle3(normal);
        }
    }
}
