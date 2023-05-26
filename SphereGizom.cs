namespace VSharpBSP
{
    using UnityEngine;

    public class SphereGizmo : MonoBehaviour
    {
        public float radius = 0.25f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}