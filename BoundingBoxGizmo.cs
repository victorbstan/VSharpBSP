using UnityEngine;
using VSharpBSP.Entities;

namespace VSharpBSP
{
    public class BoundingBoxGizmo : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            if (GetComponentInParent<Entity>() == null) return;
            switch (GetComponentInParent<Entity>().classname)
            {
                case "func_door":
                    Gizmos.color = new Color(1, 0, 0, 0.5f);
                    break;
                case "trigger_push":
                    Gizmos.color = new Color(1, 1, 0, 0.5f);
                    break;
                default:
                    Gizmos.color = new Color(1, 1, 1, 0.1f);
                    break;
            }
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, GetComponent<BoundingBox>().size);
        }
    }
}
