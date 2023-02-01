using UnityEngine;
using VSharpBSP.Entities;

namespace VSharpBSP
{
    public class EntityGizmo : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            if (GetComponent<Entity>() == null) return;
            
            switch (GetComponent<Entity>().classname)
            {
                case "light":
                    Gizmos.color = Color.yellow;
                    break;
                case string cname when cname.StartsWith("target_"):
                    Gizmos.color = Color.green;
                    break;
                case string cname when cname.StartsWith("item_"):
                    Gizmos.color = Color.magenta;
                    break;
                case string cname when cname.StartsWith("ammo_"):
                    Gizmos.color = new Color(1, 0.5f, 0, 1);
                    break;
                case string cname when cname.StartsWith("weapon_"):
                    Gizmos.color = Color.red;
                    break;
                default:
                    Gizmos.color = Color.white;
                    break;
            }
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}
