using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace VSharpBSP.Entities
{

    public class Trigger : Entity
    {
        private Vector3 _targetPos;

        public override void Init()
        {
            base.Init();

            Transform bBoxTransform = transform.Find("BoundingBox");

            if (bBoxTransform == null)
                return;

            GameObject bBoxGO = transform.Find("BoundingBox").gameObject;
            
            if (!bBoxGO.TryGetComponent<BoundingBox>(out var bBoxComp))
                return;

            if (bBoxGO.GetComponent<BoxCollider>())
                return;

            // Add trigger component to bounding box
            BoxCollider bBoxCol = bBoxGO.AddComponent<BoxCollider>();
            bBoxCol.isTrigger = true;
            bBoxCol.size = bBoxComp.size;

            // Add Rigidbody component to bounding boxes
            // Rigidbody bBoxRb = bBoxGO.AddComponent<Rigidbody>();
            // bBoxRb.useGravity = false;
            // bBoxRb.freezeRotation = true;
            // bBoxRb.isKinematic = true;

            // Add collision manager to bounding box trigger collider
            bBoxGO.AddComponent<EntityTriggerManager>();
        }

        //
        // ENTITY EVENTS
        //

        public override void EntityTriggerEnter(Collider other)
        {
            base.EntityTriggerEnter(other);
            if (classname.StartsWith("trigger_push"))
            {
                // Call directly
                // if (other.gameObject.TryGetComponent<CharacterPush>(out var charPush))
                //     charPush.Push(GetTargetLocation(), speed);
               
                // Use delegate
                object[] args = new object[2];
                args[0] = worldspawn.GetTargetLocation(target);
                args[1] = speed;
                other.SendMessage("Push", args);
            } 
            else if (classname.StartsWith("trigger_teleport"))
            {
                if (other.CompareTag("Player"))
                    // other.SendMessage("Teleport", GetTargetLocation());
                    other.SendMessage("Teleport", worldspawn.GetTargetLocation(target));
            }
        }

        public override void EntityTriggerExit(Collider other)
        {
            base.EntityTriggerExit(other);
            // if (classname.StartsWith("trigger_push"))
            // {
            // }
        }

        // private Vector3 GetTargetLocation()
        // {
        //     Debug.Log("GET TARGET: " + target);
        //     if (worldspawn.targets.ContainsKey(target))
        //         return worldspawn.targets[target].First().transform.position;
        //     
        //     return Vector3.up;
        // }
    }
}
